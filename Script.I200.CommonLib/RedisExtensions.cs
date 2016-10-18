//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.Text;
//using System.Threading.Tasks;
//using StackExchange.Redis;

//namespace CommonLib
//{
//    public class RedisExtensions
//    {
//        private static readonly string _redisHost = System.Configuration.ConfigurationManager.AppSettings["RedisHost"];
//        //private ConnectionMultiplexer connection;

//        //public RedisExtensions()
//        //{
//        //    // create a connection
//        //    var _redisHost = System.Configuration.ConfigurationManager.AppSettings["RedisHost"];

//        //    connection = ConnectionMultiplexer.Connect(_redisHost, Console.Out);
//        //}

//        #region --辅助方法--

//        private byte[] Serialize(object o)
//        {
//            if (o == null)
//            {
//                return null;
//            }

//            BinaryFormatter binaryFormatter = new BinaryFormatter();
//            using (MemoryStream memoryStream = new MemoryStream())
//            {
//                binaryFormatter.Serialize(memoryStream, o);
//                byte[] objectDataAsStream = memoryStream.ToArray();
//                return objectDataAsStream;
//            }
//        }

//        private T Deserialize<T>(byte[] stream)
//        {
//            if (stream == null)
//            {
//                return default(T);
//            }

//            BinaryFormatter binaryFormatter = new BinaryFormatter();
//            using (MemoryStream memoryStream = new MemoryStream(stream))
//            {
//                T result = (T) binaryFormatter.Deserialize(memoryStream);
//                return result;
//            }
//        }

//        #endregion

//        #region -- 公共 --

//        /// <summary>
//        /// 判断是否存在Key
//        /// </summary>
//        /// <param name="key"></param>
//        /// <returns></returns>
//        public bool ExistsKey(string key)
//        {
//            try
//            {
//                using (var connection = ConnectionMultiplexer.Connect(_redisHost))
//                {
//                }
//            }
//            catch (Exception ex)
//            {
//                Logger.Error("Redis_SetKey错误", ex);
//            }
//            IDatabase redis = connection.GetDatabase();
//            return redis.KeyExists(key.ToLower());
//        }

//        /// <summary>
//        /// 设置缓存过期
//        /// </summary>
//        /// <param name="key"></param>
//        /// <param name="datetime"></param>
//        public bool ExpireKey(string key, DateTime datetime)
//        {
//            IDatabase redis = connection.GetDatabase();
//            return redis.KeyExpire(key.ToLower(), datetime);

//        }

//        #endregion

//        #region -- Item --

//        /// <summary>
//        /// 设置单体
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="key"></param>
//        /// <param name="entity"></param>
//        /// <returns></returns>
//        public bool ItemSet<T>(string key, T entity)
//        {
//            IDatabase redis = connection.GetDatabase();
//            return redis.StringSet(key.ToLower(), Serialize(entity));
//        }

//        /// <summary>
//        /// 设置单体过期时间
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="key"></param>
//        /// <param name="entity"></param>
//        /// <param name="exDt"></param>
//        /// <returns></returns>
//        public bool ItemSet<T>(string key, T entity, TimeSpan exDt)
//        {
//            IDatabase redis = connection.GetDatabase();
//            return redis.StringSet(key.ToLower(), Serialize(entity), exDt);
//        }

//        /// <summary>
//        /// 增量
//        /// </summary>
//        /// <param name="key"></param>
//        /// <param name="count"></param>
//        /// <returns></returns>
//        public bool ItemSetIncrement(string key, int count)
//        {
//            IDatabase redis = connection.GetDatabase();
//            redis.StringIncrement(key.ToLower(), count);
//            return true;
//        }

//        /// <summary>
//        /// 获取单体
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="key"></param>
//        /// <returns></returns>
//        public T ItemGet<T>(string key)
//        {
//            IDatabase redis = connection.GetDatabase();

//            return Deserialize<T>(redis.StringGet(key.ToLower()));
//        }
//        /// <summary>
//        /// 增量值
//        /// </summary>
//        /// <param name="key"></param>
//        /// <returns></returns>
//        public int ItemGet(string key)
//        {
//            IDatabase redis = connection.GetDatabase();

//            return Deserialize<int>(redis.StringGet(key.ToLower()));
//        }

//        /// <summary>
//        /// 移除单体
//        /// </summary>
//        /// <param name="key"></param>
//        public bool ItemRemove(string key)
//        {
//            IDatabase redis = connection.GetDatabase();
//            return redis.KeyDelete(key.ToLower());
//        }

//        #endregion

//        #region -- List --

//        /// <summary>
//        /// 进队列，从右进（左头右尾）
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="key"></param>
//        /// <param name="entity"></param>
//        /// <returns></returns>
//        public bool ListAdd<T>(string key, T entity)
//        {
//            IDatabase redis = connection.GetDatabase();
//            return redis.ListRightPush(key.ToLower(), Serialize(entity)) > 0;
//        }

//        /// <summary>
//        /// 从队列删除一个节点
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="key"></param>
//        /// <param name="entity"></param>
//        /// <returns></returns>
//        public bool ListRemove<T>(string key, T entity)
//        {
//            IDatabase redis = connection.GetDatabase();
//            return redis.ListRemove(key.ToLower(), Serialize(entity)) > 0;
//        }

