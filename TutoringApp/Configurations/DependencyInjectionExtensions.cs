using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using TutoringApp.Data.Models;
using TutoringApp.Infrastructure.EmailSender;
using TutoringApp.Infrastructure.Repositories;
using TutoringApp.Infrastructure.Repositories.ModelRepositories;
using TutoringApp.Services.Auth;
using TutoringApp.Services.Interfaces;
using TutoringApp.Services.Modules;
using TutoringApp.Services.Shared;
using TutoringApp.Services.Users;

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
                .AddScoped<IModulesService, ModulesService>()
                .AddScoped<IUsersService, UsersService>()

                // Infrastructure
                .AddSingleton<IEmailSender, EmailSender>()
                .AddScoped<IRepository<Assignment>, AssignmentsRepository>()
                .AddScoped<IRepository<ChatMessage>, ChatMessagesRepository>()
                .AddScoped<IRepository<EmailTemplate>, EmailTemplatesRepository>()
                .AddScoped<IRepository<GlobalSetting>, GlobalSettingsRepository>()
                .AddScoped<IRepository<Module>, ModulesRepository>()
                .AddScoped<IRepository<TutorEvaluation>, TutorEvaluationsRepository>()
                .AddScoped<IRepository<TutoringRequest>, TutoringRequestsRepository>()
                .AddScoped<IRepository<TutoringSession>, TutoringSessionsRepository>()
                ;
        }
    }
}
