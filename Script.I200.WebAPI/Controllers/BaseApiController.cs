using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http;
using Script.I200.Application.Users;
using Script.I200.Core;
using Script.I200.Entity.API;
using Script.I200.Entity.Enum;
using Script.I200.WebAPIFramework.WebAPI;

namespace Script.I200.WebAPI.Controllers
{
    public class BaseApiController : ApiController
    {
        private const string HttpContextKey = "MS_HttpContext";

        private const string RemoteEndpointMessage =
            "System.ServiceModel.Channels.RemoteEndpointMessageProperty";

        private const string OwinContext = "MS_OwinContext";
        private readonly IUserQueryService _userQueryService = new UserQueryService();

        /// <summary>
        ///     获取登陆用户的相关信息
        /// </summary>
        /// <returns></returns>
        protected UserContext GetUserContext()
        {
            var userContext = new UserContext();
            if (!Request.Properties.ContainsKey(HttpContextKey))
            {
                return null;
            }

            if (Request.Properties.ContainsKey("userInfoContext"))
            {
                userContext = Request.Properties["userInfoContext"] as UserContext;
            }
            else
            {
                var request = Request.Headers;
                var oToken = request.SingleOrDefault(x => x.Key.ToLower() == "token");
                var oUserId = request.SingleOrDefault(x => x.Key.ToLower() == "userid");
                var oAppkey = request.SingleOrDefault(x => x.Key.ToLower() == "appkey");
                var strToken = oToken.Key == null ? string.Empty : oToken.Value.FirstOrDefault();
                var strAppkey = oAppkey.Key == null ? string.Empty : oAppkey.Value.FirstOrDefault();
                var strUserId = oUserId.Key == null ? string.Empty : oUserId.Value.FirstOrDefault();

                //校验Token不能为空
                if (string.IsNullOrWhiteSpace(strToken))
                {
                    throw new YuanbeiHttpRequestException((int)ErrorCodeEnum.TokenIsExpired, "缺少token值");
                }


                //校验UserId不能为空
                //if (string.IsNullOrWhiteSpace(strUserId))
                //{
                //    throw new YuanbeiHttpRequestException((int)ErrorCodeEnum.TokenIsExpired, "缺少userId值");
                //}

                //获取用户的相关信息
                var user = _userQueryService.GetUserContext(strToken, strAppkey, strUserId);
                if (user != null)
                {
                    userContext.AccId = user.AccId;
                    userContext.UserId = user.UserId;
                    userContext.Operater = user.UserId;
                    userContext.OperaterName = user.OperaterName;
                    userContext.Token = user.Token;
                    userContext.AppKey = user.AppKey;
                    userContext.Role = user.Role;
                    userContext.MasterId = user.MasterId;
                    userContext.PhoneNumber = user.PhoneNumber;
                    userContext.CompanyName = user.CompanyName;
                    userContext.Powers = user.Powers;
                    userContext.AccVersion = _userQueryService.GetAccountVersion(user.AccId);
                }
                userContext.IpAddress = Request.GetClientIpAddress();
            }

            if (userContext == null || userContext.AccId == 0 || userContext.UserId == 0)
            {
                throw new YuanbeiHttpRequestException((int)ErrorCodeEnum.TokenIsExpired, "没有找到用户相关信息");
            }

            // userContext.TrackingId = Request.Properties[Constants.RequestLogId].ToString();
            return userContext;
        }

        /// <summary>
        ///     返回成功结果
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected ResponseModel Success(object data)
        {
            if (data != null && data is ResponseModel)
            {
                var result = (ResponseModel) data;
                result.Code = result.Code;
                result.Message = result.Message;
                return result;
            }

            return new ResponseModel
            {
                Code = (int) ErrorCodeEnum.Success,
                Message = string.Empty,
                Data = data
            };
        }

        /// <summary>
        ///     返回失败结果
        /// </summary>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        protected ResponseModel Fail(ErrorCodeEnum errorCode)
        {
            var errorDescription = "未知错误";
            var memberInfo = (typeof (ErrorCodeEnum)).GetMember(errorCode.ToString());
            if (memberInfo.Length > 0)
            {
                var attrs = memberInfo[0].GetCustomAttributes(typeof (DescriptionAttribute), false);

                if (attrs.Length > 0)
                {
                    //返回枚举值得描述信息
                    errorDescription = ((DescriptionAttribute) attrs[0]).Description;
                }
            }

            return new ResponseModel
            {
                Code = (int) errorCode,
                Message = errorDescription
            };
        }

        /// <summary>
        ///     Model参数校验  (校验不通过直接返回错误model)
        /// </summary>
        /// <param name="checkResult"></param>
        /// <returns></returns>
        protected bool CheckModelParams(out ResponseModel checkResult)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(s => s.Errors).First();
                if (error!= null)
                {
                    checkResult = new ResponseModel
                    {
                        Code = (int) ErrorCodeEnum.ParamsInvalid,
                        Message = error.ErrorMessage,
                        Data = null
                    };
                    return true;
                }
            }
            checkResult = null;
            return false;
        }

        /// <summary>
        ///     校验请求参数是否为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestParam"></param>
        /// <param name="checkResult"></param>
        /// <returns></returns>
        public bool CheckRequestParamIsNull<T>(T requestParam, out ResponseModel checkResult)
        {
            checkResult = null;
            if (requestParam != null) return true;
            checkResult = Fail(ErrorCodeEnum.ParamIsNullArgument);
            return false;
        }

        /// <summary>
        ///     返回结果集
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public ResponseModel FunctionReturn(ResponseModel result)
        {
            return result.Code == (int) ErrorCodeEnum.Success ? Success(result) : Fail((ErrorCodeEnum) result.Code);
        }

        /// <summary>
        ///     校验是否含有特殊字符
        /// </summary>
        /// <param name="checkName"></param>
        /// <param name="checkResult"></param>
        /// <param name="regExp"></param>
        /// <returns></returns>
        public bool CheckNameIsValid(string checkName, out ResponseModel checkResult, Regex regExp)
        {
            if (regExp == null) throw new ArgumentNullException("regExp");
            checkResult = null;
            var isContainStr = regExp.IsMatch(checkName);
            if (isContainStr) return true;
            checkResult = Fail(ErrorCodeEnum.IsValidName);
            return false;
        }

        /// <summary>
        /// 校验单页最大数据量不超过100条,防止通过接口单页请求大批量数据
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="functionReturn"></param>
        /// <returns></returns>
        public bool CheckIsMoreThanPageSize(int? pageSize, out ResponseModel functionReturn)
        {
            functionReturn = null;
            if (pageSize > 100)
            {

                functionReturn = FunctionReturn(new ResponseModel
                {
                    Code = (int)ErrorCodeEnum.MoreThanMaxSize
                });
                return true;
            }
            return false;
        }
    }
}