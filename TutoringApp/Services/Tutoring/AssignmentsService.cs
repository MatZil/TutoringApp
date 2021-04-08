using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TutoringApp.Data.Models;
using TutoringApp.Infrastructure.Repositories;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Services.Tutoring
{
    public class AssignmentsService : IAssignmentsService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IRepository<Assignment> _assignmentsRepository;
        private readonly ITimeService _timeService;

        private const string AssignmentsRoot = "Assignments/";

        public AssignmentsService(
            ICurrentUserService currentUserService,
            IRepository<Assignment> assignmentsRepository,
            ITimeService timeService)
        {
            _currentUserService = currentUserService;
            _assignmentsRepository = assignmentsRepository;
            _timeService = timeService;
        }

        public async Task UpdateAssignments(int moduleId, string studentId, IFormFileCollection assignments)
        {
            var tutorId = _currentUserService.GetUserId();

            var existingAssignments = await _assignmentsRepository.GetFiltered(a =>
                a.ModuleId == moduleId
                && a.TutorId == tutorId
                && a.StudentId == studentId
                && assignments.Select(formFile => formFile.FileName).Contains(a.AssignmentFileName)
            );

            if (existingAssignments.Any())
            {
                throw new InvalidOperationException("Could not update assignments: one of the files already exist!");
            }

            Directory.CreateDirectory($"{AssignmentsRoot}{moduleId}");
            Directory.CreateDirectory($"{AssignmentsRoot}{moduleId}/{tutorId}");
            Directory.CreateDirectory($"{AssignmentsRoot}{moduleId}/{tutorId}/{studentId}");

            var assignmentEntities = new List<Assignment>();
            foreach (var assignment in assignments)
            {
                await using var stream = new FileStream($"{AssignmentsRoot}{moduleId}/{tutorId}/{studentId}/{assignment.FileName}", FileMode.Create);

                await assignment.CopyToAsync(stream);

                assignmentEntities.Add(new Assignment
                {
                    AssignmentFileName = assignment.FileName,
                    CreationDate = _timeService.GetCurrentTime(),
                    StudentId = studentId,
                    TutorId = tutorId,
                    ModuleId = moduleId
                });
            }

            await _assignmentsRepository.CreateMany(assignmentEntities);
        }
    }
}
