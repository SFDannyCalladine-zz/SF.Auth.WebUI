using System;
using System.Linq;
using SF.Auth.Accounts;
using SF.Auth.DataAccess;
using SF.Auth.Repositories.Interfaces;

namespace SF.Auth.Repositories
{
    public class UserRepository : IUserRepository
    {
        #region Private Fields

        private readonly dbCustomerDatabase _context;

        #endregion Private Fields

        #region Public Constructors

        public UserRepository(dbCustomerDatabase context)
        {
            _context = context;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public User FindByKey(Guid key)
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

        #endregion Public Methods

        #region Private Methods

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

        #endregion Private Methods
    }
}