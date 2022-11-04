using System;

namespace Application.Services
{
    public interface IUtcNowGetter
    {
        DateTimeOffset UtcNow();
    }

    public class UtcNowGetter : IUtcNowGetter
    {
        public DateTimeOffset UtcNow()
        {
            return DateTimeOffset.UtcNow;
        }
    }
}