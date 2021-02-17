using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using TutoringApp.Data.Models;

namespace TutoringAppTests.Setup.Seeds
{
    public static class AppUserSeeder
    {
        public static async Task Seed(UserManager<AppUser> userManager)
        {
            var firstUser = new AppUser
            {
                Email = "matas.zilinskas@ktu.edu",
                UserName = "matas.zilinskas@ktu.edu",
                EmailConfirmed = true,
                FirstName = "Matas",
                LastName = "Zilinskas"
            };

            await userManager.CreateAsync(firstUser, "Password1");
        }
    }
}
