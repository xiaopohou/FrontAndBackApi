using System;
using System.Collections.Generic;

namespace CommonLib
{
    public static class VoiceVerify
    {
        private static string ApiHost = "app.cloopen.com";                  //API Url地址
        private static string ApiPort = "8883";                                    //API 端口
        private static string Account = "aaf98f8949754cfb014978ff0ee50146";        //ACCOUNT SID
        private static string AccountToken = "a58f237bdeda409ab218550099776651";   //AUTH TOKEN
        private static string AppId = "8a48b55149754f8001497950ac0301a7";          //APP ID

        #region VoiceVerify 发送语音验证码
        /// <summary>
        /// 发送语音验证码
        /// </summary>
        /// <param name="verifyCode"></param>
        /// <param name="phoneNum"></param>
        /// <returns></returns>
        public static VoiceResult SendVoiceVerify(string verifyCode, string phoneNum)
        {
            var model = new VoiceResult();
            var ccpControl = new CCPRestSDK.CCPRestSDK();
            bool isInit = ccpControl.init(ApiHost, ApiPort);
            ccpControl.setAccount(Account, AccountToken);
            ccpControl.setAppId(AppId);

            model.Status = false;
            try
            {
                if (isInit)
                {
                    var oResult = ccpControl.VoiceVerify(phoneNum, verifyCode, "4006006815", "3", "http://app.i200.cn/API/VoiceVerify.ashx");
                    if (oResult.ContainsKey("statusCode"))
                    {
                        model.StatusCode = oResult["statusCode"].ToString().Trim();
                        if (oResult.ContainsKey("data"))
                        {
                            var oItem = (Dictionary<string,object>)oResult["data"];
                            if (oItem.ContainsKey("VoiceVerify"))
                            {
                                var oList = (Dictionary<string, object>)oItem["VoiceVerify"];
                                if (oList.ContainsKey("callSid"))
                                {
                                    model.CallSid = oList["callSid"].ToString().Trim();
                                }
                                if (oList.ContainsKey("dateCreated"))
                                {
                                    model.DateCreated = oList["dateCreated"].ToString().Trim();
                                }
                            }
                        }
                        model.Status = true;
                    }
                }
                else
                {
                    model.StatusCode = "-1";
                }
            }
            catch (Exception ex)
            {
                model.StatusCode = ex.ToString();
            }

            return model;
        }
        #endregion
    }

    public class VoiceResult
    {
        public bool Status { get; set; }
        public string StatusCode { get; set; }
        public string CallSid { get; set; }
        public string DateCreated { get; set; }
    }
}
