using SF.Common.Services;
using System;

namespace SF.Auth.Services.Request
{
    public class ResetPasswordRequest : BaseRequest
    {
        public Guid Key { get; private set; }

        public string Password { get; private set; }

        public ResetPasswordRequest(
            Guid key,
            string password,
            Guid userGuid)
            : base(userGuid)
        {
            Key = key;
            Password = password;
        }
    }
}