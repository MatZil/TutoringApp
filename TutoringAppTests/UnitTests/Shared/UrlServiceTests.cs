using TutoringApp.Services.Interfaces;
using TutoringApp.Services.Shared;
using TutoringAppTests.Setup;
using Xunit;

namespace TutoringAppTests.UnitTests.Shared
{
    public class UrlServiceTests
    {
        private readonly IUrlService _urlService;

        public UrlServiceTests()
        {
            _urlService = new UrlService(
                UnitTestSetup.GetConfiguration()
                );
        }

        [Theory]
        [InlineData("email@email.com", "EncodedToken")]
        public void When_GettingEmailConfirmationLink_Expect_CorrectLink(string email, string token)
        {
            var actualLink = _urlService.GetEmailConfirmationLink(email, token);

            Assert.Equal(
                $"https://localhost:5001/confirm-email?email={email}&token={token}",
                actualLink
                );
        }
    }
}
