using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringApp.Data.Models;
using TutoringApp.Data.Models.JoiningTables;

namespace TutoringAppTests.Setup.Seeds
{
    public static class ModuleTutorSeeder
    {
        public static async Task Seed(
            DbSet<ModuleTutor> moduleTutorSet,
            UserManager<AppUser> userManager
            )
        {
            var moduleTutors = new List<ModuleTutor>
            {
                new ModuleTutor
                {
                    ModuleId = 1,
                    Tutor = userManager.Users.First(u => u.Email == "matas.tutorius1@ktu.edu")
                },
                new ModuleTutor
                {
                    ModuleId = 1,
                    Tutor = userManager.Users.First(u => u.Email == "matas.tutorius2@ktu.edu")
                },
                new ModuleTutor
                {
                    ModuleId = 2,
                    Tutor = userManager.Users.First(u => u.Email == "matas.tutorius3@ktu.edu")
                },
                new ModuleTutor
                {
                    ModuleId = 3,
                    Tutor = userManager.Users.First(u => u.Email == "matas.tutorius3@ktu.edu")
                }
            };

            await moduleTutorSet.AddRangeAsync(moduleTutors);
        }
    }
}
