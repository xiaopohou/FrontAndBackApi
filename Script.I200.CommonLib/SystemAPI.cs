using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace CommonLib
{
    /// <summary>
    /// 系统API
    /// </summary>
    public class SystemAPI
    {


       // string apiHost = ;
        static string apiHost = "http://" + ConfigurationManager.AppSettings["SYSAPI"] + "/api/";

        /// <summary>
        /// 新增加店铺推送信息
        /// </summary>
        /// <param name="accid"></param>
        /// <returns></returns>
        public int NewAccountPushMessage(int accid)
        {
            try
            {
                Dictionary<string, object> postData = new Dictionary<string, object>();

                var objData = new Dictionary<string, string>();
                objData["accId"] = accid.ToString();
                var retJSON = HttpUtility.HttpPost(apiHost + "msgcenter?template=newreg", ApiSignature(), objData);
                ResponseModel rM = Helper.JsonDeserializeObject<ResponseModel>(retJSON);
                if (rM.Status == 0 && rM.ErrCode == 0)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        /// <summary>
        /// 新年活动
        /// </summary>
        /// <param name="accid"></param>
        /// <returns></returns>
        public int NewYearOEMPushMessage(int accid)
        {
            try
            {
                Dictionary<string, object> postData = new Dictionary<string, object>();

                var objData = new Dictionary<string, string>();
                objData["accId"] = accid.ToString();
                var retJSON = HttpUtility.HttpPost(apiHost + "msgcenter?template=newyearoem", ApiSignature(), objData);
                ResponseModel rM = Helper.JsonDeserializeObject<ResponseModel>(retJSON);
                if (rM.Status == 0 && rM.ErrCode == 0)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {

                return -1;
            }
        }

        /// <summary>
        /// 春节奖状
        /// </summary>
        /// <param name="accid"></param>
        /// <returns></returns>
        public int NewYearAwardPushMessage(int accid)
        {
            try
            {
                Dictionary<string, object> postData = new Dictionary<string, object>();

                var objData = new Dictionary<string, string>();
                objData["accId"] = accid.ToString();
                var retJSON = HttpUtility.HttpPost(apiHost + "msgcenter?template=newyearaward", ApiSignature(), objData);
                ResponseModel rM = Helper.JsonDeserializeObject<ResponseModel>(retJSON);
                if (rM.Status == 0 && rM.ErrCode == 0)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {

                return -1;
            }
        }

        /// <summary>
        /// 订单购买成功
        /// </summary>
        /// <param name="accid">店铺Id</param>
        /// <param name="busId">业务Id</param>
        /// <returns></returns>
        public int OrderSuccessPushMessage(int accid, int busId)
        {
            try
            {
                Dictionary<string, object> postData = new Dictionary<string, object>();

                var objData = new Dictionary<string, string>();
                objData["accId"] = accid.ToString();
                objData["itemid"] = busId.ToString();
                var retJSON = HttpUtility.HttpPost(apiHost + "msgcenter?template=ordersuccess", ApiSignature(), objData);
                ResponseModel rM = Helper.JsonDeserializeObject<ResponseModel>(retJSON);
                if (rM.Status == 0 && rM.ErrCode == 0)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
		/// <summary>
		/// 优惠券绑定推送
		/// </summary>
		/// <param name="accid"></param>
		/// <param name="couponName">优惠券名</param>
		/// <param name="expireTime">到期时间</param>
		/// <returns></returns>
		public int CouponBindingNotifyPushMessage(int accid, string couponName, DateTime? expireTime = null)
		{
			try
			{
				Dictionary<string, object> postData = new Dictionary<string, object>();

				var objData = new Dictionary<string, string>();
				objData["accId"] = accid.ToString();
				objData["couponName"] = couponName;
				objData["expireTime"] = expireTime.ToString();
				var retJSON = HttpUtility.HttpPost(apiHost + "msgcenter?template=couponBindingNotify", ApiSignature(), objData);
				ResponseModel rM = Helper.JsonDeserializeObject<ResponseModel>(retJSON);
				if (rM.Status == 0 && rM.ErrCode == 0)
				{
					return 0;
				}
				else
				{
					return -1;
				}
			}
			catch (Exception ex)
			{
				return -1;
			}
		}
		/// <summary>
        /// 发送移动端订单成功推送模板消息
		/// </summary>
		/// <param name="accid">店铺Id</param>
		/// <param name="busName">业务名称</param>
		/// <returns></returns>
		public int MobileOrderSuccessPushMessage(int accid, string busName)
		{
			try
			{
				Dictionary<string, object> postData = new Dictionary<string, object>();

				var objData = new Dictionary<string, string>();
				objData["accId"] = accid.ToString();
				objData["itemName"] = busName;
				var retJSON = HttpUtility.HttpPost(apiHost + "msgcenter/PostMobile?mobileTemplate=mobileOrderSuccess", ApiSignature(), objData);
				ResponseModel rM = Helper.JsonDeserializeObject<ResponseModel>(retJSON);
				if (rM.Status == 0 && rM.ErrCode == 0)
				{
					return 0;
				}
				else
				{
					return -1;
				}
			}
			catch (Exception ex)
			{
				return -1;
			}
		}

		/// <summary>
		/// 移动端第二次登陆推送
		/// </summary>
		/// <param name="accid">店铺Id</param>
		/// <returns></returns>
		public int MobileSecondTimesLogin(int accid)
		{
			try
			{
				Dictionary<string, object> postData = new Dictionary<string, object>();

				var objData = new Dictionary<string, string>();
				objData["accId"] = accid.ToString();
				var retJSON = HttpUtility.HttpPost(apiHost + "msgcenter/PostMobile?mobileTemplate=mobileSecondTimesLogin", ApiSignature(), objData);
				ResponseModel rM = Helper.JsonDeserializeObject<ResponseModel>(retJSON);
				if (rM.Status == 0 && rM.ErrCode == 0)
				{
					return 0;
				}
				else
				{
					return -1;
				}
			}
			catch (Exception ex)
			{
				return -1;
			}
		}
        /// <summary>
        /// 推送信息
        /// </summary>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static int TemplatePushMessage(string template, Dictionary<string, string> postData)
        {
            try
            {
                var retJSON = HttpUtility.HttpPost(apiHost + "msgcenter?template=" + template, ApiSignature(), postData);
                ResponseModel rM = Helper.JsonDeserializeObject<ResponseModel>(retJSON);
                if (rM.Status == 0 && rM.ErrCode == 0)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public static Dictionary<string, string> ApiSignature()
        {
            String _Secret = "WSKXQ896OMDCS7YMWVZFbwPYzlQp33zg";
            String _AppKey = "I200LD02WSUQIRRGYM";

            Random r = new Random();
            String i1 = (r.Next(10000) + 1000).ToString();

            String _timestamp = Helper.GetTimeStamp();


            String sign = "";
            sign = Helper.Md5Hash(_AppKey + _timestamp + i1 + _Secret);



            String _Signature = sign;


            Dictionary<string, string> head = new Dictionary<string, string>();

            head.Add("Signature", _Signature);
            head.Add("Timestamp", _timestamp);
            head.Add("Nonce", i1);
            head.Add("AppKey", _AppKey);
            return head;
        }



        /// <summary>
        /// 订单处理
        /// </summary>
        /// <param name="orderNo"></param>
        public void OrderReturn(string orderNo)
        {
            LogHelper.AsyncLogEx("OrderOtherDispose", "请求后台处理信息通知:" + orderNo, 0, "");

            string Host = ConfigurationManager.AppSettings["API_SYS"];
            var objData = new Dictionary<string, string>();
            objData["type"] = "orderrevisit";
            objData["order"] = orderNo;

            try
            {
                var retJSON = HttpUtility.HttpPost("https://"+ Host +"/API/OrederRevisit.ashx", GetSignature(), objData);
            }
            catch (Exception ex)
            {
                LogHelper.AsyncLogEx("OrderOtherDispose", "请求后台处理信息错误:" + orderNo, 0, ex.ToString());
            }
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


    #region ResponseModel 请求响应(Api)
    /// <summary>
    /// 请求响应(Api)
    /// </summary>
    public class ResponseModel
    {
        public ResponseModel()
        {
            Ver = "1.0";
        }
        /// <summary>
        /// 执行状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// API版本
        /// </summary>
        public string Ver { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrCode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg { get; set; }

        /// <summary>
        /// 业务Obj
        /// </summary>
        public object Data { get; set; }
    }
    #endregion
}
