using System;
using TutoringApp.Data.Dtos.Tutoring.TutoringSessions;

namespace TutoringApp.Data.Aggregates
{
    public class SessionEvaluationEmailAggregate
    {
        public string TutorEmail { get; set; }
        public string StudentName { get; set; }
        public DateTimeOffset SessionDate { get; set; }
        public TutoringSessionEvaluationDto EvaluationDto { get; set; }
    }
}
