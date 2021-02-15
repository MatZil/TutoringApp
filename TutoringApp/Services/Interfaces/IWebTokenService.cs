﻿using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TutoringApp.Services.Interfaces
{
    public interface IWebTokenService
    {
        SigningCredentials GetSigningCredentials();
        IEnumerable<Claim> GetClaims(IdentityUser user);
        JwtSecurityToken GetSecurityToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims);
    }
}
