using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;
using System;
using System.Threading.Tasks;

namespace TutoringApp.Infrastructure.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender
            (IConfiguration configuration,
            ILogger<EmailSender> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = GetConfiguredMessage(email, subject, htmlMessage);
            await SendMessageAsync(message);
        }

        private MimeMessage GetConfiguredMessage(string recipientEmail, string subject, string htmlMessage)
        {
            var displayName = _configuration["EmailSender:DisplayName"];
            var address = _configuration["EmailSender:Address"];
            var message = new MimeMessage();

            try
            {
                message.From.Add(new MailboxAddress(displayName, address));
                message.To.Add(new MailboxAddress("Recipient", recipientEmail));
                message.Subject = subject;
                message.Body = new TextPart(TextFormat.Html) { Text = htmlMessage };

                _logger.LogInformation("Email message successfully configured.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error occurred while configuring a message: {ex}");
            }

            return message;
        }

        private async Task SendMessageAsync(MimeMessage message)
        {
            var host = _configuration["EmailSender:Host"];
            var port = _configuration.GetValue<int>("EmailSender:Port");
            var address = _configuration["EmailSender:Address"];
            var password = _configuration["EmailSender:Password"];
            using var smtpClient = new SmtpClient();

            try
            {
                smtpClient.Connect(host, port, SecureSocketOptions.StartTls);
                smtpClient.Authenticate(address, password);

                await smtpClient.SendAsync(message);
                await smtpClient.DisconnectAsync(true);

                _logger.LogInformation("Email message successfully sent.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error occurred while sending a configured message: {ex}");
            }
        }
    }
}
