using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using TutoringApp.Data;
using TutoringApp.Data.Dtos.Users;
using TutoringApp.Data.Models;
using TutoringApp.Services.Interfaces;
using TutoringApp.Services.Shared;
using TutoringApp.Services.Users;
using TutoringAppTests.Setup;
using Xunit;

namespace TutoringAppTests.UnitTests.Users
{
    public class TutoringServiceTests
    {
        private readonly ITutoringService _tutoringService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;

        public TutoringServiceTests()
        {
            var setup = new UnitTestSetup();
            _context = setup.Context;
            _userManager = setup.UserManager;

            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _currentUserServiceMock
                .Setup(s => s.GetUserId())
                .Returns(setup.UserManager.Users.First(u => u.Email == "matas.zilinskas@ktu.edu").Id);

            _tutoringService = new TutoringService(
                setup.UserManager,
                _currentUserServiceMock.Object,
                new Mock<ILogger<ITutoringService>>().Object,
                new TimeService()
            );
        }

        [Theory]
        [InlineData(2, "This is a motivational letter.")]
        public async Task When_ApplyingForTutoring_Expect_ApplicationCreated(int moduleId, string motivationalLetter)
        {
            var application = new TutoringApplicationNewDto { MotivationalLetter = motivationalLetter };
            await _tutoringService.ApplyForTutoring(moduleId, application);

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
                await _tutoringService.ApplyForTutoring(moduleId, application)
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
                await _tutoringService.ApplyForTutoring(moduleId, application)
            );
        }
    }
}
