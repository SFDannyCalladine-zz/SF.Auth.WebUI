using SF.Auth.Accounts;

namespace SF.Auth.Repositories.Interfaces
{
    public interface ICustomerAuthRepository
    {
        CustomerUser FindUserByEmail(string email);
    }
}