using System.ComponentModel.DataAnnotations;
using TutoringApp.Data.Models.Base;

namespace TutoringApp.Data.Models
{
    public class Assignment : BaseEntity
    {
        public AppUser Tutor { get; set; }
        public string TutorId { get; set; }

        public AppUser Student { get; set; }
        public string StudentId { get; set; }

        public Module Module { get; set; }
        public int ModuleId { get; set; }

        [Required]
        public string AssignmentFileName { get; set; }
        public string SubmissionFileName { get; set; }
        public int? SubmissionEvaluation { get; set; }
    }
}
