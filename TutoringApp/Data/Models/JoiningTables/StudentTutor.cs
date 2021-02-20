namespace TutoringApp.Data.Models.JoiningTables
{
    public class StudentTutor
    {
        public AppUser Tutor { get; set; }
        public string TutorId { get; set; }

        public AppUser Student { get; set; }
        public string StudentId { get; set; }
    }
}
