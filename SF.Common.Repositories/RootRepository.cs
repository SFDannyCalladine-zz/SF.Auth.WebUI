using System;
using System.Linq;
using SF.Common.DataAccess;
using SF.Common.Repositories.Interfaces;
using SF.Common.Root;

namespace SF.Common.Repositories
{
    public class RootRepository : IRootRepository
    {
        #region Private Fields

        private readonly dbRoot _context;

        #endregion Private Fields

        #region Public Constructors

        public RootRepository(dbRoot context)
        {
            _context = context;
        }

        #endregion Public Constructors

        #region Public Methods

        public RootConnection FindConnectionByEmail(string email)
        {
            return _context
                .Connections
                .FirstOrDefault(x => x.Account.Users.Count(c => c.Email == email) > 0);
        }

        public RootConnection FindConnectionByUserGuid(Guid userGuid)
        {
            return _context
                .Connections
                .FirstOrDefault(x => x.Account.Users.Count(c => c.UserGuid == userGuid) > 0);
        }

        #endregion Public Methods
    }
}