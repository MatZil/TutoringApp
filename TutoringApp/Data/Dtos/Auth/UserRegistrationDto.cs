namespace TutoringApp.Data.Dtos.Auth
{
    public class UserRegistrationDto : UserLoginDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
