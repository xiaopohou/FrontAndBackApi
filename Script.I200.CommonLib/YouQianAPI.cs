using System.Collections.Generic;
using System.Configuration;

namespace CommonLib
{
    /// <summary>
    /// 有钱API
    /// </summary>
    public class YouQianAPI
    {
        //Test
        //string YouQianAPIHost = "http://" + System.Configuration.ConfigurationManager.AppSettings["youQianApi"] + "/external/callback/1000253/20118/";

        //Official
        string YouQianAPIHost = "http://" + ConfigurationManager.AppSettings["youQianApi"] + "/external/callback/1000271/10416/";
        
        /// <summary>
        /// 验证信息
        /// </summary>
        /// <param name="machineCode"></param>
        /// <returns></returns>
        public string Verification(string machineCode)
        {
            string returnJson = "";
            if (machineCode.Length > 0)
            {
                string url = YouQianAPIHost + "?did=" + machineCode + "&ret_type=1";
                returnJson = HttpUtility.HttpGet(url);
            }
            else
            {
                returnJson = "{\"message\":  \"机器码不存在\",  \"success\": false}";
            }
            return returnJson;
        }

        /// <summary>
        /// 格式化数据
        /// </summary>
        /// <param name="regturnJson"></param>
        /// <returns></returns>
        public bool FormatReturnJson(string returnJson)
        {
            Dictionary<string, object> returnModel = new Dictionary<string, object>();

            returnModel = Helper.JsonDeserializeObject<Dictionary<string, object>>(returnJson);
            return (bool)returnModel["success"];
        }

    }
}
