//using System;
//using StackExchange.Redis;

//namespace CommonLib
//{
//    //public class RedisHelper
//    //{
//    //    private static string _redisHost = System.Configuration.ConfigurationManager.AppSettings["RedisHost"];
//    //    //private static string _preName = "I200Core:";
//    //    public enum RedisEnumType
//    //    {
//    //        /// <summary>
//    //        /// 微信分享
//    //        /// </summary>
//    //        WeiXinShare = 1,
//    //        /// <summary>
//    //        /// 转介绍
//    //        /// </summary>
//    //        InviteRebate = 2,
//    //        /// <summary>
//    //        /// 高级版试用版
//    //        /// </summary>
//    //        BetaAdvance=3,
//    //        /// <summary>
//    //        /// 新商城改版
//    //        /// </summary>
//    //        NewSaasNotice=4
//    //    }
//    //    public static string GetPreName(RedisEnumType type)
//    //    {
//    //        string sResult = "";
//    //        switch (type)
//    //        {
//    //            case RedisEnumType.WeiXinShare:
//    //                sResult = "I200Core:";
//    //                break;
//    //            case RedisEnumType.InviteRebate:
//    //                sResult = "I200InviteRebate:";
//    //                break;
//    //            case RedisEnumType.BetaAdvance:
//    //                sResult = "I200BetaAdvance:";
//    //                break;
//    //            case RedisEnumType.NewSaasNotice:
//    //                sResult = "I200NewSaasNotice:";
//    //                break;
//    //        }
//    //        return sResult;
//    //    }


//    //    /// <summary>
//    //    /// 设置String缓存信息
//    //    /// </summary>
//    //    /// <param name="key"></param>
//    //    /// <param name="value"></param>
//    //    /// <param name="type">枚举类别</param>
//    //    /// <param name="expireSeconds"></param>
//    //    public static void SetKey(string key, string value, RedisEnumType type, int expireSeconds = 0)
//    //    {
//    //        try
//    //        {
//    //            using (var connection = ConnectionMultiplexer.Connect(_redisHost))
//    //            {
//    //                string _preName = GetPreName(type);
//    //                IDatabase redis = connection.GetDatabase();

//    //                if (expireSeconds > 0)
//    //                {
//    //                    TimeSpan ts = new TimeSpan(0, 0, expireSeconds);
//    //                    redis.StringSet(_preName + key, value, ts);
//    //                }
//    //                else
//    //                {
//    //                    redis.StringSet(_preName + key, value);
//    //                }
//    //            }
//    //        }
//    //        catch (Exception ex)
//    //        {
//    //            //Logger.Error("Redis_SetKey错误", ex);
//    //        }
//    //    }

//    //    /// <summary>
//    //    /// 获取String缓存信息
//    //    /// </summary>
//    //    /// <param name="key"></param>
//    //    /// <param name="type">类别</param>
//    //    /// <returns></returns>
//    //    public static string GetKey(string key, RedisEnumType type)
//    //    {
//    //        string strResult = "";
//    //        try
//    //        {
//    //            string _preName = GetPreName(type);
//    //            using (var connection = ConnectionMultiplexer.Connect(_redisHost))
//    //            {
//    //                IDatabase redis = connection.GetDatabase();
//    //                strResult = redis.StringGet(_preName + key);
//    //            }
//    //        }
//    //        catch (Exception ex)
//    //        {
//    //            Logger.Error("Redis_GetKey错误", ex);
//    //        }

//    //        return strResult;
//    //    }

//    //}
//}
