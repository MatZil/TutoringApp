using System.Threading.Tasks;
using TutoringApp.Data.Dtos.Chats;
using TutoringApp.Data.Dtos.Tutoring.TutoringSessions;

namespace TutoringApp.Services.Interfaces
{
    public interface IHubsService
    {
        Task SendChatNotificationToUser(string userId, ChatMessageDto chatMessage);
        Task SendSessionFinishedNotificationToUser(string userId, TutoringSessionFinishedNotificationDto sessionNotification);
    }
}
