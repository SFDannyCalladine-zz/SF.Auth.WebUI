using System;

namespace SF.Common.Services
{
    public class BaseRequest
    {
        #region Public Properties

        public Guid UserGuid { get; private set; }

        #endregion Public Properties

        #region Public Constructors

        public BaseRequest(Guid userGuid)
        {
            UserGuid = userGuid;
        }

        #endregion Public Constructors
    }
}