using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using TutoringApp.Data;
using TutoringApp.Data.Dtos.Modules;
using TutoringApp.Data.Models;
using TutoringApp.Infrastructure.Repositories.ModelRepositories;
using TutoringApp.Services.Interfaces;
using TutoringApp.Services.Modules;
using TutoringAppTests.Setup;
using Xunit;

namespace TutoringAppTests.UnitTests.Modules
{
    public class ModulesServiceTests
    {
        private readonly IModulesService _modulesService;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ModulesServiceTests()
        {
            var setup = new UnitTestSetup();
            _context = setup.Context;
            _userManager = setup.UserManager;

            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _currentUserServiceMock
                .Setup(s => s.GetUserId())
                .Returns(_userManager.Users.First(u => u.Email == "matas.tutorius1@ktu.edu").Id);

            _modulesService = new ModulesService(
                new ModulesRepository(setup.Context),
                UnitTestSetup.Mapper,
                _currentUserServiceMock.Object,
                new Mock<ILogger<IModulesService>>().Object
                );
        }

        [Fact]
        public async Task When_GettingAllModules_Expect_CorrectModules()
        {
            var actualModules = await _modulesService.GetAll();

            Assert.Collection(actualModules,
                m => Assert.Equal("Operating Systems", m.Name),
                m => Assert.Equal("Databases", m.Name),
                m => Assert.Equal("Analysis of Algorithms", m.Name),
                m => Assert.Equal("Cyber Security", m.Name)
                );
        }

        [Theory]
        [InlineData("Software Testing")]
        public async Task When_CreatingModule_Expect_ModuleCreated(string name)
        {
            var moduleNew = new ModuleNewDto { Name = name };
            var id = await _modulesService.Create(moduleNew);

            var actualModuleCreated = await _context.Modules.FirstAsync(m => m.Id == id);
            Assert.Equal(name, actualModuleCreated.Name);
        }

        [Theory]
        [InlineData(4)]
        public async Task When_DeletingModuleWithoutTutors_Expect_ModuleDeleted(int id)
        {
            await _modulesService.Delete(id);

            var actualModuleDeleted = await _context.Modules.FirstOrDefaultAsync(m => m.Id == id);
            Assert.Null(actualModuleDeleted);
        }

        [Theory]
        [InlineData(1)]
        public async Task When_DeletingModuleWithTutors_Expect_Exception(int id)
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _modulesService.Delete(id)
            );
        }

        [Theory]
        [InlineData(1)]
        public async Task When_GettingUserModuleMetadataAsTutor_Expect_CorrectMetadata(int moduleId)
        {
            var metadata = await _modulesService.GetUserModuleMetadata(moduleId);

            Assert.True(metadata.CanResignFromTutoring);
            Assert.False(metadata.CanApplyForTutoring);
        }

        [Theory]
        [InlineData(1)]
        public async Task When_GettingUserModuleMetadataAsApplicant_Expect_CorrectMetadata(int moduleId)
        {
            _currentUserServiceMock
                .Setup(s => s.GetUserId())
                .Returns(_userManager.Users.First(u => u.Email == "matas.zilinskas@ktu.edu").Id);
            var metadata = await _modulesService.GetUserModuleMetadata(moduleId);

            Assert.False(metadata.CanResignFromTutoring);
            Assert.False(metadata.CanApplyForTutoring);
        }

        [Theory]
        [InlineData(3)]
        public async Task When_GettingUserModuleMetadataAsStudent_Expect_CorrectMetadata(int moduleId)
        {
            _currentUserServiceMock
                .Setup(s => s.GetUserId())
                .Returns(_userManager.Users.First(u => u.Email == "matas.zilinskas@ktu.edu").Id);
            var metadata = await _modulesService.GetUserModuleMetadata(moduleId);

            Assert.False(metadata.CanResignFromTutoring);
            Assert.True(metadata.CanApplyForTutoring);
        }
    }
}
