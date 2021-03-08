using System;

namespace TheShop.Exceptions
{
    public class LoggedException : Exception
    {
        public LoggedException() : base()
        {

        }

        public LoggedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
