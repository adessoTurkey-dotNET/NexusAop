using System;

namespace NexusAop.Cache
{
    public interface INexusAopCacheService
    {
        void CacheResult(
            string key,
            object value,
            TimeSpan? ttl = null);

        object GetResult(
            string key);
    }
}