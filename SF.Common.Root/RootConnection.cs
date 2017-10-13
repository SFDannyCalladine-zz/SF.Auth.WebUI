using SF.Common.Domain.Exceptions;
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

        public RootConnection(
            Guid connectionGuid,
            string encryptedConnectionString)
        {
            if(string.IsNullOrWhiteSpace(encryptedConnectionString))
            {
                throw new DomainValidationException(nameof(encryptedConnectionString), "Encrypted Connection String can not be null or empty.");
            }

            ConnectionGuid = connectionGuid;
            EncryptedConnectionString = encryptedConnectionString;
        }
    }
}