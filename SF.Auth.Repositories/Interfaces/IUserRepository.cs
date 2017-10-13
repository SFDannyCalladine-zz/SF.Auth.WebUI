using System;
using SF.Auth.Accounts;
using SF.Common.Repositories.Interfaces;

namespace SF.Auth.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        #region Public Methods

        User FindByKey(Guid key);

        User FindUserByEmail(string email);

        #endregion Public Methods
    }
}