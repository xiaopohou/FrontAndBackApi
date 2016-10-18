using System;
using System.Web.Http;
using Script.I200.Application.Shared;
using Script.I200.Entity.Api.Shared;
using Script.I200.Entity.API;
using Script.I200.Entity.Enum;

namespace Script.I200.WebAPI.Controllers
{
    /// <summary>
    ///     公共的数据Api请求接口
    /// </summary>
    [RoutePrefix("v0/share")]
    public class SharedController : BaseApiController
    {
        private readonly ISharedService _shardService = new SharedService();

        /// <summary>
        ///     发送验证码
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [Route("verify-code-phone")]
        [HttpPost, HttpOptions]
        public ResponseModel SendVerficationCodeToPhone(VerficationCodeRequestParams parameters)
        {
            if (parameters == null)
            {
                return Fail(ErrorCodeEnum.NullArguments);
            }

            if (!Enum.IsDefined(typeof (CaptchaEnum), parameters.Context))
            {
                return Fail(ErrorCodeEnum.UnknowAuthCodeContex);
            }

            if (string.IsNullOrWhiteSpace(parameters.Phone))
            {
                return Fail(ErrorCodeEnum.PhoneCanNotBeNull);
            }

            return _shardService.SendVerificationCode(GetUserContext(), parameters.Context, CaptchaPhoneEmailEnum.Phone,
                parameters.Phone);
        }

        /// <summary>
        ///     获取验证账户
        /// </summary>
        /// <returns></returns>
        [Route("authentication-accounts")]
        [HttpGet, HttpOptions]
        public ResponseModel GetVerificationAccount()
        {
            return _shardService.GetVerificationAccount(GetUserContext());
        }

        /// <summary>
        ///     校验验证码
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [Route("verify-code-phone")]
        [HttpPut, HttpOptions]
        public ResponseModel CheckVerificationCode(VerficationCodeRequestParams parameters)
        {
            return _shardService.CheckVerificationCode(GetUserContext(), parameters.Context,
                (int) CaptchaPhoneEmailEnum.Phone, parameters.Code, parameters.Phone);
        }

        /// <summary>
        ///     获取绑定账户额外信息
        /// </summary>
        /// <returns></returns>
        [Route("withdrawing-accounts")]
        [HttpGet, HttpOptions]
        public ResponseModel GetWithdrawingExtraInfo()
        {
            return _shardService.GetWithdrawingExtraInfo(GetUserContext());
        }

        /// <summary>
        ///     获取店铺人员信息
        /// </summary>
        /// <returns></returns>
        [Route("accountUserInfo")]
        [HttpGet, HttpOptions]
        public ResponseModel GetAccountUserInfo()
        {
            return Success(_shardService.GetAccountUserInfo(GetUserContext()));
        }

        /// <summary>
        ///     导出Excel数据
        /// </summary>
        /// <returns></returns>
        [Route("export")]
        [HttpPost]
        public ResponseModel ExportExcelData()
        {
            return _shardService.ExportExcelData(GetUserContext());
        }

        /// <summary>
        ///     发送短信
        /// </summary>
        /// <returns></returns>
        [Route("sendsms")]
        [HttpPost, HttpOptions]
        public ResponseModel SendSms()
        {
            var result = _shardService.SendSms(GetUserContext());
            return new ResponseModel
            {
                Code = 0,
                Message = result ? "发送成功" : "发送失败",
                Data = null
            };
        }
    }
}