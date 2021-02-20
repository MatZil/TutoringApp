using System;
using TutoringApp.Data.Models.Base;
using TutoringApp.Data.Models.Enums;

namespace TutoringApp.Data.Models
{
    public class TutoringSession : BaseEntity
    {
        public DateTimeOffset RequestDate { get; set; }
        public bool IsSubscribed { get; set; }

        public TutoringSessionStatusEnum Status { get; set; }
        public DateTimeOffset? StatusChangeDate { get; set; }

        public DateTimeOffset SessionDate { get; set; }

        public bool IsReminderSent { get; set; }

        public AppUser Student { get; set; }
        public string StudentId { get; set; }

        public AppUser Tutor { get; set; }
        public string TutorId { get; set; }
    }
}
