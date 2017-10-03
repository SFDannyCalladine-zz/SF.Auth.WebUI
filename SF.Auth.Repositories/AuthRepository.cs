using SF.Auth.Accounts;
using SF.Auth.DataAccess;
using SF.Auth.Repositories.Interfaces;
using System.Linq;

namespace SF.Auth.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly dbAdmin _context;

        public AuthRepository(dbAdmin context)
        {
            _context = context;
        }

        public Connection FindConnectionByEmail(string email)
        {
            return _context
                .Connections
                .FirstOrDefault(x => x.Account.Users.Count(c => c.Email == email) > 0);
        }
    }
}