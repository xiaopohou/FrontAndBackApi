using System.Collections.Generic;
using Script.I200.Core.Caching;

namespace Script.I200.Core.Config
{
    public class RedisRemoteConfig : IRemoteSetting
    {
        private ICacheManager _localCache = new MemoryCacheManager();
        private const int _configLastMinutes = 7*24*60;

        public string GetConfig(string key)
        {
            if (_localCache.IsSet(key))
                return _localCache.Get<string>(key);

            var redisCacheService = new RedisCacheService();
            if (!redisCacheService.HasKey(key))
                throw new YuanbeiException("config key {0} not existed in redis", key);

            string val = redisCacheService.Get<string>(key);
            _localCache.Set(key, val, _configLastMinutes);
            return val;

        }

        /// <summary>
        /// 更新远程redis缓存到本地
        /// </summary>
        /// <param name="key"></param>
        public void UpdateLocalConfig(string key)
        {
            var redisCacheService = new RedisCacheService();
            if (!redisCacheService.HasKey(key))
                throw new YuanbeiException("config key {0} not existed in redis", key);

            string val = redisCacheService.Get<string>(key);
            _localCache.Set(key, val, _configLastMinutes);

        }

        public void UpdateLocalConfig(IEnumerable<string> keys)
        {
            foreach (var key in keys)
            {
                UpdateLocalConfig(key);
            }
        }

    }
}
