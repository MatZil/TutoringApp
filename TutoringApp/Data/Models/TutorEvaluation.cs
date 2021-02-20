using TutoringApp.Data.Models.Enums;

namespace TutoringApp.Data.Models
{
    public class TutorEvaluation
    {
        public AppUser Tutor { get; set; }
        public string TutorId { get; set; }

        public AppUser Student { get; set; }
        public string StudentId { get; set; }

        public TutorEvaluationEnum Evaluation { get; set; }
        public string Comment { get; set; }
    }
}
