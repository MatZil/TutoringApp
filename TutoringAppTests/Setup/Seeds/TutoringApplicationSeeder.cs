using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringApp.Data.Models;

namespace TutoringAppTests.Setup.Seeds
{
    public static class TutoringApplicationSeeder
    {
        public static async Task Seed(UserManager<AppUser> userManager, DbSet<TutoringApplication> tutoringApplicationSet)
        {
            var tutoringApplications = new List<TutoringApplication>
            {
                new TutoringApplication
                {
                    ModuleId = 1,
                    Student = userManager.Users.First(u => u.Email == "matas.zilinskas@ktu.edu"),
                    RequestDate = DateTimeOffset.Now.AddDays(-2),
                    MotivationalLetter = "First motivational letter"
                },
                new TutoringApplication
                {
                    ModuleId = 2,
                    Student = userManager.Users.First(u => u.Email == "matas.zilinskas@ktu.edu"),
                    RequestDate = DateTimeOffset.Now.AddDays(-1),
                    MotivationalLetter = "Second motivational letter"
                }
            };

            await tutoringApplicationSet.AddRangeAsync(tutoringApplications);
        }
    }
}
