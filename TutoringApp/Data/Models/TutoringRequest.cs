using System;
using TutoringApp.Data.Models.Base;
using TutoringApp.Data.Models.Enums;

namespace TutoringApp.Data.Models
{
    public class TutoringRequest : BaseEntity
    {
        public AppUser Student { get; set; }
        public string StudentId { get; set; }

        public DateTimeOffset? RequestDate { get; set; }

        public TutoringRequestStatusEnum Status { get; set; }
        public DateTimeOffset? StatusChangeDate { get; set; }
    }
}
