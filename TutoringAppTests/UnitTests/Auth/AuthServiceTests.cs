using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using TutoringApp.Data.Dtos.Auth;
using TutoringApp.Data.Models;
using TutoringApp.Data.Models.Enums;
using TutoringApp.Services.Auth;
using TutoringApp.Services.Interfaces;
using TutoringApp.Services.Shared;
using TutoringAppTests.Setup;
using Xunit;

namespace TutoringAppTests.UnitTests.Auth
{
    public class AuthServiceTests
    {
        private readonly IAuthService _authService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEncodingService _encodingService;

        private readonly Mock<IEmailService> _emailServiceMock;

        public AuthServiceTests()
        {
            var webTokenServiceMock = new Mock<IWebTokenService>();
            webTokenServiceMock
                .Setup(s => s.GetSecurityToken(It.IsAny<SigningCredentials>(), It.IsAny<IEnumerable<Claim>>()))
                .Returns(new JwtSecurityToken());

            _emailServiceMock = new Mock<IEmailService>();

            _encodingService = new EncodingService();

            var setup = new UnitTestSetup();
            _userManager = setup.UserManager;

            _authService = new AuthService(
                setup.UserManager,
                UnitTestSetup.Mapper,
                webTokenServiceMock.Object,
                new Mock<IUrlService>().Object,
                _encodingService,
                _emailServiceMock.Object
                );
        }

        [Theory]
        [InlineData("matas.pavardenis@ktu.edu")]
        [InlineData("matas.emailunconfirmed@ktu.edu")]
        public async Task When_Registering_Expect_UserRegistered(string email)
        {
            var userRegistration = new UserRegistrationDto
            {
                FirstName = "Matas",
                LastName = "Zilinskas",
                Email = email,
                Password = "Password1",
                Faculty = "Informatics",
                StudentCycle = StudentCycleEnum.Bachelor,
                StudentYear = StudentYearEnum.SecondYear,
                StudyBranch = "Software Systems"
            };

            await _authService.Register(userRegistration);

            var registeredUser = await _userManager.FindByEmailAsync(email);

            Assert.Equal("Matas", registeredUser.FirstName);
            Assert.Equal("Zilinskas", registeredUser.LastName);
            Assert.Equal("Informatics", registeredUser.Faculty);
            Assert.Equal(StudentCycleEnum.Bachelor, registeredUser.StudentCycle);
            Assert.Equal(StudentYearEnum.SecondYear, registeredUser.StudentYear);
            Assert.Equal("Software Systems", registeredUser.StudyBranch);

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(registeredUser, "Password1");
            Assert.True(isPasswordCorrect);
        }

        [Fact]
        public async Task When_RegisteringWithIncompleteDetails_Expect_Exception()
        {
            var userRegistration = new UserRegistrationDto
            {
                FirstName = "",
                LastName = "Zilinskas",
                Email = "matas.pavardenis@ktu.edu",
                Password = "Password1"
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _authService.Register(userRegistration)
            );
        }

        [Fact]
        public async Task When_RegisteringWithExistingEmail_Expect_Exception()
        {
            var userRegistration = new UserRegistrationDto
            {
                FirstName = "Matas",
                LastName = "Zilinskas",
                Email = "matas.zilinskas@ktu.edu",
                Password = "Password1",
                Faculty = "Informatics",
                StudentCycle = StudentCycleEnum.Bachelor,
                StudentYear = StudentYearEnum.SecondYear,
                StudyBranch = "Software Systems"
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _authService.Register(userRegistration)
            );
        }

        [Fact]
        public async Task When_RegisteringWithoutKtuEmail_Expect_Exception()
        {
            var userRegistration = new UserRegistrationDto
            {
                FirstName = "Matas",
                LastName = "Zilinskas",
                Email = "matas.pavardenis@ktu.lt",
                Password = "Password1",
                Faculty = "Informatics",
                StudentCycle = StudentCycleEnum.Bachelor,
                StudentYear = StudentYearEnum.SecondYear,
                StudyBranch = "Software Systems"
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _authService.Register(userRegistration)
            );
        }

        [Fact]
        public async Task When_ConfirmingEmailOfNonExistingUser_ExpectException()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () => 
                await _authService.ConfirmEmail("non existing", "a")
            );
        }

        [Fact]
        public async Task When_ConfirmingEmailWithInvalidToken_ExpectException()
        {
            var encodedMalformedToken = _encodingService.GetWebEncodedString("a");
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _authService.ConfirmEmail("matas.zilinskas@ktu.edu", encodedMalformedToken)
            );
        }

        [Fact]
        public async Task When_ConfirmingEmailWithValidToken_Expect_NoException()
        {
            var user = await _userManager.FindByEmailAsync("matas.zilinskas@ktu.edu");
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = _encodingService.GetWebEncodedString(token);

            await _authService.ConfirmEmail("matas.zilinskas@ktu.edu", encodedToken);
        }

        [Fact]
        public async Task When_LoggingIn_Expect_NoException()
        {
            var userLogin = new UserLoginDto
            {
                Email = "matas.zilinskas@ktu.edu",
                Password = "Password1"
            };

            await _authService.Login(userLogin);
        }

        [Fact]
        public async Task When_LoggingInWithInvalidEmail_Expect_Exception()
        {
            var userLogin = new UserLoginDto
            {
                Email = "Invalid",
                Password = "Password1"
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _authService.Login(userLogin)
            );
        }

        [Fact]
        public async Task When_LoggingInWithInvalidPassword_Expect_Exception()
        {
            var userLogin = new UserLoginDto
            {
                Email = "matas.zilinskas@ktu.edu",
                Password = "Invalid"
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _authService.Login(userLogin)
            );
        }

        [Fact]
        public async Task When_LoggingInAsEmailUnconfirmedUser_Expect_Exception()
        {
            var userLogin = new UserLoginDto
            {
                Email = "matas.emailunconfirmed@ktu.edu",
                Password = "Password1"
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _authService.Login(userLogin)
            );
        }

        [Fact]
        public async Task When_LoggingInAsUnconfirmedUser_Expect_Exception()
        {
            var userLogin = new UserLoginDto
            {
                Email = "matas.unconfirmed@ktu.edu",
                Password = "Password1"
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _authService.Login(userLogin)
            );
        }

        [Theory]
        [InlineData("matas.emailunconfirmed@ktu.edu")]
        public async Task When_SendingConfirmationEmail_Expect_ConfirmationEmailSent(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            await _authService.SendConfirmationEmail(user.Id);

            _emailServiceMock.Verify(
                s => s.SendConfirmationEmail(email, It.IsAny<string>()),
                Times.Once
                );
        }
    }
}
