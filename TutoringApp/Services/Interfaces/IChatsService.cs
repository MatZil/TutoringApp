using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringApp.Data.Dtos.Chats;

namespace TutoringApp.Services.Interfaces
{
    public interface IChatsService
    {
        Task PostChatMessage(string receiverId, ChatMessageNewDto chatMessageNew);
        Task<IEnumerable<ChatMessageDto>> GetChatMessages(string receiverId, int moduleId);
    }
}
