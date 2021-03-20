using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using TutoringApp.Data.Dtos.Chats;
using TutoringApp.Infrastructure.SignalR.Hubs;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Services.SignalR
{
    public class HubsService : IHubsService
    {
        private readonly IHubContext<MainHub> _hubContext;

        public HubsService(IHubContext<MainHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendChatNotificationToUser(string userId, ChatMessageDto chatMessage)
        {
            await _hubContext.Clients
                .User(userId)
                .SendAsync("chat-message-received", chatMessage);
        }
    }
}
