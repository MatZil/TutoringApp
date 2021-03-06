﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TutoringApp.Data.Models;

namespace TutoringApp.Configurations.Auth
{
    public static class InitialAdminSeeder
    {
        public static void SeedInitialAdmin(IConfiguration configuration, UserManager<AppUser> userManager)
        {
            if (userManager.GetUsersInRoleAsync(AppRoles.Admin).Result.Count <= 0)
            {
                var user = new AppUser
                {
                    FirstName = configuration.GetValue<string>("InitialAdminCredentials:FirstName"),
                    LastName = configuration.GetValue<string>("InitialAdminCredentials:LastName"),
                    UserName = configuration.GetValue<string>("InitialAdminCredentials:Email"),
                    Email = configuration.GetValue<string>("InitialAdminCredentials:Email"),
                    EmailConfirmed = true,
                    IsConfirmed = true
                };
                var result = userManager.CreateAsync(user, configuration.GetValue<string>("InitialAdminCredentials:Password")).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user,AppRoles.Admin).Wait();
                }
            }
        }
    }
}
