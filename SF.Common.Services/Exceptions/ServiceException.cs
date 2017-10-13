using System;
using SF.Common.ServiceModels.Response;

namespace SF.Common.Services.Exceptions
{
    public class ServiceException : Exception
    {
        #region Public Properties

        public ResponseCode Code { get; private set; }

        #endregion Public Properties

        #region Public Constructors

        public ServiceException(
            ResponseCode code,
            string message)
            : base(message)
        {
            Code = code;
        }

        #endregion Public Constructors
    }
}