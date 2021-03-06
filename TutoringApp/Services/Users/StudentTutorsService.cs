﻿using IdentityServer4.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringApp.Data.Dtos.Tutoring.TutoringSessions;
using TutoringApp.Data.Dtos.Users;
using TutoringApp.Data.Models;
using TutoringApp.Data.Models.Enums;
using TutoringApp.Data.Models.JoiningTables;
using TutoringApp.Infrastructure.Repositories;
using TutoringApp.Infrastructure.Repositories.Interfaces;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Services.Users
{
    public class StudentTutorsService : IStudentTutorsService
    {
        private readonly IRepository<StudentTutor> _studentTutorsRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IModuleTutorsRepository _moduleTutorsRepository;
        private readonly IRepository<TutoringSession> _tutoringSessionsRepository;
        private readonly IRepository<StudentTutorIgnore> _studentTutorIgnoresRepository;
        private readonly IHubsService _hubsService;

        public StudentTutorsService(
            IRepository<StudentTutor> studentTutorsRepository,
            ICurrentUserService currentUserService,
            IModuleTutorsRepository moduleTutorsRepository,
            IRepository<TutoringSession> tutoringSessionsRepository,
            IRepository<StudentTutorIgnore> studentTutorIgnoresRepository,
            IHubsService hubsService)
        {
            _studentTutorsRepository = studentTutorsRepository;
            _currentUserService = currentUserService;
            _moduleTutorsRepository = moduleTutorsRepository;
            _tutoringSessionsRepository = tutoringSessionsRepository;
            _studentTutorIgnoresRepository = studentTutorIgnoresRepository;
            _hubsService = hubsService;
        }

        public async Task AddStudentTutor(string tutorId, int moduleId)
        {
            var currentUserId = _currentUserService.GetUserId();
            if (currentUserId == tutorId)
            {
                throw new InvalidOperationException($"Could not add tutor (id='{tutorId}'): you can't add yourself.");
            }

            var tutorExists = await _moduleTutorsRepository.Exists(moduleId, tutorId);
            if (!tutorExists)
            {
                throw new InvalidOperationException($"Could not add tutor (id='{tutorId}'): not found.");
            }

            var tutorIgnoresStudent = await _studentTutorIgnoresRepository.Exists(sti => sti.TutorId == tutorId && sti.StudentId == currentUserId);
            if (tutorIgnoresStudent)
            {
                throw new InvalidOperationException($"Could not add tutor (id='{tutorId}'): he has ignored you.");
            }

            var studentTutor = new StudentTutor
            {
                StudentId = currentUserId,
                TutorId = tutorId,
                ModuleId = moduleId
            };

            await _studentTutorsRepository.Create(studentTutor);
        }

        public async Task RemoveStudentTutor(string tutorId, int moduleId)
        {
            var currentUserId = _currentUserService.GetUserId();
            var studentTutorQuery = await _studentTutorsRepository.GetFiltered(st =>
                st.StudentId == currentUserId
                && st.TutorId == tutorId
                && st.ModuleId == moduleId);

            var studentTutor = studentTutorQuery.FirstOrDefault();

            if (studentTutor != null)
            {
                await _studentTutorsRepository.Delete(studentTutor);

                await RemoveUpcomingTutoringSessions(currentUserId, tutorId, moduleId);

                var studentNotification = new TutoringSessionFinishedNotificationDto { ParticipantId = tutorId };
                await _hubsService.SendSessionFinishedNotificationToUser(currentUserId, studentNotification);

                var tutorNotification = new TutoringSessionFinishedNotificationDto { ParticipantId = currentUserId };
                await _hubsService.SendSessionFinishedNotificationToUser(tutorId, tutorNotification);
            }
        }

        public async Task RemoveTutorStudent(string studentId, int moduleId)
        {
            var currentUserId = _currentUserService.GetUserId();
            var tutorStudentQuery = await _studentTutorsRepository.GetFiltered(st =>
                st.StudentId == studentId
                && st.TutorId == currentUserId
                && st.ModuleId == moduleId);

            var tutorStudent = tutorStudentQuery.FirstOrDefault();

            if (tutorStudent != null)
            {
                await _studentTutorsRepository.Delete(tutorStudent);

                await RemoveUpcomingTutoringSessions(studentId, currentUserId, moduleId);

                var tutorNotification = new TutoringSessionFinishedNotificationDto { ParticipantId = studentId };
                await _hubsService.SendSessionFinishedNotificationToUser(currentUserId, tutorNotification);

                var studentNotification = new TutoringSessionFinishedNotificationDto { ParticipantId = currentUserId };
                await _hubsService.SendSessionFinishedNotificationToUser(studentId, studentNotification);
            }
        }

        public async Task IgnoreTutorStudent(string studentId)
        {
            var currentUserId = _currentUserService.GetUserId();
            var ignore = new StudentTutorIgnore
            {
                StudentId = studentId,
                TutorId = currentUserId
            };

            await _studentTutorIgnoresRepository.Create(ignore);

            await RemoveUpcomingTutoringSessions(studentId, currentUserId);

            var studentTutorEntities = await _studentTutorsRepository.GetFiltered(st => st.TutorId == currentUserId && st.StudentId == studentId);
            await _studentTutorsRepository.DeleteMany(studentTutorEntities);

            var tutorNotification = new TutoringSessionFinishedNotificationDto { ParticipantId = studentId };
            await _hubsService.SendSessionFinishedNotificationToUser(currentUserId, tutorNotification);

            var studentNotification = new TutoringSessionFinishedNotificationDto { ParticipantId = currentUserId };
            await _hubsService.SendSessionFinishedNotificationToUser(studentId, studentNotification);
        }

        public async Task<IEnumerable<IgnoredStudentDto>> GetIgnoredStudents()
        {
            var currentUserId = _currentUserService.GetUserId();
            var ignores = await _studentTutorIgnoresRepository.GetFiltered(i => i.TutorId == currentUserId);

            return ignores.Select(i => new IgnoredStudentDto
            {
                Id = i.StudentId,
                Name = $"{i.Student.FirstName} {i.Student.LastName}"
            });
        }

        public async Task UnignoreStudent(string studentId)
        {
            var ignoreQuery = await _studentTutorIgnoresRepository.GetFiltered(i =>
                i.TutorId == _currentUserService.GetUserId()
                && i.StudentId == studentId
                );

            var ignore = ignoreQuery.FirstOrDefault();
            if (ignore != null)
            {
                await _studentTutorIgnoresRepository.Delete(ignore);
            }
        }

        public async Task<bool> StudentTutorExists(string studentId, string tutorId)
        {
            if (!studentId.IsNullOrEmpty() && !tutorId.IsNullOrEmpty())
            {
                throw new InvalidOperationException("Invalid request.");
            }

            var userId = _currentUserService.GetUserId();
            var exists = await _studentTutorsRepository.Exists(st => 
                (!studentId.IsNullOrEmpty() && st.TutorId == userId && st.StudentId == studentId)
                || (!tutorId.IsNullOrEmpty() && st.StudentId == userId && st.TutorId == tutorId)
            );

            return exists;
        }

        private async Task RemoveUpcomingTutoringSessions(string studentId, string tutorId, int? moduleId = null)
        {
            var sessionsQuery = await _tutoringSessionsRepository.GetFiltered(ts =>
                (moduleId == null || ts.ModuleId == moduleId)
                && ts.StudentId == studentId
                && ts.TutorId == tutorId
                && ts.Status == TutoringSessionStatusEnum.Upcoming
            );

            var sessions = sessionsQuery.ToList();

            if (sessions.Count > 0)
            {
                await _tutoringSessionsRepository.DeleteMany(sessions);
            }
        }
    }
}
