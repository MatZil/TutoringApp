using System;
using TutoringApp.Data.Models.Base;

namespace TutoringApp.Data.Models
{
    public class Assignment : BaseEntity
    {
        public AppUser Tutor { get; set; }
        public string TutorId { get; set; }

        public AppUser Student { get; set; }
        public string StudentId { get; set; }

        public DateTimeOffset CreationDate { get; set; }

        // Tutor side
        public string Title { get; set; }
        public string Content { get; set; }
        public string AssignmentFileName { get; set; }

        // Student side
        public string Comment { get; set; }
        public string SubmissionFileName { get; set; }
    }
}
