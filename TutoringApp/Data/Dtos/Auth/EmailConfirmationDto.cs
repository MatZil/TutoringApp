namespace TutoringApp.Data.Dtos.Auth
{
    public class EmailConfirmationDto
    {
        public string Email { get; set; }
        public string EncodedToken { get; set; }
    }
}
