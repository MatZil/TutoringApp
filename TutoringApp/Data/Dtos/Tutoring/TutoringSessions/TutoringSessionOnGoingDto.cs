namespace TutoringApp.Data.Dtos.Tutoring.TutoringSessions
{
    public class TutoringSessionOnGoingDto
    {
        public int ModuleId { get; set; }
        public string ParticipantId { get; set; }
        public bool IsStudent { get; set; }
    }
}
