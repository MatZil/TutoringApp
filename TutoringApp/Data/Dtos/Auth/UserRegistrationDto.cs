using TutoringApp.Data.Models.Enums;

namespace TutoringApp.Data.Dtos.Auth
{
    public class UserRegistrationDto : UserLoginDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public StudentCycleEnum StudentCycle { get; set; }
        public StudentYearEnum StudentYear { get; set; }
        public string Faculty { get; set; }
        public string StudyBranch { get; set; }
    }
}
