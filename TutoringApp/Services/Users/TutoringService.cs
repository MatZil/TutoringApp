using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using TutoringApp.Configurations.Auth;
using TutoringApp.Data.Dtos.Users;
using TutoringApp.Data.Models;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Services.Users
{
    public class TutoringService : ITutoringService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<ITutoringService> _logger;
        private readonly ITimeService _timeService;

        public TutoringService(
            UserManager<AppUser> userManager,
            ICurrentUserService currentUserService,
            ILogger<ITutoringService> logger,
            ITimeService timeService)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
            _logger = logger;
            _timeService = timeService;
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
