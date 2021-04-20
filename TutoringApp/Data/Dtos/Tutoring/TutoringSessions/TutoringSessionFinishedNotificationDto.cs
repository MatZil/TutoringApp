namespace TutoringApp.Data.Dtos.Tutoring.TutoringSessions
{
    public class TutoringSessionFinishedNotificationDto
    {
        public int SessionId { get; set; }
        public string TutorName { get; set; }
        public bool OpenNotificationDialog { get; set; }
        public string ParticipantId { get; set; }
    }
}
