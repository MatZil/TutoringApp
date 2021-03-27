using Microsoft.AspNetCore.Identity;
using Moq;
using System.Linq;
using TutoringApp.Data.Models;
using TutoringApp.Infrastructure.Repositories.ModelRepositories;
using TutoringApp.Services.Interfaces;
using TutoringApp.Services.Shared;
using TutoringApp.Services.Tutoring;
using TutoringAppTests.Setup;

namespace TutoringAppTests.UnitTests.Tutoring
{
    public class TutoringSessionsServiceTests
    {
        private readonly ITutoringSessionsService _tutoringSessionsService;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly UserManager<AppUser> _userManager;

        public TutoringSessionsServiceTests()
        {
            var setup = new UnitTestSetup();

            _userManager = setup.UserManager;

            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _currentUserServiceMock
                .Setup(s => s.GetUserId())
                .Returns(setup.UserManager.Users.First(u => u.Email == "matas.tutorius1@ktu.edu").Id);

            _tutoringSessionsService = new TutoringSessionsService(
                new TutoringSessionsRepository(setup.Context),
                setup.UserManager,
                _currentUserServiceMock.Object,
                new TimeService()
            );
        }
    }
}
