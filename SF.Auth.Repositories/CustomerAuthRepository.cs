using SF.Auth.Accounts;
using SF.Auth.DataAccess;
using SF.Auth.Repositories.Interfaces;
using System.Linq;

namespace SF.Auth.Repositories
{
    public class CustomerAuthRepository : ICustomerAuthRepository
    {
        private readonly dbCustomerDatabase _context;

        public CustomerAuthRepository(dbCustomerDatabase context)
        {
            _context = context;
        }

        public CustomerUser FindUserByEmail(string email)
        {
            return _context
                .Users
                .FirstOrDefault(x => x.Email == email);
        }
    }
}