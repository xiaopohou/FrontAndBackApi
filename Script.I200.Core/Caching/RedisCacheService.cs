using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Script.I200.Core.Caching
{
    public class RedisCacheService : ICacheService
    {
        private static readonly string RedisHost = ConfigurationManager.AppSettings["RedisHost"];
        private static ConnectionMultiplexer _connection;
        private static readonly object LockObject = new object();

        public RedisCacheService()
        {
            if (_connection != null && _connection.IsConnected) return;

            lock (LockObject)
            {
                ConfigurationOptions config = new ConfigurationOptions()
                {
                    AbortOnConnectFail = false,
                    ConnectRetry = 10,
                    ConnectTimeout = 5000,
                    SyncTimeout = 5000,
                    EndPoints = { { RedisHost } },
                    AllowAdmin = true,
                    KeepAlive = 180
                };
                _connection = ConnectionMultiplexer.Connect(config);
            }
        }

        /// <summary>
        /// 移除指定Key
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public bool RemoveKey(string key)
        {
            if (_connection != null && _connection.IsConnected)
            {
                return _connection.GetDatabase().KeyDelete(key);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 设置key过期时间
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="secondTimeout">过期秒数</param>
        /// <returns></returns>
        public bool KeyExpire(string key, int secondTimeout)
        {
            if (_connection != null && _connection.IsConnected)
            {
                return _connection.GetDatabase().KeyExpire(key, TimeSpan.FromSeconds(secondTimeout));
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断是否存在key
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public bool HasKey(string key)
        {
            if (_connection != null && _connection.IsConnected)
            {
                return _connection.GetDatabase().KeyExists(key);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 存储数据到key
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool Set(string key, string value)
        {
            if (_connection != null && _connection.IsConnected)
            {
                return _connection.GetDatabase().StringSet(key, value);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 存储数据到key
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="secondTimeout">过期秒数</param>
        /// <returns></returns>
        public bool Set(string key, string value, int secondTimeout)
        {
            if (_connection != null && _connection.IsConnected)
            {
                return _connection.GetDatabase().StringSet(key, value, TimeSpan.FromSeconds(secondTimeout));
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取指定key数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            if (_connection != null && _connection.IsConnected)
            {
                return _connection.GetDatabase().StringGet(key);
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 存储数据到key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Set<T>(string key, T value)
        {
            if (_connection != null && _connection.IsConnected)
            {
                var stringObj = JsonSerialize(value);
                return _connection.GetDatabase().StringSet(key, stringObj);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 存储数据到key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="secondTimeout">过期秒数</param>
        /// <returns></returns>
        public bool Set<T>(string key, T value, int secondTimeout)
        {
            if (_connection != null && _connection.IsConnected)
            {
                var stringObj = JsonSerialize(value);
                return _connection.GetDatabase().StringSet(key, stringObj, TimeSpan.FromSeconds(secondTimeout));
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取指定key数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            if (_connection != null && _connection.IsConnected)
            {
                var stringObj = _connection.GetDatabase().StringGet(key);
                if (!stringObj.IsNullOrEmpty)
                {
                    return JsonDeserialize<T>(stringObj);
                }
            }

            return default(T);
        }

        /// <summary>
        /// 获取指定key数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<T> Get<T>(string[] keys)
        {
            if (_connection != null && _connection.IsConnected)
            {

                var stringObj = Array.ConvertAll(_connection
                .GetDatabase()
                .StringGet(Array.ConvertAll(keys, item => (RedisKey)item)), item => (string)item); ;
                if (stringObj.Length > 0)
                {
                    foreach (var s in stringObj.Where(s => s != null))
                    {
                        yield return JsonDeserialize<T>(s);
                    }
                }
            }
        }

        /// <summary>
        /// 指定key原子加1操作
        /// </summary>
        /// <param name="key">递增之后的value</param>
        /// <returns></returns>
        public long StringIncrement(string key)
        {
            if (_connection != null && _connection.IsConnected)
            {
                return _connection.GetDatabase().StringIncrement(key);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 指定key原子减1操作
        /// </summary>
        /// <param name="key"></param>
        /// <returns>减小之后的value</returns>
        public long StringDecrement(string key)
        {
            if (_connection != null && _connection.IsConnected)
            {
                return _connection.GetDatabase().StringDecrement(key);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 集合中添加元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否成功加入</returns>
        public bool SetAdd(string key, string value)
        {
            if (_connection != null && _connection.IsConnected)
            {
                return _connection.GetDatabase().SetAdd(key, value);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 集合中添加元素组
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns>是否成功加入</returns>
        public long SetAdd(string key, string[] values)
        {
            if (_connection != null && _connection.IsConnected)
            {
                return _connection.GetDatabase().SetAdd(key, Array.ConvertAll(values, item => (RedisValue)item));
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 集合中是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetContains(string key, string value)
        {
            if (_connection != null && _connection.IsConnected)
            {
                return _connection.GetDatabase().SetContains(key, value);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 集合中移除
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetRemove(string key, string value)
        {
            if (_connection != null && _connection.IsConnected)
            {
                return _connection.GetDatabase().SetRemove(key, value);
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 集合成员数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long SetLength(string key)
        {
            if (_connection != null && _connection.IsConnected)
            {
                return _connection.GetDatabase().SetLength(key);
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 集合所有成员
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string[] SetMembers(string key)
        {
            if (_connection != null && _connection.IsConnected)
            {
                return Array.ConvertAll(_connection.GetDatabase().SetMembers(key), item => (string)item);
            }
            else
            {
                return new string[0];
            }
        }


        /// <summary>
        /// 序列化对象为Json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string JsonSerialize(object obj)
        {
            var formatSetting = new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Local,
                DateFormatString = "yyyy-MM-dd HH:mm:ss",
                Formatting = Formatting.Indented
            };
            var jSonStr = JsonConvert.SerializeObject(obj, formatSetting);

            return jSonStr;
        }

        /// <summary>
        /// 反序列化为T对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strJson"></param>
        /// <returns></returns>
        private T JsonDeserialize<T>(string strJson)
        {
            return JsonConvert.DeserializeObject<T>(strJson);
        }


        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
            }
        }
    }
}
