using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Services.Shared
{
    public class EncodingService : IEncodingService
    {
        public string GetWebEncodedString(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            var encodedText = WebEncoders.Base64UrlEncode(plainTextBytes);

            return encodedText;
        }

        public string GetWebDecodedString(string encodedText)
        {
            var decodedBytes = WebEncoders.Base64UrlDecode(encodedText);
            var decodedText = Encoding.UTF8.GetString(decodedBytes);

            return decodedText;
        }
    }
}
