using Microsoft.EntityFrameworkCore;
using SF.Common.DataAccess.Mappings;
using SF.Common.Root;

namespace SF.Common.DataAccess
{
    public class dbRoot : DbContext
    {
        public DbSet<RootConnection> Connections { get; set; }

        public dbRoot(DbContextOptions<dbRoot> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new RootAccountMapping());
            builder.ApplyConfiguration(new RootConnectionMapping());
            builder.ApplyConfiguration(new RootUserMapping());

            base.OnModelCreating(builder);
        }
    }
}