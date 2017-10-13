using System.Collections.Generic;
using System.Linq;
using SF.Auth.DataAccess;
using SF.Auth.Repositories.Cache;
using SF.Auth.Repositories.Interfaces;
using SF.Common.Help;
using SF.Common.Repositories.Cache.Interfaces;

namespace SF.Auth.Repositories
{
    public class HelpRepository : CacheRepository, IHelpRepository
    {
        #region Private Fields

        private const string HelpLinkKey = "HelpLinks";
        private readonly dbHelp _context;

        #endregion Private Fields

        #region Public Constructors

        public HelpRepository(
            dbHelp context,
            ICacheStorage cache)
            : base(cache)
        {
            _context = context;
        }

        #endregion Public Constructors

        #region Public Methods

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

        #endregion Public Methods
    }
}