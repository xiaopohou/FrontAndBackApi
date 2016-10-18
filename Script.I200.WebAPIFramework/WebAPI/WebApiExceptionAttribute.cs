using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Filters;
using Newtonsoft.Json.Serialization;
using Script.I200.Application.Logging;
using Script.I200.Core;
using Script.I200.Entity;
using Script.I200.Entity.API;
using Script.I200.Entity.Enum;

namespace Script.I200.WebAPIFramework.WebAPI
{
    /// <summary>
    ///     异常拦截器
    /// </summary>
    public class WebApiExceptionAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger _logger = new NLogger();
        private readonly string ExceptionMessageTips = "您输入的信息暂时无法被生意专家识别，如果该信息对您十分重要可联系服务专员，谢谢！";

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var responseModel = new ResponseModel();

            var jsonFormatter = new JsonMediaTypeFormatter
            {
                SerializerSettings = {ContractResolver = new CamelCasePropertyNamesContractResolver()}
            };

            var exception = actionExecutedContext.Exception as HttpResponseException;
            if (exception != null && exception.Response.StatusCode == HttpStatusCode.OK)
            {
                return;
            }

            var httpRequestException = actionExecutedContext.Exception as YuanbeiHttpRequestException;
            if (httpRequestException != null)
            {
                if (httpRequestException.Code==(int)ErrorCodeEnum.TokenIsExpired)
                {
                    responseModel.Code = httpRequestException.Code;
                    responseModel.Message = ExceptionMessageTips;
                    actionExecutedContext.Response = new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Content = new ObjectContent<ResponseModel>(responseModel, jsonFormatter, "application/json")
                    };
                }
                else
                {
                    responseModel.Code = httpRequestException.Code;
                    responseModel.Message = ExceptionMessageTips;
                    actionExecutedContext.Response = new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Content = new ObjectContent<ResponseModel>(responseModel, jsonFormatter, "application/json")
                    };
                }
            }

            var message = actionExecutedContext.Exception.Message;
            if (actionExecutedContext.Response == null && !(actionExecutedContext.Exception is YuanbeiException))
            {
                var resultModel = new ResponseModel
                {
                    Code = (int) HttpStatusCode.InternalServerError,
                    Message = ExceptionMessageTips
                };

                actionExecutedContext.Response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new ObjectContent<ResponseModel>(resultModel, jsonFormatter, "application/json")
                };
            }

            if (actionExecutedContext.Exception != null)
            {
                var requestLogId = actionExecutedContext.Request.Properties.ContainsKey(Constants.RequestLogId)
                    ? actionExecutedContext.Request.Properties[Constants.RequestLogId].ToString()
                    : string.Empty;

                _logger.Error(actionExecutedContext.Exception.Message, requestLogId, actionExecutedContext.Exception);
            }
        }
    }
}