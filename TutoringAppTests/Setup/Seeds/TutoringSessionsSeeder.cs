using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringApp.Data.Models;
using TutoringApp.Data.Models.Enums;

namespace TutoringAppTests.Setup.Seeds
{
    public static class TutoringSessionsSeeder
    {
        public static async Task Seed(
            DbSet<TutoringSession> tutoringSessionSet,
            UserManager<AppUser> userManager)
        {
            var tutoringSessions = new List<TutoringSession>
            {
                new TutoringSession
                {
                    Tutor = userManager.Users.First(u => u.Email == "matas.tutorius1@ktu.edu"),
                    Student = userManager.Users.First(u => u.Email == "matas.zilinskas@ktu.edu"),
                    CreationDate = DateTimeOffset.Now.AddDays(-1),
                    IsReminderSent = false,
                    IsSubscribed = false,
                    ModuleId = 1,
                    SessionDate = DateTimeOffset.Now.AddHours(-2),
                    Status = TutoringSessionStatusEnum.Cancelled,
                    StatusChangeDate = DateTimeOffset.Now.AddDays(-1)
                },
                new TutoringSession
                {
                    Tutor = userManager.Users.First(u => u.Email == "matas.tutorius1@ktu.edu"),
                    Student = userManager.Users.First(u => u.Email == "matas.zilinskas@ktu.edu"),
                    CreationDate = DateTimeOffset.Now.AddDays(-1),
                    IsReminderSent = true,
                    IsSubscribed = true,
                    ModuleId = 1,
                    SessionDate = DateTimeOffset.Now.AddHours(-1),
                    Status = TutoringSessionStatusEnum.Finished,
                    StatusChangeDate = DateTimeOffset.Now.AddHours(-1)
                },
                new TutoringSession
                {
                    Tutor = userManager.Users.First(u => u.Email == "matas.tutorius1@ktu.edu"),
                    Student = userManager.Users.First(u => u.Email == "matas.zilinskas@ktu.edu"),
                    CreationDate = DateTimeOffset.Now.AddHours(-1),
                    IsReminderSent = false,
                    IsSubscribed = true,
                    ModuleId = 1,
                    SessionDate = DateTimeOffset.Now.AddDays(7).AddHours(-1),
                    Status = TutoringSessionStatusEnum.Upcoming
                }
            };

            await tutoringSessionSet.AddRangeAsync(tutoringSessions);
        }
    }
}
