using System;
using SF.Common.Domain.Exceptions;

namespace SF.Auth.Accounts
{
    public class ForgottenPassword
    {
        #region Public Properties

        public DateTime Created { get; private set; }

        public Guid Key { get; private set; }

        public bool Used { get; private set; }

        public int UserId { get; private set; }

        #endregion Public Properties

        #region Public Constructors

        public ForgottenPassword(Guid key)
        {
            Key = key;
            Created = DateTime.Now;
            Used = false;
            UserId = 0;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Deactivate()
        {
            Used = true;
        }

        public bool IsExpired(int expiryLength)
        {
            if (Created.AddMinutes(expiryLength) < DateTime.Now)
            {
                return true;
            }

            return false;
        }

        public bool IsValid(int expiryLength)
        {
            if (Used)
            {
                return false;
            }

            if (IsExpired(expiryLength))
            {
                return false;
            }

            return true;
        }

        public void Use(int expiryLength)
        {
            if (Used)
            {
                throw new DomainValidationException("Request has already been used.");
            }

            if (IsExpired(expiryLength))
            {
                throw new DomainValidationException("Can not use expired request.");
            }

            Used = true;
        }

        #endregion Public Methods
    }
}