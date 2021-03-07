using TutoringApp.Data.Models.Base;

namespace TutoringApp.Data.Models.JoiningTables
{
    public class StudentTutorIgnore : BaseEntity
    {
        public AppUser Tutor { get; set; }
        public string TutorId { get; set; }

        public AppUser Student { get; set; }
        public string StudentId { get; set; }
    }
}
