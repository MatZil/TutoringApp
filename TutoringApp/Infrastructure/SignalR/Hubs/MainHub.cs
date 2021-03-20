using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace TutoringApp.Infrastructure.SignalR.Hubs
{
    public class MainHub : Hub
    {
        private readonly ILogger<MainHub> _logger;

        public MainHub(ILogger<MainHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation("User Connected to Main Hub!");
        }
    }
}
