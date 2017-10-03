using Microsoft.EntityFrameworkCore;
using SF.Auth.Accounts;
using SF.Auth.DataAccess.Mappings;

namespace SF.Auth.DataAccess
{
    public class dbAdmin : DbContext
    {
        public DbSet<Connection> Connections { get; set; }

        public dbAdmin(DbContextOptions<dbAdmin> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AccountMapping());
            builder.ApplyConfiguration(new ConnectionMapping());
            builder.ApplyConfiguration(new UserMapping());

            base.OnModelCreating(builder);
        }
    }
}