using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public UsersService(
            UserManager<AppUser> userManager,
            ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
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
                AverageScore = t.TutorEvaluations.Average(te => (double)te.Evaluation),
                TutoringSessionCount = t.TutoredSessions.Count
            });

            return tutorDtos;
        }
    }
}
