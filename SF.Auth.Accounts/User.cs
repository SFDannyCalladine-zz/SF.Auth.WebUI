using System;
using System.Collections.Generic;
using System.Linq;
using SF.Common.Domain.Exceptions;
using SF.Common.Security;

namespace SF.Auth.Accounts
{
    public class User
    {
        #region Public Properties

        public string Email { get; private set; }

        public virtual IList<ForgottenPassword> ForgottenPasswords { get; private set; }

        public string Name { get; private set; }

        public string Password { get; private set; }

        public Guid UserGuid { get; private set; }

        public int UserId { get; private set; }

        #endregion Public Properties

        #region Public Constructors

        public User(
            Guid userGuid,
            string name,
            string emailAddress,
            string password)
            : this()
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new DomainValidationException(nameof(name), "Name can not be null or empty.");
            }

            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                throw new DomainValidationException(nameof(emailAddress), "Email Address can not be null or empty.");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new DomainValidationException(nameof(password), "Password can not be null or empty.");
            }

            UserGuid = userGuid;
            Name = name;
            Email = emailAddress;
            Password = Hashing.Hash(password);
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected User()
        {
            UserId = 0;
            ForgottenPasswords = new List<ForgottenPassword>();
        }

        #endregion Protected Constructors

        #region Public Methods

        public void AddPasswordResetRequest(
            ForgottenPassword passwordResetRequest,
            int expiryLength)
        {
            if (passwordResetRequest == null)
            {
                throw new DomainValidationException(nameof(passwordResetRequest), "Password Reset Request can not be null.");
            }

            if (!passwordResetRequest.IsValid(expiryLength))
            {
                throw new DomainValidationException("Can not add invalid request.");
            }

            foreach (var request in ForgottenPasswords.Where(x => !x.Used))
            {
                request.Deactivate();
            }

            ForgottenPasswords.Add(passwordResetRequest);
        }

        public bool IsValidKey(Guid key, int expiryLength)
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

        public void UpdatePasswordWithKey(Guid key, int expiryLength, string password)
        {
            var valid = IsValidKey(key, expiryLength);

            if (!valid)
            {
                throw new DomainValidationException("Password Reset Request is not valid.");
            }

            var request = FindPasswordResetRequest(key);

            if (request == null)
            {
                throw new DomainValidationException("Password Reset Request with provided Key does not exist under the selected Agent.");
            }

            request.Use(expiryLength);

            UpdatePassword(password);
        }

        public bool ValidatePassword(string passwordToValidate)
        {
            return Hashing.Validate(passwordToValidate, Password);
        }

        #endregion Public Methods

        #region Private Methods

        private ForgottenPassword FindPasswordResetRequest(Guid key)
        {
            return ForgottenPasswords.FirstOrDefault(x => x.Key == key);
        }

        private void UpdatePassword(string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword))
            {
                throw new DomainValidationException(nameof(newPassword), "Password can not be null or empty.");
            }

            Password = Hashing.Hash(newPassword);
        }

        #endregion Private Methods
    }
}