using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using CommonLib;
using Newtonsoft.Json;
using Script.I200.Application.Logging;
using Script.I200.Core;
using Script.I200.Entity.API;
using Script.I200.Entity.Dto.Accountbook;
using Script.I200.Entity.Enum;

namespace Script.I200.Application.Proxy
{
    public class BaseProxyService
    {
        /// <summary>
        /// 资金账户信息地址
        /// </summary>
        private static readonly string ProxyUrl = string.Format("{0}",
            ConfigurationManager.AppSettings["BankPlatServiceAddress"]);

        /// <summary>
        /// 提现秘钥
        /// </summary>
        private static readonly string BPS_Withdrawal = ConfigurationManager.AppSettings["BPS_Withdrawal"];

        /// <summary>
        /// 收单秘钥
        /// </summary>
        private static readonly string BPS_Acquir = ConfigurationManager.AppSettings["BPS_Acquir"];

        /// <summary>
        /// 查询秘钥
        /// </summary>
        private static readonly string BPS_Select = ConfigurationManager.AppSettings["BPS_Select"];

        /// <summary>
        /// 提现AppKey
        /// </summary>
        private static readonly string BPS_Withdrawal_Key = "OxSurWnWcOXtXzwLQAi6";

        /// <summary>
        /// 收单AppKey
        /// </summary>
        private static readonly string BPS_Acquir_Key = "HwGhLA5Twma7w4gRV2g4";

        /// <summary>
        /// 查询AppKey
        /// </summary>
        private static readonly string BPS_Select_Key = "wgDHIGc3KSDMxCtTkmXc";

        /// <summary>
        ///收单地址
        /// </summary>
        //protected string BaseUrl = "http://192.168.20.100:8091/api/";
        protected readonly string BaseUrl = ConfigurationManager.AppSettings["BankPlatServiceAddress"];

        protected readonly ILogger _Logger = new NLogger();


        private ResponseSerializationModel<T> ExecuteWithTryCatch<T>(Func<HttpClient, HttpResponseMessage> func,
            string httpMethod,
            string url, object requestBody, UserContext userContext, Dictionary<string, string> headers) where T : class
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //var requestLog= new RequestLog();
                //var stopwatch = new Stopwatch();

                //requestLog.LogId = Guid.NewGuid();
                //requestLog.CreatedOnUtc = DateTime.Now;

                HttpResponseMessage response = null;
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }

                response = func(client);

                // try
                // {
                //     requestLog.Headers = BuildHeaderString(headers);
                //     requestLog.ParentId = userContext.TrackingId;
                //     requestLog.Request = requestBody == null ? string.Empty : Helper.JsonSerializeObject(requestBody);
                //     requestLog.Method = httpMethod.ToUpper();

                //     stopwatch.Start();


                //     requestLog.Url = response.RequestMessage.RequestUri.AbsoluteUri;
                //     requestLog.StatusCode = (int)response.StatusCode;
                //     requestLog.Response = response.Content.ReadAsStringAsync().Result;                        
                // }
                //catch (Exception ex)
                // {
                //     requestLog.Exception = true;
                //     throw ex;
                // }
                // finally
                // {
                //     stopwatch.Stop();
                //     requestLog.ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;

                //     Task.Run(() =>
                //     {
                //         _Logger.InsertRequestLog(requestLog);
                //     });
                // }

                ResponseSerializationModel<T> responseModel;
                if (response.IsSuccessStatusCode)
                {
                    var strResult = response.Content.ReadAsStringAsync().Result;
                    responseModel =
                        JsonConvert.DeserializeObject(strResult, typeof (ResponseSerializationModel<T>)) as
                            ResponseSerializationModel<T>;
                }
                else
                {
                    // 远程请求返回失败
                    var requestException = new YuanbeiHttpRequestException((int) response.StatusCode,
                        "远程请求失败" + response.Content.ReadAsStringAsync().Result);
                    throw requestException;
                }

