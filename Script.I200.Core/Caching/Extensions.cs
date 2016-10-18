using System;

namespace Script.I200.Core.Caching
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class CacheExtensions
    {

        public static T Get<T>(this ICacheService cacheManager, string key, Func<T> acquire)
        {
            return Get(cacheManager, key, 60, acquire);
        }


        public static T Get<T>(this ICacheService cacheManager, string key, int cacheTime, Func<T> acquire)
        {
            if (cacheManager.HasKey(key))
            {
                return cacheManager.Get<T>(key);
            }

            var result = acquire();
            if (cacheTime > 0)
                cacheManager.Set(key, result, cacheTime);
            return result;
        }
    }
}
