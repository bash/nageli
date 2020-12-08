using System;

namespace Nageli
{
    public class TomlException : Exception
    {
        public TomlException()
        {
        }

        public TomlException(string? message)
            : base(message)
        {
        }

        public TomlException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}
