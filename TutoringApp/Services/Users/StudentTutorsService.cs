using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using TutoringApp.Data.Models.JoiningTables;
using TutoringApp.Infrastructure.Repositories;
using TutoringApp.Infrastructure.Repositories.Interfaces;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Services.Users
{
    public class StudentTutorsService : IStudentTutorsService
    {
        private readonly IRepository<StudentTutor> _studentTutorsRepository;
        private readonly IStudentTutorIgnoresRepository _studentTutorIgnoresRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<IStudentTutorsService> _logger;

        public StudentTutorsService(
            IRepository<StudentTutor> studentTutorsRepository,
            IStudentTutorIgnoresRepository studentTutorIgnoresRepository,
            ICurrentUserService currentUserService,
            ILogger<IStudentTutorsService> logger)
        {
            _studentTutorsRepository = studentTutorsRepository;
            _studentTutorIgnoresRepository = studentTutorIgnoresRepository;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task AddStudentTutor(string tutorId)
        {
            var currentUserId = _currentUserService.GetUserId();
            var tutorIgnoresStudent = await _studentTutorIgnoresRepository.TutorIgnoresStudent(tutorId, currentUserId);
            if (tutorIgnoresStudent)
            {
                var errorMessage = $"Could not add tutor (id='{tutorId}'): he has ignored you.";
                _logger.LogError(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }

            var studentTutor = new StudentTutor
            {
                StudentId = currentUserId,
                TutorId = tutorId
            };

            await _studentTutorsRepository.Create(studentTutor);
        }

        public async Task RemoveStudentTutor(string tutorId)
        {
            var currentUserId = _currentUserService.GetUserId();
            var studentTutorQuery = await _studentTutorsRepository.GetFiltered(st => st.StudentId == currentUserId && st.TutorId == tutorId);
            var studentTutor = studentTutorQuery.FirstOrDefault();

            if (studentTutor != null)
            {
                await _studentTutorsRepository.Delete(studentTutor);
            }
        }

        public async Task RemoveTutorStudent(string studentId)
        {
            var currentUserId = _currentUserService.GetUserId();
            var tutorStudentQuery = await _studentTutorsRepository.GetFiltered(st => st.StudentId == studentId && st.TutorId == currentUserId);
            var tutorStudent = tutorStudentQuery.FirstOrDefault();

            if (tutorStudent != null)
            {
                await _studentTutorsRepository.Delete(tutorStudent);
            }
        }
    }
}
