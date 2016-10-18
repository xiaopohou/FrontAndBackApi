using System;
using Script.I200.Entity.Enum;
using Script.I200.Entity.Model.Logging;

namespace Script.I200.Application.Logging
{
    public partial interface ILogger
    {
        void InsertLog(LogLevel logLevel, string shortMessage, string fullMessage = "" ,string requestLogId="");

        void InsertLog(LogLevel logLevel, Exception exception, string message, params  object[] args);

        void InsertLog(LogLevel logLevel, Exception exception);

        void InsertRequestLog(RequestLog log);
    }
}
