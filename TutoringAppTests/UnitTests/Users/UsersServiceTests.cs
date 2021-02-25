using Microsoft.AspNetCore.Identity;
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

        public UsersServiceTests()
        {
            var setup = new UnitTestSetup();
            _userManager = setup.UserManager;

            _usersService = new UsersService(
                setup.UserManager
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
    }
}
