using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using TutoringApp.Configurations.Auth;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Services.Auth
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetRole()
        {
            var roleClaim = GetUserClaim(AppClaimTypes.RoleClaimType);

            return roleClaim.Value;
        }

        private Claim GetUserClaim(string claimType)
        {
            return _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == claimType);
        }
    }
}
