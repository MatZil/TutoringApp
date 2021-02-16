﻿using AutoMapper;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using TutoringApp.Data.Dtos.Auth;
using TutoringApp.Data.Models;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<IAuthService> _logger;
        private readonly IMapper _mapper;
        private readonly IWebTokenService _webTokenService;
        private readonly IUrlService _urlService;
        private readonly IEncodingService _encodingService;
        private readonly IEmailService _emailService;

        public AuthService(
            UserManager<AppUser> userManager,
            ILogger<IAuthService> logger,
            IMapper mapper,
            IWebTokenService webTokenService, 
            IUrlService urlService,
            IEncodingService encodingService,
            IEmailService emailService)
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
            _webTokenService = webTokenService;
            _urlService = urlService;
            _encodingService = encodingService;
            _emailService = emailService;
        }

        #region Interface implementation
        public async Task Register(UserRegistrationDto userRegistration)
        {
            ValidateUserRegistration(userRegistration);

            var user = _mapper.Map<AppUser>(userRegistration);
            var identityResult = await _userManager.CreateAsync(user, userRegistration.Password);

            if (!identityResult.Succeeded)
            {
                var errorMessage = $"Could not register user: {identityResult.Errors.First().Description}";
                _logger.LogError(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }

            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedEmailConfirmationToken = _encodingService.GetWebEncodedString(emailConfirmationToken);
            var emailConfirmationLink = _urlService.GetEmailConfirmationLink(user.Email, encodedEmailConfirmationToken);
            await _emailService.SendConfirmationEmail(user.Email, emailConfirmationLink);
        }

        public async Task<LoginResponseDto> Login(UserLoginDto userLogin)
        {
            var user = await _userManager.FindByEmailAsync(userLogin.Email);
            await ValidateUserLogin(user, userLogin);

            var signingCredentials = _webTokenService.GetSigningCredentials();
            var userClaims = _webTokenService.GetClaims(user);
            var securityToken = _webTokenService.GetSecurityToken(signingCredentials, userClaims);

            return new LoginResponseDto
            {
                WebToken = new JwtSecurityTokenHandler().WriteToken(securityToken)
            };
        }
        #endregion

        #region Private methods
        private void ValidateUserRegistration(UserRegistrationDto userRegistration)
        {
            if (userRegistration.FirstName.IsNullOrEmpty() || userRegistration.LastName.IsNullOrEmpty())
            {
                const string errorMessage = "Could not register user: first name and last name are mandatory.";
                _logger.LogError(errorMessage);
                throw new ArgumentException(errorMessage);
            }

            if (userRegistration.Password?.Length < 6)
            {
                const string errorMessage = "Could not register user: password must consist of at least 6 symbols.";
                _logger.LogError(errorMessage);
                throw new ArgumentException(errorMessage);
            }
        }

        private async Task ValidateUserLogin(AppUser user, UserLoginDto userLogin)
        {
            if (user is null)
            {
                var errorMessage = $"Could not login: email '{userLogin.Email}' was not found.";
                _logger.LogError(errorMessage);
                throw new ArgumentException(errorMessage);
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, userLogin.Password);
            if (!isPasswordValid)
            {
                var errorMessage = $"Could not login: password was not correct for user '{userLogin.Email}'";
                _logger.LogError(errorMessage);
                throw new ArgumentException(errorMessage);
            }
        }
        #endregion
    }
}
