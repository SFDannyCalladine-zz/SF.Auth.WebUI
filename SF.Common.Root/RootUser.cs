using System;

namespace SF.Common.Root
{
    public class RootUser
    {
        #region Public Properties

        public Guid AccountGuid { get; private set; }

        public string Email { get; private set; }

        public Guid UserGuid { get; private set; }

        #endregion Public Properties

        #region Private Constructors

        private RootUser()
        {
        }

        #endregion Private Constructors
    }
}