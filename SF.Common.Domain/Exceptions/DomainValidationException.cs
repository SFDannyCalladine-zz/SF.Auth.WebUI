using System;

namespace SF.Common.Domain.Exceptions
{
    public class DomainValidationException : ArgumentException
    {
        public DomainValidationException(
            string message)
            : base(message)
        {
        }

        public DomainValidationException(
            string paramName,
            string message)
            : base(
                  message,
                  paramName)
        {
        }
    }
}