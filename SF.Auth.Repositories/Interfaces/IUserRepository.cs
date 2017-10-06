using SF.Auth.Accounts;
using SF.Common.Repositories.Interfaces;

namespace SF.Auth.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User FindByKey(string key);

        User FindUserByEmail(string email);
    }
}