using AutoMapper;
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
        public RoleManager<IdentityRole> RoleManager { get; }

        public static IMapper Mapper
        {
            get
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new AuthMappingProfile());
                    cfg.AddProfile(new ModuleMappingProfile());
                    cfg.AddProfile(new UserMappingProfile());
                });

                return config.CreateMapper();
            }
        }

        public UnitTestSetup()
        {
            Context = GetDatabase("TestDatabase");
            UserManager = GetUserManager();
            RoleManager = GetRoleManager();

            RoleSeeder.Seed(RoleManager).Wait();
            AppUserSeeder.Seed(UserManager).Wait();
            ModuleSeeder.Seed(Context.Modules).Wait();
            ModuleTutorSeeder.Seed(Context.ModuleTutors, UserManager).Wait();
            TutoringApplicationSeeder.Seed(UserManager, Context.TutoringApplications).Wait();
            StudentTutorSeeder.Seed(UserManager, Context.StudentTutors).Wait();
            StudentTutorIgnoreSeeder.Seed(UserManager, Context.StudentTutorIgnores).Wait();
            ChatSeeder.Seed(Context.ChatMessages, UserManager).Wait();
            TutoringSessionsSeeder.Seed(Context.TutoringSessions, UserManager).Wait();
            AssignmentSeed.Seed(Context.Assignments, UserManager).Wait();

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
                Options.Create(new IdentityOptions { User = new UserOptions { RequireUniqueEmail = true } }),
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

        private RoleManager<IdentityRole> GetRoleManager()
        {
            var roleStore = new RoleStore<IdentityRole>(Context);
            var roleManager = new RoleManager<IdentityRole>(
                roleStore,
                new IRoleValidator<IdentityRole>[0],
                null,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<ILogger<RoleManager<IdentityRole>>>().Object
            );

            return roleManager;
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
