using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Infrastructure.SignalR.Services
{
    public class UserIdProvider : IUserIdProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public UserIdProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public string GetUserId(HubConnectionContext a)
        {
            using var scope = _serviceProvider.CreateScope();
            var currentUserService = scope.ServiceProvider.GetRequiredService<ICurrentUserService>();

            return currentUserService.GetUserId();
        }
    }
}
