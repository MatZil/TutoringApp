using System;

namespace TutoringApp.Data.Dtos.Tutoring.TutoringSessions
{
    public class TutoringSessionNewDto
    {
        public bool IsSubscribed { get; set; }
        public DateTimeOffset SessionDate { get; set; }
        public string StudentId { get; set; }
        public int ModuleId { get; set; }
    }
}
