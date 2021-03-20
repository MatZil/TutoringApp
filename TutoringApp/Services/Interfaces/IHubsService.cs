using System.Threading.Tasks;
using TutoringApp.Data.Dtos.Chats;

namespace TutoringApp.Services.Interfaces
{
    public interface IHubsService
    {
        Task SendChatNotificationToUser(string userId, ChatMessageDto chatMessage);
    }
}
