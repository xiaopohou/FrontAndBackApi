using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Text;
using log4net;

namespace CommonLib
{
    /// <summary>
    /// MongoDBAPI
    /// </summary>
    public class MongoDBAPI
    {
        string MongoDBAPIHost = "http://" + ConfigurationManager.AppSettings["MongoDBAPI"] + "/api/";

        public void RunTime(string sql, long time)
        {
            Dictionary<string, string> formData = new Dictionary<string, string>();
            formData["SqlStr"] = sql;
            formData["RunTime"] = time.ToString();
            formData["SourceType"] = "app.i200.cn";
            try
            {
                var retJSON = HttpUtility.HttpPost(MongoDBAPIHost + "sqlruntime", GetSignature(), formData);
            }
            catch (Exception)
            {
              
            }
        }

        /// <summary>
        /// 记录页面浏览信息
        /// </summary>
        /// <param name="userName">当前登录账号</param>
        /// <param name="SourcePage">来源页面</param>
        /// <param name="SourceIP">来源IP</param>
        /// <param name="UserAgent">浏览器信息</param>
        /// <param name="PreviousSource">上一页面</param>
        /// <param name="OtherJson">其他数据</param>
        /// <param name="Key">一个标示，无特别用处</param>
        public void HtmlBrowse(string userName,string SourcePage, string SourceIP, string UserAgent, string PreviousSource, string OtherJson,string Key)
        {
            var objData = new Dictionary<string, string>();
            objData["SourcePage"] = SourcePage;
            objData["SourceIP"] = SourceIP;
            objData["UserAgent"] = UserAgent;
            objData["PreviousSource"] = PreviousSource;
            objData["OtherJson"] = OtherJson;
            objData["HostHeader"] = "app.i200.cn";
            objData["ViewTime"] = Helper.GetTimeStamp();
            objData["Key"] = Key;
            objData["userName"] = userName;


            try
            {
               var json= HttpUtility.HttpPost(MongoDBAPIHost + "HtmlBrowse", GetSignature(), objData);
            }
            catch (Exception)
            {

            }
        }


        /// <summary>
        /// 用户来源记录
        /// </summary>
        /// <param name="accId">店铺Id</param>
        /// <param name="accName">店铺名称</param>
        /// <param name="source">来源类型</param>
        /// <param name="keyWord">关键字</param>
        public void RegSource(int accId, string accName, string source, string keyWord)
        {
            var objData = new Dictionary<string, string>();
            objData["AccId"] = accId.ToString();
            objData["AccName"] = accName;
            objData["Source"] = source;
            objData["Keyword"] = keyWord;

            try
            {
                if (source != "")
                {
                    var retJSON = HttpUtility.HttpPost(MongoDBAPIHost + "sourcerecord", GetSignature(), objData);
                }
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 按钮点击日志
        /// </summary>
        /// <param name="ButtonKey">按钮的KEY</param>
        /// <param name="ButtonName">按钮名称说明</param>
        /// <param name="SourceIP">来源IP</param>
        /// <param name="SourcePage">来源页面</param>
        /// <param name="UserAgent">浏览器信息</param>
        /// <param name="Accid">店铺</param>
        /// <param name="OtherJson">其他数据</param>
        public string ButtonClickLog(int ButtonKey, string ButtonName, string SourceIP, string SourcePage, string UserAgent, int Accid=0, string OtherJson = "")
        {
            var objData = new Dictionary<string, string>();
            objData["ButtonKey"] = ButtonKey.ToString() ;
            objData["ButtonName"] = ButtonName;
            objData["Accid"] = Accid.ToString();
            objData["SourceIP"] = SourceIP;
            objData["SourcePage"] = SourcePage;
            objData["UserAgent"] = UserAgent;
            objData["OtherJson"] = OtherJson;


            try
            {
                var json = HttpUtility.HttpPost(MongoDBAPIHost + "ButtonCount", GetSignature(), objData);
                return json.ToString();
            }
            catch (Exception ex)
            {
                Error("按钮点击日志错误", ex);
                return ex.ToString();
            }
        }

        private void Error(object message, Exception ex)
        {
             ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
             log.Error(message,ex);
        }

        public Dictionary<string, string> GetSignature()
        {
            Random ro = new Random();


            string strTimestamp = Helper.GetTimeStamp();
            string strNonce = ro.Next(4000, 9000).ToString();
            string strAppKey = "XKCE9P34TsqemfITS0W18RX6ewsxPK07MALZJ7Y";

            StringBuilder strSign = new StringBuilder();
            strSign.Append(strAppKey);
            strSign.Append(strTimestamp);
            strSign.Append(strNonce);



            string strAuthCode = Helper.Md5Hash(strSign.ToString());

            Dictionary<string, string> value = new Dictionary<string, string>();
            value["Signature"] = strAuthCode;
            value["Timestamp"] = strTimestamp;
            value["Nonce"] = strNonce;
            return value;
        }

    }

}
