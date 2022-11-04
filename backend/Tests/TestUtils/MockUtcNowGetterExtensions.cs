using System;
using Application.Services;
using Moq;

namespace Tests.TestUtils
{
    public static class MockUtcNowGetterExtensions
    {
        public static DateTimeOffset SeedNow(this Mock<IUtcNowGetter> utcNowGetter)
        {
            var now = DateTimeOffset.UtcNow;
            utcNowGetter.Setup(e => e.UtcNow()).Returns(now);

            return now;
        }
    }
}