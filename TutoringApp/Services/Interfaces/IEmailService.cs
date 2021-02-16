using System.Threading.Tasks;

namespace TutoringApp.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendConfirmationEmail(string receiverEmail, string emailConfirmationLink);
    }
}
