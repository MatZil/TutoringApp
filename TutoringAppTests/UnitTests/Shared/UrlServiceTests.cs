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

        [Theory]
        [InlineData("/my-sub-route/another", "https://localhost:5001/my-sub-route/another")]
        [InlineData("", "https://localhost:5001")]
        public void When_Getting_AppUrl_Expect_CorrectUrl(string subRoute, string expectedResult)
        {
            var actualResult = _urlService.GetAppUrl(subRoute);

            Assert.Equal(expectedResult, actualResult);
        }
    }
}
