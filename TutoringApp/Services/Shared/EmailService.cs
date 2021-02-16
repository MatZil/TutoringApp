using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Services.Shared
{
    public class EmailService : IEmailService
    {
        private readonly IEmailSender _emailSender;

        public EmailService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task SendConfirmationEmail(string receiverEmail, string emailConfirmationLink)
        {
            var linkHtml = $"<a href='{HtmlEncoder.Default.Encode(emailConfirmationLink)}'>here</a>";
            var emailBody = $"Please confirm your registration in Tutoring App by clicking {linkHtml}.";
            const string subject = "Tutoring App Email Confirmation";
            await _emailSender.SendEmailAsync(receiverEmail, subject, emailBody);
        }
    }
}
