using System;
using SF.Common.Services;

namespace SF.Auth.Services.Request
{
    public class IsValidKeyRequest : BaseRequest
    {
        #region Public Properties

        public Guid Key { get; private set; }

        #endregion Public Properties

        #region Public Constructors

        public IsValidKeyRequest(
            Guid key,
            Guid userGuid)
            : base(userGuid)
        {
            Key = key;
        }

        #endregion Public Constructors
    }
}