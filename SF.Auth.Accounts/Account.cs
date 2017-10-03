using System;
using System.Collections.Generic;

namespace SF.Auth.Accounts
{
    public class Account
    {
        public Guid AccountGuid { get; private set; }

        public virtual Connection Connection { get; private set; }

        public Guid ConnectionGuid { get; private set; }

        public virtual IList<User> Users { get; private set; }

        protected Account()
        {
        }
    }
}