using Microsoft.EntityFrameworkCore;
using SF.Auth.Accounts;
using SF.Auth.DataAccess.Mappings.Account;

namespace SF.Auth.DataAccess
{
    public class dbCustomerDatabase : DbContext
    {
        #region Public Properties

        public virtual DbSet<User> Users { get; set; }

        #endregion Public Properties

        #region Private Fields

        private readonly string _connectionString;

        #endregion Private Fields

        #region Public Constructors

        public dbCustomerDatabase(string connectionString)
        {
            _connectionString = connectionString;
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected dbCustomerDatabase()
        {
        }

        #endregion Protected Constructors

        #region Protected Methods

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

        #endregion Protected Methods
    }
}