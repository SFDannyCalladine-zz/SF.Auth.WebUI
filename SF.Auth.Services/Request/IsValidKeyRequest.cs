using SF.Common.Services;
using System;

namespace SF.Auth.Services.Request
{
    public class IsValidKeyRequest : BaseRequest
    {
        public string Key { get; private set; }

        public IsValidKeyRequest(
            string key,
            Guid userGuid)
            : base(userGuid)
        {
            Key = key;
        }
    }
}