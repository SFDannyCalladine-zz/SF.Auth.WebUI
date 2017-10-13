using SF.Auth.DataAccess;
using SF.Common.DataAccess.Interface;

namespace SF.Common.DataAccess
{
    public class DbCustomerDatabaseFactory : IDbCustomerDatabaseFactory
    {
        #region Public Methods

        public dbCustomerDatabase CreateDbContext(string connectionString)
        {
            return new dbCustomerDatabase(connectionString);
        }

        #endregion Public Methods
    }
}