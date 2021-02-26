using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TutoringApp.Configurations.Auth;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Services.Auth
{
    public class WebTokenService : IWebTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _webTokenSettings;
        private readonly IUsersService _usersService;

        public WebTokenService(
            IConfiguration configuration,
            IUsersService usersService)
        {
            _configuration = configuration;
            _usersService = usersService;
            _webTokenSettings = configuration.GetSection("WebTokenSettings");
        }

        public SigningCredentials GetSigningCredentials()
        {
            var securityKey = _webTokenSettings.GetSection("securityKey").Value;
            var securityKeyBytes = Encoding.UTF8.GetBytes(securityKey);
            var symmetricSecurityKey = new SymmetricSecurityKey(securityKeyBytes);

            return new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        }

        public async Task<IEnumerable<Claim>> GetClaims(IdentityUser user)
        {
            var role = await _usersService.GetRole(user.Id);

            return new List<Claim>
            {
                new Claim(AppClaimTypes.EmailClaimType, user.Email),
                new Claim(AppClaimTypes.RoleClaimType, role),
                new Claim("roles", role) // Used by Identity to authorize endpoints.
            };
        }

        public JwtSecurityToken GetSecurityToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
        {
            var lifetimeString = _webTokenSettings.GetSection("LifetimeInHours").Value;
            var lifetime = Convert.ToDouble(lifetimeString);

            return new JwtSecurityToken(
                issuer: _webTokenSettings.GetSection("ValidIssuer").Value,
                audience: _configuration["AppSettings:RootUrl"],
                claims: claims,
                expires: DateTime.Now.AddHours(lifetime),
                signingCredentials: signingCredentials
                );
        }
    }
}
