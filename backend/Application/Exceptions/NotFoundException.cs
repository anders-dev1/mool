using System;

namespace Application.Exceptions
{
    /// <summary>
    /// Throwing this exception will result in HTTP code 404.
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message){}
    }
}