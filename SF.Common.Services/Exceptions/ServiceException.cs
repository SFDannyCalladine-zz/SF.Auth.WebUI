using SF.Common.ServiceModels.Response;
using System;

namespace SF.Common.Services.Exceptions
{
    public class ServiceException : Exception
    {
        public ResponseCode Code { get; private set; }

        public ServiceException(
            ResponseCode code,
            string message)
            : base(message)
        {
            Code = code;
        }
    }
}