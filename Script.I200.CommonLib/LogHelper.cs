using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web;
using LogService;

namespace CommonLib
{
    public static class LogHelper
    {
        /// <summary>
        /// 网页访问日志
        /// </summary>
        /// <param name="logModel"></param>
        public static void WebLog(WebLogModel webLogModel)
        {
            try
            {
                var logger = new LogCenter();
                var logModel = new WebModel();
                logModel.AccId = webLogModel.AccId;
                logModel.SubName = webLogModel.SubName;
                logModel.ActDate = webLogModel.ActDate;
                logModel.BrowserName = webLogModel.BrowserName;
                logModel.BrowserVer = webLogModel.BrowserVer;
                logModel.FromIp = webLogModel.FromIp;
                logModel.FromUrl = webLogModel.FromUrl;
                logModel.TargetName = webLogModel.TargetName;
                logModel.UserAgent = webLogModel.UserAgent;
                logModel.FromName = webLogModel.Message;
                logModel.ClientType=webLogModel.Flag;

                logger.WebLog(logModel);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 网页访问日志(异步)
        /// </summary>
        /// <param name="model"></param>
        public static void AsyncLog(WebLogModel model)
        {
            try
            {
                Task.Run(() =>
                {
                    WebLog(model);
                });
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 网页访问日志(异步)
        /// </summary>
        /// <param name="subName"></param>
        /// <param name="message"></param>
        /// <param name="accId"></param>
        /// <param name="flag"></param>
        /// <param name="oHttp"></param>
        public static void AsyncLogEx(string subName, string message, int accId, string flag, HttpRequest oHttp = null)
        {
            try
            {
                var model = new WebLogModel();
                model.Message = message;
                model.TargetName = message;
                model.SubName = subName;
                model.AccId = accId;
                model.Flag = flag;
                if (oHttp != null)
                {
                    model.ActDate = DateTime.Now;
                    model.TargetName = "";
                    model.UserAgent = oHttp.UserAgent;
                    if (oHttp.Browser != null)
                    {
                        model.BrowserName = oHttp.Browser.Browser;
                        model.BrowserVer = oHttp.Browser.Version;
                    }
                    if (oHttp.UserHostAddress != null)
                    {
                        model.FromIp = oHttp.UserHostAddress;
                    }
                    if (oHttp.UrlReferrer != null)
                    {
                        model.FromUrl = oHttp.UrlReferrer.ToString();
                    }
                }
                AsyncLog(model);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 错误日志记录(异步)
        /// </summary>
        /// <param name="model"></param>
        public static void AsyncErrorLog(ErrorModel model)
        {
            try
            {
                Task.Run(() =>
                {
                    try
                    {
                        var logger = new LogCenter();
                        var errModel = new LogService.ErrorModel();
                        errModel.Event = "ErrorLog";
                        errModel.SubName = model.SubName;
                        errModel.message = model.message;
                        errModel.ErrorMsg = model.ErrorMsg;
                        errModel.StackTrace = model.StackTrace;
                        errModel.TargetName = model.SubName;
                        errModel.SourceName = model.SourceName;
                        errModel.ErrorUrl = model.ErrorUrl;
                        errModel.RequestFrom = model.RequestFrom;
                        errModel.Ip = model.Ip;
                        errModel.BrowserName = model.BrowserName;
                        errModel.BrowserVer = model.BrowserVer;
                        errModel.UserAgent = model.UserAgent;
                        errModel.AccId = model.AccId;
                        errModel.Flag = model.Flag;

                        logger.Error(errModel);
                    }
                    catch (Exception)
                    {
                        
                    }
                });
            }
            catch (Exception)
            {

            }
        }

        public static string GetRequestFrom(NameValueCollection from)
        {
            string sResult = "";
            if (from.Count > 0)
            {
                var fromList = new List<string>();
                foreach (var item in from)
                {
                    fromList.Add(item.ToString());
                }
                sResult = Helper.JsonSerializeObject(fromList);
            }
            return sResult;
        }
    }

    public class WebLogModel
    {
        public string SubName { get; set; }
        public string Message { get; set; }
        public DateTime ActDate { get; set; }
        public string BrowserName { get; set; }
        public string BrowserVer { get; set; }
        public string FromIp { get; set; }
        public string FromUrl { get; set; }
        public string TargetName { get; set; }
        public string UserAgent { get; set; }
        public int AccId { get; set; }
        public int UserId { get; set; }
        public string Flag { get; set; }
    }

    public class ErrorModel
    {
        public string SubName { get; set; }
        public string message { get; set; }
        public string ErrorMsg { get; set; }
        public string StackTrace { get; set; }
        public string TargetName { get; set; }
        public string SourceName { get; set; }
        public string ErrorUrl { get; set; }
        public string RequestFrom { get; set; }
        public string Ip { get; set; }
        public string BrowserName { get; set; }
        public string BrowserVer { get; set; }
        public string UserAgent { get; set; }
        public int AccId { get; set; }
        public string Level { get; set; }
        public string Flag { get; set; }
    }
}
