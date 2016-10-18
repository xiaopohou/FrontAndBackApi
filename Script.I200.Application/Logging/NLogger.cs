using System;
using NLog;
using Script.I200.Entity.Model.Logging;

namespace Script.I200.Application.Logging
{
    public class NLogger:ILogger
    {
        private readonly Logger _logger;

        public NLogger()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }


        private LogLevel GetNLoggerLever(Entity.Enum.LogLevel level)
        {
            switch (level)
            {
                case Entity.Enum.LogLevel.Information:
                    return LogLevel.Info;
                case Entity.Enum.LogLevel.Error:
                    return LogLevel.Error;
                case Entity.Enum.LogLevel.Debug:
                    return LogLevel.Debug;
                case Entity.Enum.LogLevel.Fatal:
                    return LogLevel.Fatal;
                case Entity.Enum.LogLevel.Off:
                    return LogLevel.Off;
                case Entity.Enum.LogLevel.Warning:
                    return LogLevel.Warn;
                default:
                    return LogLevel.Trace;

            }

        }

        public void InsertLog(Entity.Enum.LogLevel logLevel, string shortMessage, string fullMessage = "", string requestLogId = "")
        {
            try
            {
                _logger.Log( GetNLoggerLever(logLevel), shortMessage);   
            }
            catch (Exception ex)
            {
                // 记日志出错不能再抛出
                
            }
        }

        public void InsertRequestLog(RequestLog log)
        {
            try
            {
                _logger.Log(LogLevel.Info, log.ToFormatString());
            }
            catch (Exception)
            {
               
            }
            
        }

        public void InsertLog(Entity.Enum.LogLevel logLevel, Exception exception, string message, params object[] args)
        {
            try
            {
                _logger.Log(GetNLoggerLever(logLevel), exception, message, args);
            }
            catch (Exception)
            {

            }
            
        }

        public void InsertLog(Entity.Enum.LogLevel logLevel, Exception exception)
        {
            try
            {
                _logger.Log(GetNLoggerLever(logLevel) , exception);
            }
            catch (Exception)
            {

            }
            
        }
    }
}
