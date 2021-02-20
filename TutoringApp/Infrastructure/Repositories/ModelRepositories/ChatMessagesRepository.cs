using TutoringApp.Data;
using TutoringApp.Data.Models;

namespace TutoringApp.Infrastructure.Repositories.ModelRepositories
{
    public class ChatMessagesRepository : BaseRepository<ChatMessage>
    {
        public ChatMessagesRepository(ApplicationDbContext context) : base(context)
        {
            ItemSet = context.ChatMessages;
        }
    }
}
