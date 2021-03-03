using System;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Services.Shared
{
    public class TimeService : ITimeService
    {
        public DateTimeOffset GetCurrentTime()
        {
            return DateTimeOffset.UtcNow;
        }
    }
}
