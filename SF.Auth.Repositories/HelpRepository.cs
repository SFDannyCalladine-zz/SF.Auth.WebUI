using SF.Auth.DataAccess;
using SF.Auth.Repositories.Interfaces;
using SF.Common.Help;
using System.Collections.Generic;
using System.Linq;

namespace SF.Auth.Repositories
{
    public class HelpRepository : IHelpRepository
    {
        private readonly dbHelp _context;

        public HelpRepository(dbHelp context)
        {
            _context = context;
        }

        public IList<HelpLink> GetAllLinks()
        {
            return _context
                .HelpLinks
                .ToList();
        }
    }
}