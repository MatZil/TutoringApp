using System;

namespace TutoringApp.Services.Interfaces
{
    public interface ITimeService
    {
        DateTimeOffset GetCurrentTime();
    }
}
