using System;
using TutoringApp.Data.Models.Base;

namespace TutoringApp.Data.Models
{
    public class ChatMessage : BaseEntity
    {
        public AppUser Sender { get; set; }
        public string SenderId { get; set; }

        public AppUser Receiver { get; set; }
        public string ReceiverId { get; set; }

        public DateTimeOffset Timestamp { get; set; }
        public string Content { get; set; }
    }
}
