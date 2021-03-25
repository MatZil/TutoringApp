using TutoringApp.Data.Models.Enums;

namespace TutoringApp.Data.Dtos.Tutoring.TutoringSessions
{
    public class TutoringSessionEvaluationDto
    {
        public TutoringSessionEvaluationEnum Evaluation { get; set; }
        public string Comment { get; set; }
    }
}