                //if (responseModel != null && responseModel.Code != (int) ResponseErrorcode.C200)
                //{
                //    var requestException = new YuanbeiHttpRequestException((int) responseModel.Code,
                //        "远程处理失败" + responseModel.Message);
                //    throw requestException;
                //}

                return responseModel;
            }
        }

        protected ResponseSerializationModel<T> RestPost<T, TEntity>(string url, UserContext userContext,
            TEntity request,
            Dictionary<string, string> headers) where T : class
        {
            return ExecuteWithTryCatch<T>(client => client.PostAsJsonAsync(url, request).Result, "post", url, request,
                userContext, headers);
        }

        protected ResponseSerializationModel<T> RestPut<T, TEntity>(string url, UserContext userContext, TEntity request,
            Dictionary<string, string> headers) where T : class
        {
            return ExecuteWithTryCatch<T>(client => client.PutAsJsonAsync(url, request).Result, "put", url, request,
                userContext, headers);
        }

        protected ResponseSerializationModel<T> RestGet<T>(string url, UserContext userContext,
            Dictionary<string, string> headers) where T : class
        {
            return ExecuteWithTryCatch<T>(client => client.GetAsync(url).Result, "get", url, null, userContext, headers);
        }

        protected ResponseSerializationModel<T> RestDelete<T>(string url, UserContext userContext,
            Dictionary<string, string> headers) where T : class
        {
            return ExecuteWithTryCatch<T>(client => client.DeleteAsync(url).Result, "delete", url, null, userContext,
                headers);
        }

        /// <summary>
        /// 填充header
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="bankPlatBusinessEnum"></param>
        /// <returns></returns>
        protected Dictionary<string, string> RestHead(UserContext userContext, BankPlatBusinessEnum bankPlatBusinessEnum)
        {
            var requestMd = CreateAuthCode(bankPlatBusinessEnum);
            var dic = new Dictionary<string, string>
            {
                {"BusinessKey", requestMd.BusinessKey},
                {"Signature", requestMd.Signature},
                {"Timestamp", requestMd.Timestamp},
                {"Nonce", requestMd.Nonce},
                {"AccId", userContext.AccId.ToString()},
                {"Operater", userContext.UserId.ToString()},
                {"OperaterName", userContext.OperaterName},
                {"Ip", userContext.IpAddress}
            };
            return dic;
        }

        /// <summary>
        /// 获取http头信息
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        protected string BuildHeaderString(Dictionary<string, string> headers)
        {
            if (headers == null || !headers.Any())
                return string.Empty;

            var result = new StringBuilder();
            foreach (var header in headers)
            {
                result.AppendFormat("{0}={1};", header.Key, header.Value);
            }

            return result.ToString();
        }


        /// <summary>
        /// 生成验证信息
        /// </summary>
        /// <returns></returns>
        public AccountbookRequest CreateAuthCode(BankPlatBusinessEnum businessEnum)
        {
            string stroBusinessKey = "";
            string strAppValue = "";
            switch (businessEnum)
            {
                //收单
                case BankPlatBusinessEnum.Acquir:
                    stroBusinessKey = BPS_Acquir_Key;
                    strAppValue = BPS_Acquir;
                    break;
                //提现
                case BankPlatBusinessEnum.Withdrawal:
                    stroBusinessKey = BPS_Withdrawal_Key;
                    strAppValue = BPS_Withdrawal;
                    break;
                //查询
                case BankPlatBusinessEnum.Select:
                    stroBusinessKey = BPS_Select_Key;
                    strAppValue = BPS_Select;
                    break;
                default:
                    break;
            }
            var requestMd = new AccountbookRequest
            {
                Timestamp = Helper.GetTimeStamp(),
                Nonce = Helper.GetRandomNum(),
                BusinessKey = stroBusinessKey
            };
            var strSign = new StringBuilder();
            strSign.Append(stroBusinessKey);
            strSign.Append(requestMd.Timestamp);
            strSign.Append(requestMd.Nonce);
            strSign.Append(strAppValue);

            requestMd.Signature = Helper.Md5Hash(strSign.ToString());

            return requestMd;
        }
    }
}