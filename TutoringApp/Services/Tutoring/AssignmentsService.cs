using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TutoringApp.Data.Dtos.Tutoring.Assignments;
using TutoringApp.Data.Models;
using TutoringApp.Data.Models.JoiningTables;
using TutoringApp.Infrastructure.Repositories;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Services.Tutoring
{
    public class AssignmentsService : IAssignmentsService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IRepository<Assignment> _assignmentsRepository;
        private readonly IRepository<StudentTutor> _studentTutorsRepository;
        private readonly IFilesService _filesService;

        private const string AssignmentsRoot = "Assignments/";

        public AssignmentsService(
            ICurrentUserService currentUserService,
            IRepository<Assignment> assignmentsRepository,
            IRepository<StudentTutor> studentTutorsRepository,
            IFilesService filesService)
        {
            _currentUserService = currentUserService;
            _assignmentsRepository = assignmentsRepository;
            _studentTutorsRepository = studentTutorsRepository;
            _filesService = filesService;
        }

        public async Task UploadAssignments(int moduleId, string studentId, IFormFileCollection formFiles)
        {
            var tutorId = _currentUserService.GetUserId();

            await ValidateAssignmentsUpload(moduleId, studentId, formFiles);

            _filesService.CreateDirectory($"{AssignmentsRoot}{moduleId}");
            _filesService.CreateDirectory($"{AssignmentsRoot}{moduleId}/{tutorId}");
            _filesService.CreateDirectory($"{AssignmentsRoot}{moduleId}/{tutorId}/{studentId}");

            var assignmentEntities = new List<Assignment>();
            foreach (var formFile in formFiles)
            {
                await _filesService.UploadFile(formFile, $"{AssignmentsRoot}{moduleId}/{tutorId}/{studentId}/{formFile.FileName}");

                assignmentEntities.Add(new Assignment
                {
                    AssignmentFileName = formFile.FileName,
                    StudentId = studentId,
                    TutorId = tutorId,
                    ModuleId = moduleId
                });
            }

            await _assignmentsRepository.CreateMany(assignmentEntities);
        }

        private async Task ValidateAssignmentsUpload(int moduleId, string studentId, IFormFileCollection formFiles)
        {
            var tutorId = _currentUserService.GetUserId();

            var studentTutorExists = await _studentTutorsRepository.Exists(st =>
                st.ModuleId == moduleId
                && st.StudentId == studentId
                && st.TutorId == tutorId);

            if (!studentTutorExists)
            {
                throw new InvalidOperationException("Could not update assignments: you are not a tutor in this module.");
            }

            if (formFiles.Any(f => f.Length > 5242880))
            {
                throw new InvalidOperationException("Could not update assignments: files are too big.");
            }

            var existingAssignments = await _assignmentsRepository.GetFiltered(a =>
                a.ModuleId == moduleId
                && a.TutorId == tutorId
                && a.StudentId == studentId
                && formFiles.Select(formFile => formFile.FileName).Contains(a.AssignmentFileName)
            );

            if (existingAssignments.Any())
            {
                throw new InvalidOperationException("Could not update assignments: one of the files already exist!");
            }
        }

        public async Task<IEnumerable<AssignmentDto>> GetAssignments(int moduleId, string tutorId, string studentId)
        {
            var existingAssignments = await _assignmentsRepository.GetFiltered(a =>
                a.ModuleId == moduleId
                && a.TutorId == tutorId
                && a.StudentId == studentId
            );

            return existingAssignments.Select(a => new AssignmentDto
            {
                Id = a.Id,
                FileName = a.AssignmentFileName,
                SubmissionFileName = a.SubmissionFileName,
                SubmissionEvaluation = a.SubmissionEvaluation
            });
        }

        public async Task UploadSubmission(int assignmentId, IFormFileCollection formFiles)
        {
            var assignment = await ValidateSubmissionUpload(assignmentId, formFiles);

            if (!assignment.SubmissionFileName.IsNullOrEmpty())
            {
                _filesService.DeleteFile($"{AssignmentsRoot}{assignment.ModuleId}/{assignment.TutorId}/{assignment.StudentId}/Submissions/{assignment.SubmissionFileName}");
                assignment.SubmissionEvaluation = null;
            }

            _filesService.CreateDirectory($"{AssignmentsRoot}{assignment.ModuleId}/{assignment.TutorId}/{assignment.StudentId}/Submissions");
            await _filesService.UploadFile(formFiles[0], $"{AssignmentsRoot}{assignment.ModuleId}/{assignment.TutorId}/{assignment.StudentId}/Submissions/{formFiles[0].FileName}");

            assignment.SubmissionFileName = formFiles[0].FileName;
            await _assignmentsRepository.Update(assignment);
        }

        private async Task<Assignment> ValidateSubmissionUpload(int assignmentId, IFormFileCollection formFiles)
        {
            var assignment = await _assignmentsRepository.GetById(assignmentId);

            if (formFiles.Count != 1)
            {
                throw new InvalidOperationException("Could not upload submission: you may only upload a single file.");
            }

            if (formFiles[0].Length > 5242880)
            {
                throw new InvalidOperationException("Could not upload submission: file is too large.");
            }

            if (assignment is null)
            {
                throw new InvalidOperationException("Could not upload submission: assignment does not exist.");
            }

            var currentUserId = _currentUserService.GetUserId();
            if (assignment.StudentId != currentUserId)
            {
                throw new InvalidOperationException("Could not upload submission: you are not the student of this assignment.");
            }

            return assignment;
        }

        public async Task EvaluateSubmission(int assignmentId, int evaluation)
        {
            var assignment = await _assignmentsRepository.GetById(assignmentId);
            if (assignment is null)
            {
                throw new InvalidOperationException("Could not evaluate assignment: it does not exist.");
            }

            if (assignment.TutorId != _currentUserService.GetUserId())
            {
                throw new InvalidOperationException("Could not evaluate assignment: you are not the tutor of this assignment.");
            }

            if (evaluation < 0 || evaluation > 10)
            {
                throw new InvalidOperationException("Could not evaluate assignment: evaluation value is invalid.");
            }

            assignment.SubmissionEvaluation = evaluation;

            await _assignmentsRepository.Update(assignment);
        }

        public async Task DeleteAssignment(int assignmentId)
        {
            var assignment = await _assignmentsRepository.GetById(assignmentId);
            if (assignment is null)
            {
                return;
            }

            if (assignment.TutorId != _currentUserService.GetUserId())
            {
                throw new InvalidOperationException("Could not delete assignment: you are not the tutor of this assignment.");
            }

            _filesService.DeleteFile($"{AssignmentsRoot}{assignment.ModuleId}/{assignment.TutorId}/{assignment.StudentId}/{assignment.AssignmentFileName}");
            if (!assignment.SubmissionFileName.IsNullOrEmpty())
            {
                _filesService.DeleteFile($"{AssignmentsRoot}{assignment.ModuleId}/{assignment.TutorId}/{assignment.StudentId}/Submissions/{assignment.SubmissionFileName}");
            }

            await _assignmentsRepository.Delete(assignment);
        }

        public async Task<Stream> DownloadAssignmentFile(int assignmentId, string fileName)
        {
            var assignment = await _assignmentsRepository.GetById(assignmentId);
            if (assignment is null)
            {
                throw new InvalidOperationException("Could not download file: assignment does not exist.");
            }

            var currentUserId = _currentUserService.GetUserId();
            if (assignment.TutorId != currentUserId && assignment.StudentId != currentUserId)
            {
                throw new InvalidOperationException("Could not download file: you do not belong to this assignment.");
            }

            if (assignment.AssignmentFileName != fileName && assignment.SubmissionFileName != fileName)
            {
                throw new InvalidOperationException("Could not download file: it does not exist.");
            }

            var name = assignment.AssignmentFileName == fileName
                ? fileName
                : $"Submissions/{fileName}";

            return _filesService.DownloadFile($"{AssignmentsRoot}{assignment.ModuleId}/{assignment.TutorId}/{assignment.StudentId}/{name}");
        }
    }
}
