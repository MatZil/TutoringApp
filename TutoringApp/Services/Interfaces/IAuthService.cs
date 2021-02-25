using System.Threading.Tasks;
using TutoringApp.Data.Dtos.Auth;

namespace TutoringApp.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> Register(UserRegistrationDto userRegistration);
        Task SendConfirmationEmail(string userId);
        Task ConfirmEmail(string email, string encodedToken);
        Task<LoginResponseDto> Login(UserLoginDto userLogin);
    }
}
