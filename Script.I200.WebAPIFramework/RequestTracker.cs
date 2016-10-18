using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Script.I200.Application.Logging;
using Script.I200.Entity.Model.Logging;

namespace Script.I200.WebAPIFramework
{
    public class RequestTracker
    {
        private RequestLog requestLog;
        private Stopwatch stopwatch;
        private ILogger _iLogger = new NLogger();

        public RequestTracker(RequestLog info)
        {
            this.requestLog = info;
        }


        public void ProcessActionStart()
        {
            this.stopwatch = Stopwatch.StartNew();
        }


        public void ProcessActionComplete(bool exceptionThrown, int statusCode)
        {
            try
            {
                this.stopwatch.Stop();
                requestLog.ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                requestLog.StatusCode = statusCode;
                requestLog.Exception = exceptionThrown;

                Task.Run(() =>
                {
                    _iLogger.InsertRequestLog(requestLog);
                });
            }
            catch (Exception ex)
            {
                String message = String.Format("Exception {0} occurred PerformanceTracker.ProcessActionComplete().  Message {1}\nStackTrace {0}",
                    ex.GetType().FullName, ex.Message, ex.StackTrace);
                Trace.WriteLine(message);
            }
        }

    }
}
