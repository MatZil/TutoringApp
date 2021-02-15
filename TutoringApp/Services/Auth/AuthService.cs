using AutoMapper;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
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

        public AuthService(
            UserManager<AppUser> userManager,
            ILogger<IAuthService> logger,
            IMapper mapper)
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
        }

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
        }

        public async Task<LoginResponseDto> Login(UserLoginDto userLogin)
        {
            var user = await _userManager.FindByEmailAsync(userLogin.Email);

        }

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
    }
}
