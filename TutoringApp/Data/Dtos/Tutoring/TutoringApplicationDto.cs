using System;
using TutoringApp.Data.Models.Enums;

namespace TutoringApp.Data.Dtos.Tutoring
{
    public class TutoringApplicationDto
    {
        public int Id { get; set; }
        public string ModuleName { get; set; }
        public string StudentName { get; set; }
        public string Email { get; set; }
        public StudentCycleEnum StudentCycle { get; set; }
        public StudentYearEnum StudentYear { get; set; }
        public string Faculty { get; set; }
        public string StudyBranch { get; set; }
        public DateTimeOffset RequestDate { get; set; }
        public string MotivationalLetter { get; set; }
    }
}
