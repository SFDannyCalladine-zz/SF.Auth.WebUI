using System;
using System.Collections.Generic;

namespace SF.Common.Root
{
    public class RootAccount
    {
        #region Public Properties

        public Guid AccountGuid { get; private set; }

        public virtual RootConnection Connection { get; private set; }

        public Guid ConnectionGuid { get; private set; }

        public virtual IList<RootUser> Users { get; private set; }

        #endregion Public Properties

        #region Protected Constructors

        protected RootAccount()
        {
        }

        #endregion Protected Constructors
    }
}