using System.Collections.Generic;
using Lucene.Net.Store;

namespace CommonLib.UserSearch
{
    public class RamDirectoryFactory
    {
        private static object _initLock = new object();
        private static Dictionary<string, RAMDirectory> _ramDirectoryCache = new Dictionary<string, RAMDirectory>(); 

        /// <summary>
        /// 获取内存索引目录
        /// </summary>
        public static RAMDirectory GetRamDirectory(string path, FSDirectory directory)
        {
            if (_ramDirectoryCache.ContainsKey(path))
            {
                return _ramDirectoryCache[path];
            }

            lock (_initLock)
            { 
                var ramDirectory = new RAMDirectory(directory);
                _ramDirectoryCache.Add(path, ramDirectory);

                return ramDirectory;
            }
        }
    }
}
