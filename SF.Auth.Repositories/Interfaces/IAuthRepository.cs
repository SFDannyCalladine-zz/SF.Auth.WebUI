using SF.Auth.Accounts;

namespace SF.Auth.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Connection FindConnectionByEmail(string email);
    }
}