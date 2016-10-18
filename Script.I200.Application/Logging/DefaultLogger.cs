using System;
using System.Data.SqlClient;
using Script.I200.Core.Config;
using Script.I200.Core.Data;
using Script.I200.Data;
using Script.I200.Entity.Enum;
using Script.I200.Entity.Model.Logging;

namespace Script.I200.Application.Logging
{
    public class DefaultLogger:ILogger
    {
        private readonly IRepository<RequestLog> _requestlogRepository;
        private readonly IRepository<Log> _logRepository;
        private ILogger _nLogger;

        public DefaultLogger()
        {
            var dapperDbContext = new DapperDbContext(new SqlConnection(WebConfigSetting.Instance.WebAPILogDbConnection));

            _requestlogRepository = new DapperRepository<RequestLog>(dapperDbContext);
            _logRepository = new DapperRepository<Log>(dapperDbContext);
            _nLogger = new NLogger();
        }

        public void InsertLog(LogLevel logLevel, string shortMessage, string fullMessage = "", string requestLogId="")
        {

            try
            {
                _logRepository.Insert(new Log()
                {
                    ShortMessage = shortMessage,
                    LogLevel = logLevel,
                    RequestLogId = requestLogId,
                    FullMessage = fullMessage,
                    CreatedOnUtc = DateTime.UtcNow
                });

                _nLogger.InsertLog(LogLevel.Error, shortMessage, fullMessage, requestLogId);
            }
            catch (Exception ex)
            {
                _nLogger.InsertLog(LogLevel.Error, ex);
            }   
        }

        public void InsertRequestLog(RequestLog log)
        {
            try
            {
                _requestlogRepository.Insert(log);
                _nLogger.InsertRequestLog(log);
            }
            catch (Exception ex)
            {
                _nLogger.InsertLog(LogLevel.Error,ex);
            }
        }


        public void InsertLog(LogLevel logLevel, Exception exception, string message, params object[] args)
        {
            _nLogger.InsertLog(logLevel, exception,message, args);
        }


        public void InsertLog(LogLevel logLevel, Exception exception)
        {
            _nLogger.InsertLog(LogLevel.Error, exception);
        }
    }
}
