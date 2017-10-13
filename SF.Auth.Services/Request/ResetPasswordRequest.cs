using System;
using SF.Common.Services;

namespace SF.Auth.Services.Request
{
    public class ResetPasswordRequest : BaseRequest
    {
        #region Public Properties

        public Guid Key { get; private set; }

        public string Password { get; private set; }

        #endregion Public Properties

        #region Public Constructors

        public ResetPasswordRequest(
            Guid key,
            string password,
            Guid userGuid)
            : base(userGuid)
        {
            Key = key;
            Password = password;
        }

        #endregion Public Constructors
    }
}