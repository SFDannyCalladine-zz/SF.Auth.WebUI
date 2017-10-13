using System;
using SF.Common.Root;

namespace SF.Common.Repositories.Interfaces
{
    public interface IRootRepository
    {
        #region Public Methods

        RootConnection FindConnectionByEmail(string email);

        RootConnection FindConnectionByUserGuid(Guid userGuid);

        #endregion Public Methods
    }
}