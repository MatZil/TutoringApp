using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TutoringApp.Data.Models.JoiningTables;

namespace TutoringApp.Data.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public IList<TutoringSession> TutoredSessions { get; set; }
        public IList<TutoringSession> LearningSessions { get; set; }
        public IList<TutoringRequest> TutoringRequests { get; set; }
        public IList<ChatMessage> SentMessages { get; set; }
        public IList<ChatMessage> ReceivedMessages { get; set; }
        public IList<Assignment> TutorAssignments { get; set; }
        public IList<Assignment> StudentAssignments { get; set; }
        public IList<TutorEvaluation> TutorEvaluations { get; set; }
        public IList<TutorEvaluation> StudentEvaluations { get; set; }
        public IList<StudentTutorIgnoration> IgnorationsToStudents { get; set; }
        public IList<StudentTutorIgnoration> IgnorationsFromTutors { get; set; }
        public IList<ModuleTutor> TutorModules { get; set; }
    }
}
