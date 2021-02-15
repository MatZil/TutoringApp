﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.IO;
using TutoringApp.Configurations.Mapping;
using TutoringApp.Data;
using TutoringApp.Data.Models;
using TutoringAppTests.Setup.Seeds;

namespace TutoringAppTests.Setup
{
    public class UnitTestSetup
    {
        public ApplicationDbContext Context { get; }
        public UserManager<AppUser> UserManager { get; }

        public static IMapper Mapper
        {
            get
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new AuthMappingProfile());
                });

                return config.CreateMapper();
            }
        }

        public UnitTestSetup()
        {
            Context = GetDatabase("TestDatabase");
            UserManager = GetUserManager();

            AppUserSeeder.Seed(UserManager).Wait();

            Context.SaveChanges();
        }

        private static ApplicationDbContext GetDatabase(string databaseName)
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName)
                .UseInternalServiceProvider(serviceProvider)
                .EnableSensitiveDataLogging()
                .Options;

            return new ApplicationDbContext(dbContextOptions);
        }

        private UserManager<AppUser> GetUserManager()
        {
            var userStore = new UserStore<AppUser>(Context);

            var userManager = new UserManager<AppUser>(
                userStore,
                Options.Create(new IdentityOptions()),
                new PasswordHasher<AppUser>(),
                Array.Empty<IUserValidator<AppUser>>(),
                Array.Empty<IPasswordValidator<AppUser>>(),
                null,
                new IdentityErrorDescriber(),
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<AppUser>>>().Object);

            userManager.RegisterTokenProvider("Default", new EmailTokenProvider<AppUser>());

            return userManager;
        }

        public static IConfiguration GetConfiguration()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            return config;
        }
    }
}