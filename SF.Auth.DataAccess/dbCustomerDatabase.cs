using Microsoft.EntityFrameworkCore;
using SF.Auth.Accounts;
using SF.Auth.DataAccess.Mappings.Account;

namespace SF.Auth.DataAccess
{
    public class dbCustomerDatabase : DbContext
    {
        public virtual DbSet<User> Users { get; set; }

        private readonly string _connectionString;

        protected dbCustomerDatabase() { }

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
            builder.ApplyConfiguration(new UserMapping());
            builder.ApplyConfiguration(new ForgottenPasswordMapping());

            base.OnModelCreating(builder);
        }
    }
}