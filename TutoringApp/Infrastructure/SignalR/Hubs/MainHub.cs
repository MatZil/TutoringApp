using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace TutoringApp.Infrastructure.SignalR.Hubs
{
    public class MainHub : Hub
    {
        private readonly ILogger<MainHub> _logger;

        public MainHub(ILogger<MainHub> logger)
        {
            _logger = logger;
        }

        public override Task OnConnectedAsync()
        {
            _logger.LogInformation("User Connected to Main Hub!");

            return base.OnConnectedAsync();
        }
    }
}
