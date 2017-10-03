using System;

namespace SF.Auth.Accounts
{
    public class CustomerUser
    {
        public string Email { get; private set; }

        public string Name { get; private set; }
        public string Password { get; private set; }
        public Guid UserGuid { get; private set; }

        public int UserId { get; private set; }

        protected CustomerUser()
        {
        }
    }
}