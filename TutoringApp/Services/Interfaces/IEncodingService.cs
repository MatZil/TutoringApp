namespace TutoringApp.Services.Interfaces
{
    public interface IEncodingService
    {
        string GetWebEncodedString(string plainText);
        string GetWebDecodedString(string encodedText);
    }
}
