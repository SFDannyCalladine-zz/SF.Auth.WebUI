using Microsoft.EntityFrameworkCore;
using SF.Auth.DataAccess.Mappings.Help;
using SF.Common.Help;

namespace SF.Auth.DataAccess
{
    public class dbHelp : DbContext
    {
        #region Public Properties

        public DbSet<HelpLink> HelpLinks { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public dbHelp(DbContextOptions<dbHelp> options)
            : base(options)
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new HelpLinkMapping());

            base.OnModelCreating(builder);
        }

        #endregion Protected Methods
    }
}