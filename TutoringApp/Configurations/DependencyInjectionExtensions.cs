using Microsoft.Extensions.DependencyInjection;
using TutoringApp.Services.Auth;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Configurations
{
    public static class DependencyInjectionExtensions
    {
        public static void ConfigureDependencyInjections(this IServiceCollection services)
        {
            services

                // Services
                .AddScoped<IAuthService, AuthService>()
                .AddScoped<IWebTokenService, WebTokenService>()

                ;
        }
    }
}
