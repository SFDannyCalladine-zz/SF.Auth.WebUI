using SF.Common.Services;
using System;

namespace SF.Auth.Services.Request
{
    public class IsValidKeyRequest : BaseRequest
    {
        public Guid Key { get; private set; }

        public IsValidKeyRequest(
            Guid key,
            Guid userGuid)
            : base(userGuid)
        {
            Key = key;
        }
    }
}