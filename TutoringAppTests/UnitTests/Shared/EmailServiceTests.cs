using Microsoft.AspNetCore.Identity.UI.Services;
using Moq;
using System.Threading.Tasks;
using TutoringApp.Data.Aggregates;
using TutoringApp.Data.Dtos.Tutoring.TutoringSessions;
using TutoringApp.Services.Interfaces;
using TutoringApp.Services.Shared;
using TutoringAppTests.Setup;
using Xunit;

namespace TutoringAppTests.UnitTests.Shared
{
    public class EmailServiceTests
    {
        private readonly IEmailService _emailService;
        private readonly Mock<IEmailSender> _emailSenderMock;

        public EmailServiceTests()
        {
            _emailSenderMock = new Mock<IEmailSender>();

            _emailService = new EmailService(
                _emailSenderMock.Object,
                new UrlService(UnitTestSetup.GetConfiguration())
                );
        }

        [Fact]
        public async Task When_SendingConfirmationEmail_Expect_CorrectSubject()
        {
            await _emailService.SendConfirmationEmail("email", "any");

            _emailSenderMock.Verify(
                s => s.SendEmailAsync("email", "Tutoring App Email Confirmation", It.IsAny<string>()),
                Times.Once
                );
        }

        [Fact]
        public async Task When_SendingUserConfirmedEmail_Expect_CorrectSubject()
        {
            await _emailService.SendUserConfirmedEmail("email");

            _emailSenderMock.Verify(
                s => s.SendEmailAsync("email", "Confirmed at Tutoring App!", It.IsAny<string>()),
                Times.Once
            );
        }

        [Fact]
        public async Task When_SendingUserRejectedEmail_Expect_CorrectSubject()
        {
            await _emailService.SendUserRejectedEmail("email", "any");

            _emailSenderMock.Verify(
                s => s.SendEmailAsync("email", "Tutoring App Rejection", It.IsAny<string>()),
                Times.Once
            );
        }

        [Fact]
        public async Task When_SendingApplicationConfirmedEmail_Expect_CorrectSubject()
        {
            await _emailService.SendTutoringApplicationConfirmedEmail("email", "any");

            _emailSenderMock.Verify(
                s => s.SendEmailAsync("email", "Tutoring App Tutoring Confirmation", It.IsAny<string>()),
                Times.Once
            );
        }

        [Fact]
        public async Task When_SendingApplicationRejectedEmail_Expect_CorrectSubject()
        {
            await _emailService.SendTutoringApplicationRejectedEmail("email", "any");

            _emailSenderMock.Verify(
                s => s.SendEmailAsync("email", "Tutoring App Tutoring Rejection", It.IsAny<string>()),
                Times.Once
            );
        }

        [Fact]
        public async Task When_SendingSessionEvaluatedEmail_Expect_CorrectSubject()
        {
            await _emailService.SendTutoringSessionEvaluatedEmail(new SessionEvaluationEmailAggregate { TutorEmail = "email", StudentName = "any", EvaluationDto = new TutoringSessionEvaluationDto()});

            _emailSenderMock.Verify(
                s => s.SendEmailAsync("email", "Tutoring App Tutoring Evaluation", It.IsAny<string>()),
                Times.Once
            );
        }

        [Fact]
        public async Task When_SendingSessionReminderEmail_Expect_CorrectSubject()
        {
            await _emailService.SendTutoringSessionReminder("email");

            _emailSenderMock.Verify(
                s => s.SendEmailAsync("email", "Tutoring App Session Reminder", It.IsAny<string>()),
                Times.Once
            );
        }
    }
}
