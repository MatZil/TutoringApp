using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using TutoringApp.Data.Models;
using TutoringApp.Data.Models.JoiningTables;
using TutoringApp.Infrastructure.EmailSender;
using TutoringApp.Infrastructure.Repositories;
using TutoringApp.Infrastructure.Repositories.Interfaces;
using TutoringApp.Infrastructure.Repositories.ModelRepositories;
using TutoringApp.Infrastructure.SignalR.Services;
using TutoringApp.Services.Auth;
using TutoringApp.Services.Chats;
using TutoringApp.Services.Interfaces;
using TutoringApp.Services.Modules;
using TutoringApp.Services.Shared;
using TutoringApp.Services.SignalR;
using TutoringApp.Services.Tutoring;
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
                .AddScoped<ICurrentUserService, CurrentUserService>()
                .AddScoped<ITimeService, TimeService>()
                .AddScoped<ITutoringApplicationsService, TutoringApplicationsService>()
                .AddScoped<IStudentTutorsService, StudentTutorsService>()
                .AddScoped<IChatsService, ChatsService>()
                .AddScoped<IHubsService, HubsService>()
                .AddScoped<ITutoringSessionsService, TutoringSessionsService>()
                .AddScoped<IAssignmentsService, AssignmentsService>()

                // Infrastructure
                .AddSingleton<IEmailSender, EmailSender>()
                .AddSingleton<IUserIdProvider, UserIdProvider>()
                .AddScoped<IRepository<Assignment>, AssignmentsRepository>()
                .AddScoped<IRepository<ChatMessage>, ChatMessagesRepository>()
                .AddScoped<IRepository<Module>, ModulesRepository>()
                .AddScoped<IRepository<TutoringApplication>, TutoringApplicationsRepository>()
                .AddScoped<IRepository<TutoringSession>, TutoringSessionsRepository>()
                .AddScoped<IRepository<StudentTutor>, StudentTutorsRepository>()
                .AddScoped<IRepository<StudentTutorIgnore>, StudentTutorIgnoresRepository>()
                .AddScoped<IModuleTutorsRepository, ModuleTutorsRepository>()
                ;
        }
    }
}
