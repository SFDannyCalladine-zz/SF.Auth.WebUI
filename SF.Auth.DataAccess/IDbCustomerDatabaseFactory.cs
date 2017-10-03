using SF.Auth.DataAccess;

namespace SF.Common.DataAccess.Interface
{
    public interface IDbCustomerDatabaseFactory
    {
        dbCustomerDatabase CreateDbContext(string connectionString);
    }
}