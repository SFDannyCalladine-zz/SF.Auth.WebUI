using System;

namespace SF.Common.Services
{
    public class BaseRequest
    {
        public Guid UserGuid { get; private set; }

        public BaseRequest(Guid userGuid)
        {
            UserGuid = userGuid;
        }
    }
}