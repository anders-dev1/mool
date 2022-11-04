using System;

namespace Application.Exceptions
{
    /// <summary>
    /// Throwing this exception will result in HTTP code 409.
    /// </summary>
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }
    }
}