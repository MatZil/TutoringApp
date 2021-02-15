using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using TutoringApp.Data.Models;

namespace TutoringAppTests.Configuration.Seeds
{
    public static class AppUserSeeder
    {
        public static async Task Seed(UserManager<AppUser> userManager)
        {
            var firstUser = new AppUser
            {
                Email = "matzil@ktu.lt",
                UserName = "matzil@ktu.lt",
                EmailConfirmed = true,
                FirstName = "Matas",
                LastName = "Zilinskas"
            };

            await userManager.CreateAsync(firstUser, "Password1");
        }
    }
}
