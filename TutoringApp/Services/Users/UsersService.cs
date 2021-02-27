using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringApp.Data.Dtos.Users;
using TutoringApp.Data.Models;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ILogger<IUsersService> _logger;

        public UsersService(
            UserManager<AppUser> userManager,
            ICurrentUserService currentUserService,
            IMapper mapper,
            ILogger<IUsersService> logger)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> GetRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);

            return roles.FirstOrDefault();
        }

        public async Task<IEnumerable<TutorDto>> GetTutors(int moduleId)
        {
            var userId = _currentUserService.GetUserId();
            var tutors = await _userManager.Users
                .Include(u => u.TutorEvaluations)
                .Include(u => u.TutoredSessions)
                .Where(u => u.IsTutor)
                .Where(u => u.TutorModules.Any(tm => tm.ModuleId == moduleId))
                .Where(u => u.IgnoresToStudents.All(its => its.StudentId != userId))
                .OrderByDescending(u => u.TutoredSessions.Count)
                .ToListAsync();

            var tutorDtos = tutors.Select(t => new TutorDto
            {
                Id = t.Id,
                Name = $"{t.FirstName} {t.LastName}",
                StudentCycle = t.StudentCycle,
                StudentYear = t.StudentYear,
                Faculty = t.Faculty,
                StudyBranch = t.StudyBranch,
                TutoringSessionCount = t.TutoredSessions.Count,
                AverageScore = t.TutorEvaluations
                    .Select(te => (double)te.Evaluation)
                    .DefaultIfEmpty(0.0)
                    .Average()
            });

            return tutorDtos;
        }

        public async Task<IEnumerable<UserUnconfirmedDto>> GetUnconfirmedUsers()
        {
            var users = await _userManager.Users
                .Where(u => u.EmailConfirmed)
                .Where(u => !u.IsConfirmed)
                .ToListAsync();

            var userDtos = _mapper.Map<IEnumerable<UserUnconfirmedDto>>(users);
            return userDtos;
        }

        public async Task<string> ConfirmUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            ValidateUserConfirmation(user, id);

            user.IsConfirmed = true;
            await _userManager.UpdateAsync(user);

            return user.Email;
        }

        private void ValidateUserConfirmation(AppUser user, string id)
        {
            if (user is null)
            {
                var errorMessage = $"Could not confirm user (id='{id}'): User does not exist.";
                _logger.LogError(errorMessage);
                throw new InvalidOperationException(id);
            }

            if (!user.EmailConfirmed)
            {
                var errorMessage = $"Could not confirm user (id='{id}'): User hasn't confirmed his email yet.";
                _logger.LogError(errorMessage);
                throw new InvalidOperationException(id);
            }

            if (user.IsConfirmed)
            {
                var errorMessage = $"Could not confirm user (id='{id}'): User is already confirmed.";
                _logger.LogError(errorMessage);
                throw new InvalidOperationException(id);
            }
        }
    }
}
