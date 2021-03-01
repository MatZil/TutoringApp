using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using TutoringApp.Configurations.Auth;
using TutoringApp.Data.Models;
using TutoringApp.Services.Interfaces;
using TutoringApp.Services.Users;
using TutoringAppTests.Setup;
using Xunit;

namespace TutoringAppTests.UnitTests.Users
{
    public class UsersServiceTests
    {
        private readonly IUsersService _usersService;
        private readonly UserManager<AppUser> _userManager;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;

        public UsersServiceTests()
        {
            var setup = new UnitTestSetup();
            _userManager = setup.UserManager;
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _currentUserServiceMock
                .Setup(s => s.GetRole())
                .Returns(AppRoles.Student);

            _usersService = new UsersService(
                setup.UserManager,
                _currentUserServiceMock.Object,
                UnitTestSetup.Mapper,
                new Mock<ILogger<IUsersService>>().Object
            );
        }

        [Theory]
        [InlineData("matas.zilinskas@ktu.edu", AppRoles.Student)]
        [InlineData("matas.tutorius@ktu.edu", AppRoles.Tutor)]
        [InlineData("matas.admin@ktu.edu", AppRoles.Admin)]
        [InlineData("matas.lecturer@ktu.edu", AppRoles.Lecturer)]
        public async Task When_GettingUserRole_Expect_CorrectRole(string email, string expectedRole)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var actualRole = await _usersService.GetRole(user.Id);

            Assert.Equal(expectedRole, actualRole);
        }

        [Theory]
        [InlineData(1)]
        public async Task When_GettingModuleTutors_Expect_CorrectTutors(int moduleId)
        {
            var tutors = await _usersService.GetTutors(moduleId);

            Assert.Collection(tutors,
                t => Assert.Equal("Matas FirstTutor", t.Name),
                t => Assert.Equal("Matas SecondTutor", t.Name)
            );
        }

        [Fact]
        public async Task When_GettingUnconfirmedUsers_Expect_CorrectUsers()
        {
            var actualUsers = await _usersService.GetUnconfirmedUsers();

            Assert.Collection(actualUsers,
                u => Assert.Equal("matas.unconfirmed@ktu.edu", u.Email)
            );
        }

        [Theory]
        [InlineData("matas.unconfirmed@ktu.edu")]
        public async Task When_ConfirmingUser_Expect_UserConfirmed(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var actualEmail = await _usersService.ConfirmUser(user.Id);

            Assert.Equal(email, actualEmail);
            Assert.True(user.IsConfirmed);
        }

        [Theory]
        [InlineData("non-existing@email.com")]
        [InlineData("matas.emailunconfirmed@ktu.edu")]
        [InlineData("matas.zilinskas@ktu.edu")]
        public async Task When_ConfirmingInvalidUser_Expect_Exception(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _usersService.ConfirmUser(user?.Id)
            );
        }

        [Theory]
        [InlineData("matas.unconfirmed@ktu.edu")]
        public async Task When_RejectingUser_Expect_UserRejected(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            // ReSharper disable once UseDeconstruction
            var actualEmail = await _usersService.RejectUser(user.Id);

            Assert.Equal(email, actualEmail);

            var userDeleted = await _userManager.FindByEmailAsync(email);
            Assert.Null(userDeleted);
        }

        [Theory]
        [InlineData("non-existing@email.com")]
        [InlineData("matas.emailunconfirmed@ktu.edu")]
        [InlineData("matas.zilinskas@ktu.edu")]
        public async Task When_RejectingInvalidUser_Expect_Exception(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _usersService.RejectUser(user?.Id)
            );
        }
    }
}
