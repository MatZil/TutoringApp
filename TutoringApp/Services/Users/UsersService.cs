using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using TutoringApp.Data.Models;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<AppUser> _userManager;

        public UsersService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> GetRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);

            return roles.FirstOrDefault();
        }
    }
}
