using System.Configuration;

namespace Script.I200.Core.Config
{
    public sealed class WebConfigSetting
    {
        private static WebConfigSetting instance = null;
        private static readonly object padlock = new object();

        public static WebConfigSetting Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = InitConfiguration();
                        }
                    }
                }
                return instance;
            }
        }

        private static WebConfigSetting InitConfiguration()
        {
            return new WebConfigSetting()
            {
                RedisHost = ConfigurationManager.AppSettings["RedisHost"],
                I200DbConnectionString = ConfigurationManager.ConnectionStrings["DataBase"] == null ? string.Empty : ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString,
                I200SysDbConnectionString = ConfigurationManager.ConnectionStrings["DataBase"] == null ? string.Empty : ConfigurationManager.ConnectionStrings["ManageDataBase"].ConnectionString,
                I200StationDbConnectionString = ConfigurationManager.ConnectionStrings["DataBaseStation"] == null ? string.Empty : ConfigurationManager.ConnectionStrings["DataBaseStation"].ConnectionString,
                WebAPILogDbConnection = ConfigurationManager.ConnectionStrings["WebApiPerformance.ConnectionString"] == null ? string.Empty : ConfigurationManager.ConnectionStrings["WebApiPerformance.ConnectionString"].ConnectionString,
                PadAppKey = "iPadMaO8VUvVH0eBss",
                PhoneAppKey = "iPhoneHT5I0O4HDN65",
                AndroidAppKey = "AndroidYnHWyROQosO",
                WebAppKey = "WeblTbwyOOExn58AsE",
                ImageServer = ConfigurationManager.AppSettings["ImageServer"],
                AdvertTransferUrl = ConfigurationManager.AppSettings["AdvertTransferUrl"],
            };
        }

        /// <summary>
        /// Redis缓存服务器地址
        /// </summary>
        public string RedisHost { get; set; }

        /// <summary>
        /// 主站数据库记录
        /// </summary>
        public string I200DbConnectionString { get; set; }

        /// <summary>
        /// 运营后台数据库记录
        /// </summary>
        public string I200SysDbConnectionString { get; set; }

        /// <summary>
        /// 站内管理数据库记录
        /// </summary>
        public string I200StationDbConnectionString { get; set; } 

        /// <summary>
        /// API 日志记录数据库
        /// </summary>
        public string WebAPILogDbConnection { get; set; }

        /// <summary>
        /// iPad请求验证的AppKey
        /// </summary>
        public string PadAppKey { get; set; }

        /// <summary>
        /// iPhone请求验证的AppKey
        /// </summary>
        public string PhoneAppKey { get; set; }

        /// <summary>
        /// Android请求验证的AppKey
        /// </summary>
        public string AndroidAppKey { get; set; }

        /// <summary>
        /// Web端请求验证的AppKey
        /// </summary>
        public string WebAppKey { get; set; }

        /// <summary>
        /// 又拍云图片存储服务器地址
        /// </summary>
        public string ImageServer { get; set; }

        /// <summary>
        /// 广告链接中转页
        /// </summary>
        public string AdvertTransferUrl { get; set; }


    }
}