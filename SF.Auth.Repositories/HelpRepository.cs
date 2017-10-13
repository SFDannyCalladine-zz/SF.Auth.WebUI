using SF.Auth.DataAccess;
using SF.Auth.Repositories.Cache;
using SF.Auth.Repositories.Interfaces;
using SF.Common.Help;
using SF.Common.Repositories.Cache.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SF.Auth.Repositories
{
    public class HelpRepository : CacheRepository, IHelpRepository
    {
        private const string HelpLinkKey = "HelpLinks";
        private readonly dbHelp _context;

        public HelpRepository(
            dbHelp context,
            ICacheStorage cache)
            : base(cache)
        {
            _context = context;
        }

        public IList<HelpLink> GetAllLinks()
        {
            var links = Cache.Retrieve<IList<HelpLink>>(HelpLinkKey);

            if (links == null)
            {
                links = _context
                    .HelpLinks
                    .ToList();

                if (links != null)
                {
                    Cache.Store(HelpLinkKey, links);
                }
            }

            return links;
        }
    }
}