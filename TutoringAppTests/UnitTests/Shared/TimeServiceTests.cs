using System;
using TutoringApp.Services.Interfaces;
using TutoringApp.Services.Shared;
using Xunit;

namespace TutoringAppTests.UnitTests.Shared
{
    public class TimeServiceTests
    {
        private readonly ITimeService _timeService;

        public TimeServiceTests()
        {
            _timeService = new TimeService();
        }

        [Fact]
        public void When_GettingCurrentTime_Expect_CorrectTime()
        {
            var currentTime = _timeService.GetCurrentTime();

            Assert.Equal(DateTimeOffset.Now.Date, currentTime.Date);
        }
    }
}
