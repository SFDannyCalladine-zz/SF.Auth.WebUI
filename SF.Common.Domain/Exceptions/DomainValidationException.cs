using System;

namespace SF.Common.Domain.Exceptions
{
    public class DomainValidationException : ArgumentException
    {
        #region Public Constructors

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

        #endregion Public Constructors
    }
}