using AutoMapper;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Identity;
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
        private readonly IMapper _mapper;
        private readonly IWebTokenService _webTokenService;
        private readonly IUrlService _urlService;
        private readonly IEncodingService _encodingService;
        private readonly IEmailService _emailService;

        public AuthService(
            UserManager<AppUser> userManager,
            IMapper mapper,
            IWebTokenService webTokenService, 
            IUrlService urlService,
            IEncodingService encodingService,
            IEmailService emailService)
        {
            _userManager = userManager;
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
                throw new InvalidOperationException($"Could not register user: {identityResult.Errors.First().Description}");
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
                    throw new InvalidOperationException($"Could not confirm {email} because the token was incorrect.");
                }
            }
            else
            {
                throw new InvalidOperationException($"Could not confirm {email} because it is not registered.");
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
                throw new ArgumentException("Could not register user: email is not in ktu.edu domain.");
            }

            if (
                userRegistration.Faculty.IsNullOrEmpty()
                || userRegistration.StudyBranch.IsNullOrEmpty()
                || userRegistration.StudentCycle == StudentCycleEnum.Undefined
                || userRegistration.StudentYear == StudentYearEnum.Undefined
                )
            {
                throw new ArgumentException("Could not register user: incomplete information.");
            }
        }

        private async Task ValidateUserLogin(AppUser user, UserLoginDto userLogin)
        {
            if (user is null)
            {
                throw new ArgumentException($"Could not login: email '{userLogin.Email}' was not found.");
            }

            if (!user.EmailConfirmed)
            {
                throw new ArgumentException($"Could not login: user '{userLogin.Email}' was not confirmed via email.");
            }

            if (!user.IsConfirmed)
            {
                throw new ArgumentException($"Could not login: user '{userLogin.Email}' was not confirmed by our Administrator just yet.");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, userLogin.Password);
            if (!isPasswordValid)
            {
                throw new ArgumentException($"Could not login: password was not correct for user '{userLogin.Email}'");
            }
        }
        #endregion
    }
}
