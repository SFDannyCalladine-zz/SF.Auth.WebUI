using Microsoft.EntityFrameworkCore;
using SF.Common.DataAccess.Mappings;
using SF.Common.Root;

namespace SF.Common.DataAccess
{
    public class dbRoot : DbContext
    {
        #region Public Properties

        public DbSet<RootConnection> Connections { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public dbRoot(DbContextOptions<dbRoot> options)
            : base(options)
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new RootAccountMapping());
            builder.ApplyConfiguration(new RootConnectionMapping());
            builder.ApplyConfiguration(new RootUserMapping());

            base.OnModelCreating(builder);
        }

        #endregion Protected Methods
    }
}