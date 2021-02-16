using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Services.Auth
{
    public class WebTokenService : IWebTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _webTokenSettings;

        public WebTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _webTokenSettings = configuration.GetSection("WebTokenSettings");
        }

        public SigningCredentials GetSigningCredentials()
        {
            var securityKey = _webTokenSettings.GetSection("securityKey").Value;
            var securityKeyBytes = Encoding.UTF8.GetBytes(securityKey);
            var symmetricSecurityKey = new SymmetricSecurityKey(securityKeyBytes);

            return new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        }

        public IEnumerable<Claim> GetClaims(IdentityUser user)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email)
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
