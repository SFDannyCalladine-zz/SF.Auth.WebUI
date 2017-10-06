using SF.Common.Domain.Exceptions;
using SF.Common.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SF.Auth.Accounts
{
    public class User
    {
        public string Email { get; private set; }

        public virtual IList<ForgottenPassword> ForgottenPasswords { get; private set; }
        public string Name { get; private set; }

        public string Password { get; private set; }

        public Guid UserGuid { get; private set; }

        public int UserId { get; private set; }

        protected User()
        {
        }

        public void AddPasswordResetRequest(ForgottenPassword passwordResetRequest)
        {
            if (passwordResetRequest == null)
            {
                throw new DomainValidationException(nameof(passwordResetRequest), "Password Reset Request can not be null.");
            }

            if (passwordResetRequest.Used)
            {
                throw new DomainValidationException("Can not add used request.");
            }

            foreach (var request in ForgottenPasswords.Where(x => !x.Used))
            {
                request.Deactivate();
            }

            ForgottenPasswords.Add(passwordResetRequest);
        }

        public bool IsValidKey(string key, int expiryLength)
        {
            var request = FindPasswordResetRequest(key);

            if (request == null)
            {
                return false;
            }

            if (!request.IsValid(expiryLength))
            {
                return false;
            }

            return true;
        }

        public void UpdatePassword(string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword))
            {
                throw new DomainValidationException(nameof(newPassword), "Password can not be null or empty.");
            }

            Password = Hashing.Hash(newPassword);
        }

        public void UsePasswordResetRequest(string key, int expiryLength)
        {
            var request = FindPasswordResetRequest(key);

            if (request == null)
            {
                throw new DomainValidationException("Password Reset Request with provided Key does not exist under the selected Agent.");
            }

            request.Use(expiryLength);
        }

        public bool ValidatePassword(string passwordToValidate)
        {
            return Hashing.Validate(passwordToValidate, Password);
        }

        private ForgottenPassword FindPasswordResetRequest(string key)
        {
            return ForgottenPasswords.FirstOrDefault(x => x.Key == key);
        }
    }
}