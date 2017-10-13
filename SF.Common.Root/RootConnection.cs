using System;
using SF.Common.Domain.Exceptions;

namespace SF.Common.Root
{
    public class RootConnection
    {
        #region Public Properties

        public virtual RootAccount Account { get; private set; }

        public Guid ConnectionGuid { get; private set; }

        public string EncryptedConnectionString { get; private set; }

        #endregion Public Properties

        #region Public Constructors

        public RootConnection(
            Guid connectionGuid,
            string encryptedConnectionString)
        {
            if (string.IsNullOrWhiteSpace(encryptedConnectionString))
            {
                throw new DomainValidationException(nameof(encryptedConnectionString), "Encrypted Connection String can not be null or empty.");
            }

            ConnectionGuid = connectionGuid;
            EncryptedConnectionString = encryptedConnectionString;
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected RootConnection()
        {
        }

        #endregion Protected Constructors
    }
}