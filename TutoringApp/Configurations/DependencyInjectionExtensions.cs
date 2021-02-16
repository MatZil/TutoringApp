using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using TutoringApp.Infrastructure.EmailSender;
using TutoringApp.Services.Auth;
using TutoringApp.Services.Interfaces;
using TutoringApp.Services.Shared;

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
                .AddScoped<IEmailService, EmailService>()
                .AddScoped<IEncodingService, EncodingService>()
                .AddScoped<IUrlService, UrlService>()

                // Infrastructure
                .AddSingleton<IEmailSender, EmailSender>()
                ;
        }
    }
}
