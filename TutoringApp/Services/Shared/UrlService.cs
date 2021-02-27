using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Services.Shared
{
    public class UrlService : IUrlService
    {
        private readonly string _rootUrl;

        public UrlService(IConfiguration configuration)
        {
            _rootUrl = configuration["AppSettings:RootUrl"];
        }

        public string GetEmailConfirmationLink(string email, string encodedEmailConfirmationToken)
        {
            var queryParams = GetEmailTokenParams(email, encodedEmailConfirmationToken);

            var url = _rootUrl + "/confirm-email";
            return QueryHelpers.AddQueryString(url, queryParams);
        }

        public string GetAppUrl(string subRoute = "")
        {
            return _rootUrl + subRoute;
        }

        private static Dictionary<string, string> GetEmailTokenParams(string email, string token)
        {
            return new Dictionary<string, string>()
            {
                { nameof(email), email },
                { nameof(token), token }
            };
        }
    }
}
