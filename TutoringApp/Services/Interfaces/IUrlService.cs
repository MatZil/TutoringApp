﻿namespace TutoringApp.Services.Interfaces
{
    public interface IUrlService
    {
        string GetEmailConfirmationLink(string email, string encodedEmailConfirmationToken);
    }
}
