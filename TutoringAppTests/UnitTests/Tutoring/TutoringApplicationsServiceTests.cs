using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using TutoringApp.Data;
using TutoringApp.Data.Dtos.Tutoring;
using TutoringApp.Data.Models;
using TutoringApp.Infrastructure.Repositories.ModelRepositories;
using TutoringApp.Services.Interfaces;
using TutoringApp.Services.Shared;
using TutoringApp.Services.Tutoring;
using TutoringAppTests.Setup;
using Xunit;

namespace TutoringAppTests.UnitTests.Tutoring
{
    public class TutoringApplicationsServiceTests
    {
        private readonly ITutoringApplicationsService _tutoringApplicationsService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;

        public TutoringApplicationsServiceTests()
        {
            var setup = new UnitTestSetup();
            _context = setup.Context;
            _userManager = setup.UserManager;

            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _currentUserServiceMock
                .Setup(s => s.GetUserId())
                .Returns(setup.UserManager.Users.First(u => u.Email == "matas.zilinskas@ktu.edu").Id);

            _tutoringApplicationsService = new TutoringApplicationsService(
                setup.UserManager,
                _currentUserServiceMock.Object,
                new Mock<ILogger<ITutoringApplicationsService>>().Object,
                new TimeService(),
                new TutoringApplicationsRepository(setup.Context)
            );
        }

        [Theory]
        [InlineData(2, "This is a motivational letter.")]
        public async Task When_ApplyingForTutoring_Expect_ApplicationCreated(int moduleId, string motivationalLetter)
        {
            var application = new TutoringApplicationNewDto { MotivationalLetter = motivationalLetter };
            await _tutoringApplicationsService.ApplyForTutoring(moduleId, application);

            var applicationCreated = await _context.TutoringApplications.FirstAsync(ta => ta.MotivationalLetter == motivationalLetter);
            Assert.Equal(moduleId, applicationCreated.ModuleId);
            Assert.Equal(DateTimeOffset.Now.Date, applicationCreated.RequestDate.Date);
        }

        [Theory]
        [InlineData(1, "This is a motivational letter.")]
        public async Task When_ApplyingForTutoringAsTutor_Expect_Exception(int moduleId, string motivationalLetter)
        {
            _currentUserServiceMock
                .Setup(s => s.GetUserId())
                .Returns(_userManager.Users.First(u => u.Email == "matas.tutorius2@ktu.edu").Id);

            var application = new TutoringApplicationNewDto { MotivationalLetter = motivationalLetter };

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _tutoringApplicationsService.ApplyForTutoring(moduleId, application)
            );
        }

        [Theory]
        [InlineData(1, "This is a motivational letter.")]
        public async Task When_ApplyingForTutoringAsAdmin_Expect_Exception(int moduleId, string motivationalLetter)
        {
            _currentUserServiceMock
                .Setup(s => s.GetUserId())
                .Returns(_userManager.Users.First(u => u.Email == "matas.admin@ktu.edu").Id);

            var application = new TutoringApplicationNewDto { MotivationalLetter = motivationalLetter };

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _tutoringApplicationsService.ApplyForTutoring(moduleId, application)
            );
        }

        [Theory]
        [InlineData(1)]
        public async Task When_ResigningFromTutoring_Expect_ModuleTutorRemoved(int moduleId)
        {
            var tutor = await _userManager.FindByEmailAsync("matas.tutorius2@ktu.edu");
            _currentUserServiceMock
                .Setup(s => s.GetUserId())
                .Returns(tutor.Id);

            await _tutoringApplicationsService.ResignFromTutoring(moduleId);

            var moduleTutorRemoved = await _context.ModuleTutors.FirstOrDefaultAsync(mt => mt.ModuleId == moduleId && mt.TutorId == tutor.Id);
            Assert.Null(moduleTutorRemoved);
        }

        [Fact]
        public async Task When_GettingTutoringApplications_Expect_CorrectApplications()
        {
            var applications = await _tutoringApplicationsService.GetTutoringApplications();

            Assert.Collection(applications,
                application =>
                {
                    Assert.Equal(1, application.Id);
                    Assert.Equal("First motivational letter", application.MotivationalLetter);
                    Assert.Equal("Operating Systems", application.ModuleName);
                    Assert.Equal("matas.zilinskas@ktu.edu", application.Email);
                },
                application =>
                {
                    Assert.Equal(2, application.Id);
                    Assert.Equal("Second motivational letter", application.MotivationalLetter);
                    Assert.Equal("Databases", application.ModuleName);
                    Assert.Equal("matas.zilinskas@ktu.edu", application.Email);
                }
                );
        }
    }
}
