using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringApp.Data.Models;
using TutoringApp.Data.Models.JoiningTables;

namespace TutoringAppTests.Setup.Seeds
{
    public static class StudentTutorSeeder
    {
        public static async Task Seed(UserManager<AppUser> userManager, DbSet<StudentTutor> studentTutorSet)
        {
            var studentTutors = new List<StudentTutor>
            {
                new StudentTutor
                {
                    Student = userManager.Users.First(u => u.Email == "matas.zilinskas@ktu.edu"),
                    Tutor = userManager.Users.First(u => u.Email == "matas.tutorius1@ktu.edu"),
                    ModuleId = 1
                }
            };

            await studentTutorSet.AddRangeAsync(studentTutors);
        }
    }
}
