using SF.Common.Repositories.Cache.Interfaces;

namespace SF.Auth.Repositories.Cache
{
    public abstract class CacheRepository
    {
        #region Protected Fields

        protected readonly ICacheStorage Cache;

        #endregion Protected Fields

        #region Protected Constructors

        protected CacheRepository(ICacheStorage cache)
        {
            Cache = cache;
        }

        #endregion Protected Constructors
    }
}