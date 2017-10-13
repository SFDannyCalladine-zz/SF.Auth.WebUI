using System;

namespace SF.Auth.DataTransferObjects.Account
{
    public class RequestPasswordResetDto
    {
        public Guid UserGuid { get; set; }

        public Guid Key { get; set; }
    }
}
