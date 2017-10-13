using System;

namespace SF.Auth.DataTransferObjects.Account
{
    public class RequestPasswordResetDto
    {
        #region Public Properties

        public Guid Key { get; set; }
        public Guid UserGuid { get; set; }

        #endregion Public Properties
    }
}