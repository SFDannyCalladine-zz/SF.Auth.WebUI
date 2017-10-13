namespace SF.Common.Repositories.Cache.Interfaces
{
    public interface ICacheStorage
    {
        T Retrieve<T>(string key);

        void Store<T>(string key, T data);
    }
}