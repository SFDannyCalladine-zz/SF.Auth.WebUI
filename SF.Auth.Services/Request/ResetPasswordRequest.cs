using SF.Common.Services;
using System;

namespace SF.Auth.Services.Request
{
    public class ResetPasswordRequest : BaseRequest
    {
        public string Key { get; private set; }

        public string Password { get; private set; }

        public ResetPasswordRequest(
            string key,
            string password,
            Guid userGuid)
            : base(userGuid)
        {
            Key = key;
            Password = password;
        }
    }
}