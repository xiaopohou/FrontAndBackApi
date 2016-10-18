using Script.I200.Entity.Enum;

namespace Script.I200.Entity.Dto.Settings
{
    public class LoggerSettings
    {
        /// <summary>
        /// 日志数据库连接字符串
        /// </summary>
        public string LogConnectionString { get; set; }

        /// <summary>
        /// 日志级别
        /// </summary>
        public LogLevel LogLevel { get; set; }  
    }
}
