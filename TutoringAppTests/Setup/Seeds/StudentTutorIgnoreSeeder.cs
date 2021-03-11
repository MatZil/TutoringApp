using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringApp.Data.Models;
using TutoringApp.Data.Models.JoiningTables;

namespace TutoringAppTests.Setup.Seeds
{
    public static class StudentTutorIgnoreSeeder
    {
        public static async Task Seed(UserManager<AppUser> userManager, DbSet<StudentTutorIgnore> studentTutorIgnoreSet)
        {
            var studentTutorIgnores = new List<StudentTutorIgnore>
            {
                new StudentTutorIgnore
                {
                    Tutor = userManager.Users.First(u => u.Email == "matas.tutorius3@ktu.edu"),
                    Student = userManager.Users.First(u => u.Email == "matas.zilinskas@ktu.edu")
                }
            };

            await studentTutorIgnoreSet.AddRangeAsync(studentTutorIgnores);
        }
    }
}
