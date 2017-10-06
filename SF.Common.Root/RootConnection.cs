using System;

namespace SF.Common.Root
{
    public class RootConnection
    {
        public virtual RootAccount Account { get; private set; }

        public Guid ConnectionGuid { get; private set; }

        public string EncryptedConnectionString { get; private set; }

        protected RootConnection()
        {
        }
    }
}