using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringApp.Configurations.Auth;
using TutoringApp.Data.Dtos.Tutoring;
using TutoringApp.Data.Models;
using TutoringApp.Infrastructure.Repositories;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Services.Tutoring
{
    public class TutoringApplicationsService : ITutoringApplicationsService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<ITutoringApplicationsService> _logger;
        private readonly ITimeService _timeService;
        private readonly IRepository<TutoringApplication> _tutoringApplicationsRepository;

        public TutoringApplicationsService(
            UserManager<AppUser> userManager,
            ICurrentUserService currentUserService,
            ILogger<ITutoringApplicationsService> logger,
            ITimeService timeService,
            IRepository<TutoringApplication> tutoringApplicationsRepository)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
            _logger = logger;
            _timeService = timeService;
            _tutoringApplicationsRepository = tutoringApplicationsRepository;
        }

        public async Task ApplyForTutoring(int moduleId, TutoringApplicationNewDto tutoringApplicationNew)
        {
            var currentUserId = _currentUserService.GetUserId();
            var currentUser = await _userManager.Users
                .Include(u => u.TutorModules)
                .Include(u => u.TutoringApplications)
                .FirstOrDefaultAsync(u => u.Id == currentUserId);

            await ValidateTutoringApplication(currentUser, currentUserId, moduleId);

            var tutoringApplication = new TutoringApplication
            {
                ModuleId = moduleId,
                StudentId = currentUserId,
                RequestDate = _timeService.GetCurrentTime(),
                MotivationalLetter = tutoringApplicationNew.MotivationalLetter
            };
            currentUser.TutoringApplications.Add(tutoringApplication);

            await _userManager.UpdateAsync(currentUser);
        }

        public async Task ResignFromTutoring(int moduleId)
        {
            var currentUserId = _currentUserService.GetUserId();
            var currentUser = await _userManager.Users
                .Include(u => u.TutorModules)
                .FirstOrDefaultAsync(u => u.Id == currentUserId);

            currentUser.TutorModules = currentUser.TutorModules.Where(tm => tm.ModuleId != moduleId).ToList();
            await _userManager.UpdateAsync(currentUser);
        }

        public async Task<IEnumerable<TutoringApplicationDto>> GetTutoringApplications()
        {
            var tutoringApplications = await _tutoringApplicationsRepository.GetAll();

            var tutoringApplicationDtos = tutoringApplications.Select(ta => new TutoringApplicationDto
            {
                Id = ta.Id,
                ModuleName = ta.Module.Name,
                Email = ta.Student.Email,
                Faculty = ta.Student.Faculty,
                MotivationalLetter = ta.MotivationalLetter,
                RequestDate = ta.RequestDate,
                StudentCycle = ta.Student.StudentCycle,
                StudentYear = ta.Student.StudentYear,
                StudentName = $"{ta.Student.FirstName} {ta.Student.LastName}",
                StudyBranch = ta.Student.StudyBranch
            });

            return tutoringApplicationDtos;
        }

        private async Task ValidateTutoringApplication(AppUser user, string userId, int moduleId)
        {
            if (user is null)
            {
                var errorMessage = $"Could not apply for tutoring: user (id='{userId}') was not found.";
                _logger.LogError(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }

            if (user.TutorModules.Any(tm => tm.ModuleId == moduleId))
            {
                var errorMessage = $"Could not apply for tutoring: user (id='{userId}') is already a tutor in this module.";
                _logger.LogError(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }

            var isStudent = await _userManager.IsInRoleAsync(user, AppRoles.Student);
            if (!isStudent)
            {
                var errorMessage = $"Could not apply for tutoring: user (id='{userId}') is not a student.";
                _logger.LogError(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }
        }
    }
}
