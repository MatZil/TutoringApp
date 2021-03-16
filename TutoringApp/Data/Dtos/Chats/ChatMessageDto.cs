using System;

namespace TutoringApp.Data.Dtos.Chats
{
    public class ChatMessageDto
    {
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string Content { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}
