using System;

namespace SF.Auth.DataTransferObjects.Account
{
    public class UserDto
    {
        #region Public Properties

        public string Email { get; set; }

        public string Name { get; set; }

        public Guid UserGuid { get; set; }

        public int UserId { get; set; }

        #endregion Public Properties
    }
}