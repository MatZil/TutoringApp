using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using TutoringApp.Configurations.Auth;

namespace TutoringAppTests.Setup.Seeds
{
    public static class RoleSeeder
    {
        public static async Task Seed(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole { Name = AppRoles.Student });
            await roleManager.CreateAsync(new IdentityRole { Name = AppRoles.Admin });
            await roleManager.CreateAsync(new IdentityRole { Name = AppRoles.Lecturer });
        }
    }
}
