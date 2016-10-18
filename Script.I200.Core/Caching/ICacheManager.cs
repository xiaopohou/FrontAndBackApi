using System;

namespace Script.I200.Core.Caching
{
    /// <summary>
    /// Cache manager interface
    /// </summary>
    public interface ICacheManager : IDisposable
    {
      
        T Get<T>(string key);

        void Set(string key, object data, int cacheTime);

      
        bool IsSet(string key);

     
        void Remove(string key);

     
        void RemoveByPattern(string pattern);

    
        void Clear();
    }
}
