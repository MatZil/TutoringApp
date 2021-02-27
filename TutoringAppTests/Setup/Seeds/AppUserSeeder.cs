using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using TutoringApp.Configurations.Auth;
using TutoringApp.Data.Models;
using TutoringApp.Data.Models.Enums;

namespace TutoringAppTests.Setup.Seeds
{
    public static class AppUserSeeder
    {
        public static async Task Seed(UserManager<AppUser> userManager)
        {
            var firstStudent = new AppUser
            {
                Email = "matas.zilinskas@ktu.edu",
                UserName = "matas.zilinskas@ktu.edu",
                EmailConfirmed = true,
                IsConfirmed = true,
                FirstName = "Matas",
                LastName = "Zilinskas",
                Faculty = "Informatics",
                StudyBranch = "Software Systems",
                StudentCycle = StudentCycleEnum.Bachelor,
                StudentYear = StudentYearEnum.FourthYear,
                IsTutor = false
            };

            await userManager.CreateAsync(firstStudent, "Password1");
            await userManager.AddToRoleAsync(firstStudent, AppRoles.Student);

            var firstTutor = new AppUser
            {
                Email = "matas.tutorius1@ktu.edu",
                UserName = "matas.tutorius1@ktu.edu",
                EmailConfirmed = true,
                IsConfirmed = true,
                FirstName = "Matas",
                LastName = "FirstTutor",
                Faculty = "Informatics",
                StudyBranch = "Software Systems",
                StudentCycle = StudentCycleEnum.Bachelor,
                StudentYear = StudentYearEnum.FourthYear,
                IsTutor = true
            };

            await userManager.CreateAsync(firstTutor, "Password1");
            await userManager.AddToRoleAsync(firstTutor, AppRoles.Tutor);

            var secondTutor = new AppUser
            {
                Email = "matas.tutorius2@ktu.edu",
                UserName = "matas.tutorius2@ktu.edu",
                EmailConfirmed = true,
                IsConfirmed = true,
                FirstName = "Matas",
                LastName = "SecondTutor",
                Faculty = "Informatics",
                StudyBranch = "Software Systems",
                StudentCycle = StudentCycleEnum.Bachelor,
                StudentYear = StudentYearEnum.FourthYear,
                IsTutor = true
            };

            await userManager.CreateAsync(secondTutor, "Password1");
            await userManager.AddToRoleAsync(secondTutor, AppRoles.Tutor);

            var thirdTutor = new AppUser
            {
                Email = "matas.tutorius3@ktu.edu",
                UserName = "matas.tutorius3@ktu.edu",
                EmailConfirmed = true,
                IsConfirmed = true,
                FirstName = "Matas",
                LastName = "ThirdTutor",
                Faculty = "Informatics",
                StudyBranch = "Software Systems",
                StudentCycle = StudentCycleEnum.Bachelor,
                StudentYear = StudentYearEnum.FourthYear,
                IsTutor = true
            };

            await userManager.CreateAsync(thirdTutor, "Password1");
            await userManager.AddToRoleAsync(thirdTutor, AppRoles.Tutor);

            var firstAdmin = new AppUser
            {
                Email = "matas.admin@ktu.edu",
                UserName = "matas.admin@ktu.edu",
                EmailConfirmed = true,
                IsConfirmed = true,
                FirstName = "Matas",
                LastName = "Zilinskas",
                Faculty = "Informatics",
                StudyBranch = "Software Systems",
                StudentCycle = StudentCycleEnum.Bachelor,
                StudentYear = StudentYearEnum.FourthYear,
                IsTutor = false
            };

            await userManager.CreateAsync(firstAdmin, "Password1");
            await userManager.AddToRoleAsync(firstAdmin, AppRoles.Admin);

            var firstLecturer = new AppUser
            {
                Email = "matas.lecturer@ktu.edu",
                UserName = "matas.lecturer@ktu.edu",
                EmailConfirmed = true,
                IsConfirmed = true,
                FirstName = "Matas",
                LastName = "Zilinskas",
                Faculty = "Informatics",
                StudyBranch = "Software Systems",
                StudentCycle = StudentCycleEnum.Bachelor,
                StudentYear = StudentYearEnum.FourthYear,
                IsTutor = false
            };

            await userManager.CreateAsync(firstLecturer, "Password1");
            await userManager.AddToRoleAsync(firstLecturer, AppRoles.Lecturer);

            var emailUnconfirmedStudent = new AppUser
            {
                Email = "matas.emailunconfirmed@ktu.edu",
                UserName = "matas.emailunconfirmed@ktu.edu",
                EmailConfirmed = false,
                IsConfirmed = false,
                FirstName = "Matas",
                LastName = "Zilinskas",
                Faculty = "Informatics",
                StudyBranch = "Software Systems",
                StudentCycle = StudentCycleEnum.Bachelor,
                StudentYear = StudentYearEnum.FourthYear,
                IsTutor = false
            };

            await userManager.CreateAsync(emailUnconfirmedStudent, "Password1");
            await userManager.AddToRoleAsync(emailUnconfirmedStudent, AppRoles.Student);

            var unconfirmedStudent = new AppUser
            {
                Email = "matas.unconfirmed@ktu.edu",
                UserName = "matas.unconfirmed@ktu.edu",
                EmailConfirmed = true,
                IsConfirmed = false,
                FirstName = "Matas",
                LastName = "Zilinskas",
                Faculty = "Informatics",
                StudyBranch = "Software Systems",
                StudentCycle = StudentCycleEnum.Bachelor,
                StudentYear = StudentYearEnum.FourthYear,
                IsTutor = false
            };

            await userManager.CreateAsync(unconfirmedStudent, "Password1");
            await userManager.AddToRoleAsync(unconfirmedStudent, AppRoles.Student);
        }
    }
}
