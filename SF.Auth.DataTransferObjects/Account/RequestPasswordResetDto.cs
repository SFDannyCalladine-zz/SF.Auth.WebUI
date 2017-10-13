using System;

namespace SF.Auth.DataTransferObjects.Account
{
    public class RequestPasswordResetDto
    {
        public Guid Key { get; set; }
        public Guid UserGuid { get; set; }
    }
}