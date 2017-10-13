using SF.Common.Repositories.Cache.Interfaces;

namespace SF.Auth.Repositories.Cache
{
    public abstract class CacheRepository
    {
        protected readonly ICacheStorage Cache;

        protected CacheRepository(ICacheStorage cache)
        {
            Cache = cache;
        }
    }
}