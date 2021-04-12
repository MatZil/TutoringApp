using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly ILogger<IStudentTutorsService> _logger;
        private readonly IModuleTutorsRepository _moduleTutorsRepository;
        private readonly IRepository<TutoringSession> _tutoringSessionsRepository;
        private readonly IRepository<StudentTutorIgnore> _studentTutorIgnoresRepository;

        public StudentTutorsService(
            IRepository<StudentTutor> studentTutorsRepository,
            ICurrentUserService currentUserService,
            ILogger<IStudentTutorsService> logger,
            IModuleTutorsRepository moduleTutorsRepository,
            IRepository<TutoringSession> tutoringSessionsRepository,
            IRepository<StudentTutorIgnore> studentTutorIgnoresRepository)
        {
            _studentTutorsRepository = studentTutorsRepository;
            _currentUserService = currentUserService;
            _logger = logger;
            _moduleTutorsRepository = moduleTutorsRepository;
            _tutoringSessionsRepository = tutoringSessionsRepository;
            _studentTutorIgnoresRepository = studentTutorIgnoresRepository;
        }

        public async Task AddStudentTutor(string tutorId, int moduleId)
        {
            var currentUserId = _currentUserService.GetUserId();
            if (currentUserId == tutorId)
            {
                var errorMessage = $"Could not add tutor (id='{tutorId}'): you can't add yourself.";
                _logger.LogError(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }

            var tutorExists = await _moduleTutorsRepository.Exists(moduleId, tutorId);
            if (!tutorExists)
            {
                var errorMessage = $"Could not add tutor (id='{tutorId}'): not found.";
                _logger.LogError(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }

            var tutorIgnoresStudent = await _studentTutorIgnoresRepository.Exists(sti => sti.TutorId == tutorId && sti.StudentId == currentUserId);
            if (tutorIgnoresStudent)
            {
                var errorMessage = $"Could not add tutor (id='{tutorId}'): he has ignored you.";
                _logger.LogError(errorMessage);
                throw new InvalidOperationException(errorMessage);
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
