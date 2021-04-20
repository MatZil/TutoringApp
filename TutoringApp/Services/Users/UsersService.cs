using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringApp.Data.Dtos.Users;
using TutoringApp.Data.Models;
using TutoringApp.Data.Models.Enums;
using TutoringApp.Infrastructure.Repositories.Interfaces;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IModuleTutorsRepository _moduleTutorsRepository;

        public UsersService(
            UserManager<AppUser> userManager,
            ICurrentUserService currentUserService,
            IMapper mapper,
            IModuleTutorsRepository moduleTutorsRepository)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _moduleTutorsRepository = moduleTutorsRepository;
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
                .Include(u => u.TutoredSessions)
                .Include(u => u.TutorStudents)
                .Where(u => u.TutorModules.Any(tm => tm.ModuleId == moduleId))
                .Where(u => u.IgnoresToStudents.All(its => its.StudentId != userId))
                .Where(u => u.Id != userId)
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
                TutoringSessionCount = t.TutoredSessions.Count(ts => ts.Status == TutoringSessionStatusEnum.Finished),
                IsAddable = t.TutorStudents.All(st => st.ModuleId != moduleId || st.StudentId != userId),
                AverageScore = Math.Round(
                    t.TutoredSessions
                        .Where(ts => ts.Evaluation != null)
                        .Select(ts => (double)ts.Evaluation.GetValueOrDefault())
                        .DefaultIfEmpty(0.0)
                        .Average(),
                    1
                    )
            });

            return tutorDtos;
        }

        public async Task<IEnumerable<StudentDto>> GetStudents(int moduleId)
        {
            var userId = _currentUserService.GetUserId();
            var students = await _userManager.Users
                .Where(u => u.StudentTutors.Any(st => st.TutorId == userId && st.ModuleId == moduleId))
                .ToListAsync();

            var studentDtos = _mapper.Map<IEnumerable<StudentDto>>(students);

            return studentDtos;
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

        public async Task<string> RejectUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            ValidateUserConfirmation(user, id);

            await _userManager.DeleteAsync(user);

            return user.Email;
        }

        public async Task ResignFromTutoring(int moduleId)
        {
            var currentUserId = _currentUserService.GetUserId();

            await _moduleTutorsRepository.Delete(moduleId, currentUserId);
        }

        public async Task<UserDto> GetUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return new UserDto
            {
                Name = $"{user.FirstName} {user.LastName}"
            };
        }

        private void ValidateUserConfirmation(AppUser user, string id)
        {
            if (user is null)
            {
                throw new InvalidOperationException($"Could not confirm user (id='{id}'): User does not exist.");
            }

            if (!user.EmailConfirmed)
            {
                throw new InvalidOperationException($"Could not confirm user (id='{id}'): User hasn't confirmed his email yet.");
            }

            if (user.IsConfirmed)
            {
                throw new InvalidOperationException($"Could not confirm user (id='{id}'): User is already confirmed.");
            }
        }
    }
}
