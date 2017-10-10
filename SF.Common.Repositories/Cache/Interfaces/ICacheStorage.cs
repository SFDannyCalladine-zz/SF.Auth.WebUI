namespace SF.Common.Repositories.Cache.Interfaces
{
    public interface ICacheStorage
    {
        #region Public Methods

        T Retrieve<T>(string key);

        void Store<T>(string key, T data);

        #endregion Public Methods
    }
}
