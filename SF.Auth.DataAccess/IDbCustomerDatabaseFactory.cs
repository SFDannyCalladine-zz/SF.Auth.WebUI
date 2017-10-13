using SF.Auth.DataAccess;

namespace SF.Common.DataAccess.Interface
{
    public interface IDbCustomerDatabaseFactory
    {
        #region Public Methods

        dbCustomerDatabase CreateDbContext(string connectionString);

        #endregion Public Methods
    }
}