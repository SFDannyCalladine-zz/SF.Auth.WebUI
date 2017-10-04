using System;

namespace SF.Auth.Accounts
{
    public class User
    {
        public Guid AccountGuid { get; private set; }

        public string Email { get; private set; }

        public Guid UserGuid { get; private set; }

        private User()
        {
        }
    }
}