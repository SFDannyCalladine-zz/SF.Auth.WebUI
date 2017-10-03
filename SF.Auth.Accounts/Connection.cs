using System;

namespace SF.Auth.Accounts
{
    public class Connection
    {
        public virtual Account Account { get; private set; }
        public Guid ConnectionGuid { get; private set; }
        public string EncryptedConnectionString { get; private set; }

        protected Connection()
        {
        }
    }
}