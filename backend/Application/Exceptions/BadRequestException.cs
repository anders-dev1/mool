using System;

namespace Application.Exceptions
{
    /// <summary>
    /// Throwing this exception will result in HTTP code 400.
    /// </summary>
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message){ }
    }
}