//        public long ListCount(string key)
//        {
//            IDatabase redis = connection.GetDatabase();
//            return redis.ListLength(key.ToLower());
//        }

//        public List<T> ListGetRange<T>(string key, long start, long count)
//        {
//            IDatabase redis = connection.GetDatabase();
//            var valueList = redis.ListRange(key.ToLower(), start, start + count - 1);

//            return valueList.Select(value => Deserialize<T>(value)).ToList();
//        }

//        public List<T> ListGetList<T>(string key, int pageIndex, int pageSize)
//        {
//            int start = pageSize * (pageIndex - 1);
//            return ListGetRange<T>(key.ToLower(), start, pageSize);
//        }

//        #endregion

//        #region -- Set --

//        public bool SetAdd<T>(string key, T entity)
//        {
//            IDatabase redis = connection.GetDatabase();
//            return redis.SetAdd(key.ToLower(), Serialize(entity));
//        }

//        public bool SetContains<T>(string key, T entity)
//        {
//            IDatabase redis = connection.GetDatabase();
//            return redis.SetContains(key.ToLower(), Serialize(entity));
//        }

//        public bool SetRemove<T>(string key, T entity)
//        {
//            IDatabase redis = connection.GetDatabase();
//            return redis.SetRemove(key.ToLower(), Serialize(entity));
//        }

//        #endregion

//        #region -- SortedSet --

//        /// <summary>
//        ///  添加数据到 SortedSet
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="key"></param>
//        /// <param name="entity"></param>
//        /// <param name="score"></param>
//        public bool SortedSetAdd<T>(string key, T entity, double score)
//        {
//            IDatabase redis = connection.GetDatabase();
//            return redis.SortedSetAdd(key.ToLower(), Serialize(entity), score);
//        }

//        /// <summary>
//        /// 移除数据从SortedSet
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="key"></param>
//        /// <param name="entity"></param>
//        /// <returns></returns>
//        public bool SortedSetRemove<T>(string key, T entity)
//        {
//            IDatabase redis = connection.GetDatabase();
//            return redis.SortedSetRemove(key.ToLower(), Serialize(entity));
//        }

//        #endregion

//        #region -- Hash --

//        /// <summary>
//        /// 判断某个数据是否已经被缓存
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="key"></param>
//        /// <param name="dataKey"></param>
//        /// <returns></returns>
//        public bool HashExist<T>(string key, string dataKey)
//        {
//            IDatabase redis = connection.GetDatabase();
//            return redis.HashExists(key.ToLower(), dataKey.ToLower());
//        }

//       /// <summary>
//        /// 存储数据到hash表
//       /// </summary>
//       /// <typeparam name="T"></typeparam>
//       /// <param name="key"></param>
//       /// <param name="dataKey"></param>
//       /// <param name="entity"></param>
//       /// <returns></returns>
//        public bool HashSet<T>(string key, string dataKey, T entity)
//        {
//            IDatabase redis = connection.GetDatabase();
//            return redis.HashSet(key.ToLower(), dataKey.ToLower(), Serialize(entity));
//        }

//        /// <summary>
//        /// 多属性添加数据，KeyValuePare<string,string>
//        /// </summary>
//        /// <param name="key"></param>
//        /// <param name="keyValuePairs"></param>
//        public void HashSet(string key, IEnumerable<KeyValuePair<string, string>> keyValuePairs)
//        {
//            List<HashEntry> hashEntries = keyValuePairs.Select(item => new HashEntry(item.Key.ToLower(), item.Value)).ToList();

//            IDatabase redis = connection.GetDatabase();
//            redis.HashSet(key.ToLower(), hashEntries.ToArray());
//        }

//        /// <summary>
//        /// 移除hash中的某值
//        /// </summary>
//        /// <param name="key"></param>
//        /// <param name="dataKey"></param>
//        /// <returns></returns>
//        public bool HashRemove(string key, string dataKey)
//        {
//            IDatabase redis = connection.GetDatabase();
//            return redis.HashDelete(key.ToLower(), dataKey.ToLower());
//        }

//        /// <summary>
//        /// 从hash表获取数据
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="key"></param>
//        /// <param name="dataKey"></param>
//        /// <returns></returns>
//        public T HashGet<T>(string key, string dataKey)
//        {
//            IDatabase redis = connection.GetDatabase();
//            return Deserialize<T>(redis.HashGet(key.ToLower(), dataKey.ToLower()));
//        }

//        /// <summary>
//        /// 从hash表获取数据（部分）
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="key"></param>
//        /// <param name="dataKey"></param>
//        /// <returns></returns>
//        public List<T> HashGet<T>(string key, string[] dataKey)
//        {
//            string[] hashFileds = dataKey.Select(hf => hf.ToLower()).ToArray();

//            IDatabase redis = connection.GetDatabase();

//            //redis.HashGet(key.ToLower(), hashFileds);

//            return null;
//        }

//        /// <summary>
//        /// 从hash表获取数据（全部）
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="key"></param>
//        /// <returns></returns>
//        public List<T> HashGet<T>(string key)
//        {
//            IDatabase redis = connection.GetDatabase();

//            var hashValues = redis.HashValues(key.ToLower());

//            return hashValues.Select(v => Deserialize<T>(v)).ToList();
//        }

//        #endregion

//    }
//}
