using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using TutoringApp.Configurations.Auth;
using TutoringApp.Data.Models;

namespace TutoringAppTests.Setup.Seeds
{
    public static class AppUserSeeder
    {
        public static async Task Seed(UserManager<AppUser> userManager)
        {
            var firstStudent= new AppUser
            {
                Email = "matas.zilinskas@ktu.edu",
                UserName = "matas.zilinskas@ktu.edu",
                EmailConfirmed = true,
                FirstName = "Matas",
                LastName = "Zilinskas"
            };

            await userManager.CreateAsync(firstStudent, "Password1");
            await userManager.AddToRoleAsync(firstStudent, AppRoles.Student);

            var firstTutor= new AppUser
            {
                Email = "matas.tutorius@ktu.edu",
                UserName = "matas.tutorius@ktu.edu",
                EmailConfirmed = true,
                FirstName = "Matas",
                LastName = "Zilinskas"
            };

            await userManager.CreateAsync(firstTutor, "Password1");
            await userManager.AddToRoleAsync(firstTutor, AppRoles.Tutor);

            var firstAdmin = new AppUser
            {
                Email = "matas.admin@ktu.edu",
                UserName = "matas.admin@ktu.edu",
                EmailConfirmed = true,
                FirstName = "Matas",
                LastName = "Zilinskas"
            };

            await userManager.CreateAsync(firstAdmin, "Password1");
            await userManager.AddToRoleAsync(firstAdmin, AppRoles.Admin);

            var firstLecturer= new AppUser
            {
                Email = "matas.lecturer@ktu.edu",
                UserName = "matas.lecturer@ktu.edu",
                EmailConfirmed = true,
                FirstName = "Matas",
                LastName = "Zilinskas"
            };

            await userManager.CreateAsync(firstLecturer, "Password1");
            await userManager.AddToRoleAsync(firstLecturer, AppRoles.Lecturer);
        }
    }
}
