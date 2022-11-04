using System;

namespace Application.Exceptions
{
    /// <summary>
    /// Throwing this exception will result in HTTP code 401
    /// </summary>
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message) { }
    }
}