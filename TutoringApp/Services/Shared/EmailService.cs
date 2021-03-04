using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Services.Shared
{
    public class EmailService : IEmailService
    {
        private readonly IEmailSender _emailSender;
        private readonly IUrlService _urlService;

        public EmailService(
            IEmailSender emailSender,
            IUrlService urlService)
        {
            _emailSender = emailSender;
            _urlService = urlService;
        }

        public async Task SendConfirmationEmail(string receiverEmail, string emailConfirmationLink)
        {
            var linkHtml = $"<a href='{HtmlEncoder.Default.Encode(emailConfirmationLink)}'>here</a>";
            var emailBody = $"Please confirm your registration in Tutoring App by clicking {linkHtml}.";
            const string subject = "Tutoring App Email Confirmation";
            await _emailSender.SendEmailAsync(receiverEmail, subject, emailBody);
        }

        public async Task SendUserConfirmedEmail(string receiverEmail)
        {
            var appUrl = _urlService.GetAppUrl("/login");
            var linkHtml = $"<a href='{appUrl}'>login</a>";
            var emailBody = $"You have been confirmed to join Tutoring App! You may {linkHtml} and start learning!";
            const string subject = "Confirmed at Tutoring App!";
            await _emailSender.SendEmailAsync(receiverEmail, subject, emailBody);
        }

        public async Task SendUserRejectedEmail(string receiverEmail, string rejectionReason)
        {
            var emailBody = $"Unfortunately, your request to join Tutoring App has been rejected. Provided reason by admin:<br><br>{rejectionReason}";
            const string subject = "Tutoring App Rejection";
            await _emailSender.SendEmailAsync(receiverEmail, subject, emailBody);
        }

        public async Task SendTutoringApplicationConfirmedEmail(string receiverEmail, string moduleName)
        {
            var emailBody = $"Congratulations! Your application for tutoring in {moduleName} has been confirmed. You may now start spreading knowledge!";
            const string subject = "Tutoring App Tutoring Confirmation";
            await _emailSender.SendEmailAsync(receiverEmail, subject, emailBody);
        }

        public async Task SendTutoringApplicationRejectedEmail(string receiverEmail, string moduleName)
        {
            var emailBody = $"Unfortunately, your application for tutoring in {moduleName} has been declined. Do not stop learning and try applying in the future!";
            const string subject = "Tutoring App Tutoring Rejection";
            await _emailSender.SendEmailAsync(receiverEmail, subject, emailBody);
        }
    }
}
