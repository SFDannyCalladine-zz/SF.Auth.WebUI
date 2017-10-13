using Microsoft.Extensions.Caching.Memory;
using SF.Common.Repositories.Cache.Interfaces;

namespace SF.Auth.Repositories.Cache
{
    public class MemoryCacheAdpater : ICacheStorage
    {
        #region Private Fields

        private readonly IMemoryCache _memoryCache;

        #endregion Private Fields

        #region Public Constructors

        public MemoryCacheAdpater(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        #endregion Public Constructors

        #region Public Methods

        public T Retrieve<T>(string key)
        {
            var itemStored = _memoryCache.Get<T>(key);

            if (itemStored == null)
            {
                itemStored = default(T);
            }

            return itemStored;
        }

        public void Store<T>(string key, T data)
        {
            _memoryCache.Set(key, data);
        }

        #endregion Public Methods
    }
}