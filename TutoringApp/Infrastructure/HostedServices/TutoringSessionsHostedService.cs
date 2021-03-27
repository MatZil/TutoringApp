using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Infrastructure.HostedServices
{
    public class TutoringSessionsHostedService : IHostedService
    {
        private readonly ILogger<TutoringSessionsHostedService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;

        public TutoringSessionsHostedService(
            ILogger<TutoringSessionsHostedService> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Tutoring Sessions Hosted Service has started.");

            _timer = new Timer(
                ExecuteSessionsRecheck,
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(30));

            await Task.Yield();
        }

        private async void ExecuteSessionsRecheck(object state)
        {
            _logger.LogInformation($"Tutoring Sessions Hosted Service is executing...");

            using var scope = _serviceProvider.CreateScope();
            var tutoringSessionsService = scope.ServiceProvider.GetRequiredService<ITutoringSessionsService>();
            await tutoringSessionsService.RecheckUpcomingTutoringSessions();

            _logger.LogInformation($"Tutoring Sessions Hosted Service finished executing.");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Tutoring Sessions Hosted Service has stopped.");

            _timer.Change(Timeout.Infinite, 0);
            await _timer.DisposeAsync();
        }
    }
}
