namespace TutoringApp.Data.Dtos.Tutoring.Assignments
{
    public class AssignmentDto
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string SubmissionFileName { get; set; }
        public int? SubmissionEvaluation { get; set; }
    }
}
