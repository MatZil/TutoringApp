using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringApp.Data.Models;

namespace TutoringAppTests.Setup.Seeds
{
    public static class ChatSeeder
    {
        public static async Task Seed(DbSet<ChatMessage> chatMessageSet, UserManager<AppUser> userManager)
        {
            var chatMessages = new List<ChatMessage>
            {
                new ChatMessage
                {
                    Sender = userManager.Users.First(u => u.Email == "matas.tutorius1@ktu.edu"),
                    Receiver = userManager.Users.First(u => u.Email == "matas.zilinskas@ktu.edu"),
                    Content = "Hello",
                    Timestamp = DateTimeOffset.Now.AddMinutes(-10),
                    ModuleId = 1
                },
                new ChatMessage
                {
                    Receiver = userManager.Users.First(u => u.Email == "matas.tutorius1@ktu.edu"),
                    Sender = userManager.Users.First(u => u.Email == "matas.zilinskas@ktu.edu"),
                    Content = "World",
                    Timestamp = DateTimeOffset.Now.AddMinutes(-8),
                    ModuleId = 1
                },
                new ChatMessage
                {
                    Sender = userManager.Users.First(u => u.Email == "matas.tutorius1@ktu.edu"),
                    Receiver = userManager.Users.First(u => u.Email == "matas.zilinskas@ktu.edu"),
                    Content = "Timing is everything :)",
                    Timestamp = DateTimeOffset.Now.AddMinutes(-9),
                    ModuleId = 1
                },
                new ChatMessage
                {
                    Sender = userManager.Users.First(u => u.Email == "matas.tutorius2@ktu.edu"),
                    Receiver = userManager.Users.First(u => u.Email == "matas.zilinskas@ktu.edu"),
                    Content = "Tutor Ended",
                    Timestamp = DateTimeOffset.Now.AddDays(-9),
                    ModuleId = 1
                }
            };

            await chatMessageSet.AddRangeAsync(chatMessages);
        }
    }
}
