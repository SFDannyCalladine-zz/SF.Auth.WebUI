using Microsoft.EntityFrameworkCore;
using SF.Auth.Accounts;
using SF.Auth.DataAccess.Mappings;

namespace SF.Auth.DataAccess
{
    public class dbCustomerDatabase : DbContext
    {
        public DbSet<CustomerUser> Users { get; set; }

        private readonly string _connectionString;

        public dbCustomerDatabase(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CustomerUserMapping());

            base.OnModelCreating(builder);
        }
    }
}