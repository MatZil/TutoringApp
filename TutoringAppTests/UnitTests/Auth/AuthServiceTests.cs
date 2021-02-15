﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using TutoringApp.Data.Dtos.Auth;
using TutoringApp.Data.Models;
using TutoringApp.Services.Auth;
using TutoringApp.Services.Interfaces;
using TutoringAppTests.Configuration;
using Xunit;

namespace TutoringAppTests.UnitTests.Auth
{
    public class AuthServiceTests
    {
        private readonly IAuthService _authService;
        private readonly UserManager<AppUser> _userManager;

        public AuthServiceTests()
        {
            var webTokenServiceMock = new Mock<IWebTokenService>();
            webTokenServiceMock
                .Setup(s => s.GetSecurityToken(It.IsAny<SigningCredentials>(), It.IsAny<IEnumerable<Claim>>()))
                .Returns(new JwtSecurityToken());

            var setup = new UnitTestSetup();
            _userManager = setup.UserManager;

            _authService = new AuthService(
                setup.UserManager,
                new Mock<ILogger<IAuthService>>().Object,
                UnitTestSetup.Mapper,
                webTokenServiceMock.Object
                );
        }

        [Theory]
        [InlineData("matas.zilinskas@ktu.edu")]
        public async Task When_Registering_Expect_UserRegistered(string email)
        {
            var userRegistration = new UserRegistrationDto
            {
                FirstName = "Matas",
                LastName = "Zilinskas",
                Email = email,
                Password = "Password1"
            };

            await _authService.Register(userRegistration);

            var registeredUser = await _userManager.FindByEmailAsync(email);

            Assert.Equal("Matas", registeredUser.FirstName);
            Assert.Equal("Zilinskas", registeredUser.LastName);

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(registeredUser, "Password1");
            Assert.True(isPasswordCorrect);
        }

        [Fact]
        public async Task When_RegisteringWithoutFirstName_Expect_Exception()
        {
            var userRegistration = new UserRegistrationDto
            {
                FirstName = "",
                LastName = "Zilinskas",
                Email = "matas.zilinskas@ktu.edu",
                Password = "Password1"
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _authService.Register(userRegistration)
            );
        }

        [Fact]
        public async Task When_RegisteringWithoutLastName_Expect_Exception()
        {
            var userRegistration = new UserRegistrationDto
            {
                FirstName = "Matas",
                LastName = "",
                Email = "matas.zilinskas@ktu.edu",
                Password = "Password1"
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _authService.Register(userRegistration)
            );
        }

        [Fact]
        public async Task When_LoggingIn_Expect_NoException()
        {
            var userLogin = new UserLoginDto
            {
                Email = "matzil@ktu.lt",
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
                Email = "matzil@ktu.lt",
                Password = "Invalid"
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _authService.Login(userLogin)
            );
        }
    }
}
