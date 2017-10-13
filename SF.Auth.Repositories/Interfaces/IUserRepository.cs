using SF.Auth.Accounts;
using SF.Common.Repositories.Interfaces;
using System;

namespace SF.Auth.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User FindByKey(Guid key);

        User FindUserByEmail(string email);
    }
}