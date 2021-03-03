using System;
using System.ComponentModel.DataAnnotations;
using TutoringApp.Data.Models.Base;

namespace TutoringApp.Data.Models
{
    public class TutoringApplication : BaseEntity
    {
        public AppUser Student { get; set; }
        public string StudentId { get; set; }

        public int ModuleId { get; set; }
        public Module Module { get; set; }

        public DateTimeOffset? RequestDate { get; set; }
        
        [Required]
        public string MotivationalLetter { get; set; }
    }
}
