namespace TutoringApp.Data.Extensions
{
    public static class StringExtensions
    {
        public static bool IsKtuEmail(this string email)
        {
            var domainStartIndex = email.LastIndexOf('@') + 1;
            var domain = email[domainStartIndex..].ToLower();

            return domain == "ktu.edu";
        }
    }
}
