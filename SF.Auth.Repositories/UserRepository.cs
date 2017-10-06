using SF.Auth.Accounts;
using SF.Auth.DataAccess;
using SF.Auth.Repositories.Interfaces;
using System;
using System.Linq;

namespace SF.Auth.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly dbCustomerDatabase _context;

        public UserRepository(dbCustomerDatabase context)
        {
            _context = context;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public User FindByKey(string key)
        {
            return _context
                .Users
                .FirstOrDefault(x => x.ForgottenPasswords.Count(c => c.Key == key) > 0);
        }

        public User FindUserByEmail(string email)
        {
            return _context
                .Users
                .FirstOrDefault(x => x.Email == email);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}