using System.Threading.Tasks;
using TutoringApp.Data.Aggregates;

namespace TutoringApp.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendConfirmationEmail(string receiverEmail, string emailConfirmationLink);
        Task SendUserConfirmedEmail(string receiverEmail);
        Task SendUserRejectedEmail(string receiverEmail, string rejectionReason);
        Task SendTutoringApplicationConfirmedEmail(string receiverEmail, string moduleName);
        Task SendTutoringApplicationRejectedEmail(string receiverEmail, string moduleName);
        Task SendTutoringSessionEvaluatedEmail(SessionEvaluationEmailAggregate evaluationEmailAggregate);
        Task SendTutoringSessionReminder(string receiverEmail);
    }
}
