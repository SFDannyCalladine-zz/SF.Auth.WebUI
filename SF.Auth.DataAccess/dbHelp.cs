using Microsoft.EntityFrameworkCore;
using SF.Auth.DataAccess.Mappings.Help;
using SF.Common.Help;

namespace SF.Auth.DataAccess
{
    public class dbHelp : DbContext
    {
        public DbSet<HelpLink> HelpLinks { get; set; }

        public dbHelp(DbContextOptions<dbHelp> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new HelpLinkMapping());

            base.OnModelCreating(builder);
        }
    }
}