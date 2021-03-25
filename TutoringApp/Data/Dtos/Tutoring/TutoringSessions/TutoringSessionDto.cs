using System;
using TutoringApp.Data.Models.Enums;

namespace TutoringApp.Data.Dtos.Tutoring.TutoringSessions
{
    public class TutoringSessionDto
    {
        public int Id { get; set; }
        public string ModuleName { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public bool IsSubscribed { get; set; }
        public DateTimeOffset SessionDate { get; set; }
        public TutoringSessionStatusEnum Status { get; set; }
        public DateTimeOffset? StatusChangeDate { get; set; }
        public TutoringSessionEvaluationEnum? Evaluation { get; set; }
        public string EvaluationComment { get; set; }
        public string ParticipantName { get; set; }
    }
}
