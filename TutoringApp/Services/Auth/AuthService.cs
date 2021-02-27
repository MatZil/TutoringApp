using AutoMapper;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using TutoringApp.Configurations.Auth;
using TutoringApp.Data.Dtos.Auth;
using TutoringApp.Data.Extensions;
using TutoringApp.Data.Models;
using TutoringApp.Data.Models.Enums;
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
        public async Task<string> Register(UserRegistrationDto userRegistration)
        {
            ValidateUserRegistration(userRegistration);
            await DeleteUnconfirmedUser(userRegistration.Email);

            var user = _mapper.Map<AppUser>(userRegistration);
            var identityResult = await _userManager.CreateAsync(user, userRegistration.Password);

            if (!identityResult.Succeeded)
            {
                var errorMessage = $"Could not register user: {identityResult.Errors.First().Description}";
                _logger.LogError(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }

            await _userManager.AddToRoleAsync(user, AppRoles.Student);

            return user.Id;
        }

        public async Task SendConfirmationEmail(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedEmailConfirmationToken = _encodingService.GetWebEncodedString(emailConfirmationToken);
            var emailConfirmationLink = _urlService.GetEmailConfirmationLink(user.Email, encodedEmailConfirmationToken);
            await _emailService.SendConfirmationEmail(user.Email, emailConfirmationLink);
        }

        private async Task DeleteUnconfirmedUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null && !user.EmailConfirmed)
            {
                await _userManager.DeleteAsync(user);
            }
        }

        public async Task ConfirmEmail(string email, string encodedToken)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var decodedToken = _encodingService.GetWebDecodedString(encodedToken);
                var identityResult = await _userManager.ConfirmEmailAsync(user, decodedToken);
                if (!identityResult.Succeeded)
                {
                    _logger.LogInformation($"Could not confirm {email}: {identityResult.Errors.First()}");
                    throw new InvalidOperationException($"Could not confirm {email} because the token was incorrect.");
                }
            }
            else
            {
                var errorMessage = $"Could not confirm {email} because it is not registered.";
                _logger.LogInformation(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }
        }

        public async Task<LoginResponseDto> Login(UserLoginDto userLogin)
        {
            var user = await _userManager.FindByEmailAsync(userLogin.Email);
            await ValidateUserLogin(user, userLogin);

            var signingCredentials = _webTokenService.GetSigningCredentials();
            var userClaims = await _webTokenService.GetClaims(user);
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
            if (!userRegistration.Email.IsKtuEmail())
            {
                const string errorMessage = "Could not register user: email is not in ktu.edu domain.";
                _logger.LogError(errorMessage);
                throw new ArgumentException(errorMessage);
            }

            if (
                userRegistration.Faculty.IsNullOrEmpty()
                || userRegistration.StudyBranch.IsNullOrEmpty()
                || userRegistration.StudentCycle == StudentCycleEnum.Undefined
                || userRegistration.StudentYear == StudentYearEnum.Undefined
                )
            {
                const string errorMessage = "Could not register user: incomplete information.";
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

            if (!user.EmailConfirmed)
            {
                var errorMessage = $"Could not login: user '{userLogin.Email}' was not confirmed via email.";
                _logger.LogError(errorMessage);
                throw new ArgumentException(errorMessage);
            }

            if (!user.IsConfirmed)
            {
                var errorMessage = $"Could not login: user '{userLogin.Email}' was not confirmed by our Administrator just yet.";
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
