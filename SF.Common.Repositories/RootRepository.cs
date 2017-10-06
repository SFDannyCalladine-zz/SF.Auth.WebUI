using SF.Common.DataAccess;
using SF.Common.Repositories.Interfaces;
using SF.Common.Root;
using System;
using System.Linq;

namespace SF.Common.Repositories
{
    public class RootRepository : IRootRepository
    {
        private readonly dbRoot _context;

        public RootRepository(dbRoot context)
        {
            _context = context;
        }

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
    }
}