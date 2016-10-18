using System;
using System.Data.SqlClient;
using CommonLib;
using Newtonsoft.Json;
using Script.I200.Application.AuthCode;
using Script.I200.Controls;
using Script.I200.Core.Config;
using Script.I200.Data;
using Script.I200.Entity.Api.Shared;
using Script.I200.Entity.API;
using Script.I200.Entity.Dto.Accountbook;
using Script.I200.Entity.Enum;
using Script.I200.Entity.Model.User;
using ResponseErrorcode = Script.I200.Model.Enum.ResponseErrorcode;
using ResponseModel = Script.I200.Entity.API.ResponseModel;

namespace Script.I200.Application.Shared
{
    public class SharedService : ISharedService
    {
        private IAuthCodeService _mockAuthCodeService = new MockAuthCodeService();
        private IAuthCodeService _authCodeService = new AuthCodeService();
        private readonly DapperRepository<AccountUser> _accountUser;

        /// <summary>
        /// 构造初始化
        /// </summary>
        public SharedService()
        {
            var dapperDbContext =
                new DapperDbContext(new SqlConnection(WebConfigSetting.Instance.I200DbConnectionString));
            _accountUser = new DapperRepository<AccountUser>(dapperDbContext);
        }

        /// <summary>
        ///  发送验证码
        /// </summary>
        /// <returns></returns>
        public ResponseModel SendVerificationCode(UserContext userContext, int context, CaptchaPhoneEmailEnum typeEnum,
            string phoneOrEmail)
        {
            var result = _authCodeService.SendAuthCode((CaptchaEnum) context, userContext.AccId, typeEnum, phoneOrEmail);

            return new ResponseModel
            {
                Code = result.IsSuccess ? (int) ErrorCodeEnum.Success : (int) ErrorCodeEnum.Failed,
                Message = result.ErrMessage
            };
        }

        /// <summary>
        /// 获取验证账户(手机或邮箱)
        /// </summary>
        /// <returns></returns>
        public ResponseModel GetVerificationAccount(UserContext userContext)
        {
            var result = _authCodeService.GetAuthPhoneEmailByAccId(userContext.AccId);
            var checkAccount = new CheckAccount
            {
                Phone = result.Phone,
                Email = result.Email
            };

            return new ResponseModel()
            {
                Code = (int) ErrorCodeEnum.Success,
                Data = checkAccount
            };
        }

        /// <summary>
        /// 校验验证码
        /// </summary>
        /// <returns></returns>
        public ResponseModel CheckVerificationCode(UserContext userContext,
            int context, int channel, int code,
            string phone)
        {
            var result = _authCodeService.CheckCaptchaCode((CaptchaEnum) context, userContext.AccId,
                (CaptchaPhoneEmailEnum) channel,
                code, phone);

            return new ResponseModel
            {
                Code = result.IsSuccess ? (int) ErrorCodeEnum.Success : (int) ErrorCodeEnum.Failed,
                Message = result.ErrMessage
            };
        }

        /// <summary>
        /// 获取绑定账户的额外信息
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public ResponseModel GetWithdrawingExtraInfo(UserContext userContext)
        {
            var result = CaptchaHelper.GetWithdrawingExtraInfo(userContext.AccId);
            var responseModel = new ResponseModel();
            var youPaiSdk = new UploadCalculationInfo();
            var payLoad = new YouPaiImgParams();
            if (result.Code == ResponseErrorcode.C200)
            {
                if (result.Data != null)
                {
                    var model =
                        JsonConvert.DeserializeObject<WithdrawOtherData>(
                            Helper.JsonSerializeObject(result.Data));
                    if (model.youPaiSDK != null)
                    {
                        payLoad.Policy = model.youPaiSDK.Policy;
                        payLoad.Prefix = model.youPaiSDK.ImgHostUrl;
                        payLoad.Signature = model.youPaiSDK.SignStr;
                        payLoad.BucketName = model.youPaiSDK.BucketName;
                        youPaiSdk.HasBusinessLicense = model.IsHasBusinessUrl;
                        youPaiSdk.Payload = payLoad;
                    }
                }
            }

            responseModel.Data = youPaiSdk;
            responseModel.Code = Convert.ToInt32(result.Code);
            responseModel.Message = result.Message;
            return responseModel;
        }

        /// <summary>
        /// 获取店铺人员信息
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public ResponseModel GetAccountUserInfo(UserContext userContext)
        {
            var result = _accountUser.FindAll(x => x.AccountId == userContext.AccId);
            return new ResponseModel
            {
                Code = 0,
                Message = "获取数据成功",
                Data = result
            };
        }

        /// <summary>
        /// 导出Excel数据
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public ResponseModel ExportExcelData(UserContext userContext)
        {
            //1.导出数据返回导出文件的src地址
            return new ResponseModel
            {
                Code = 0,
                Message = "获取数据成功！",
                Data = null
            };
        }

        /// <summary>
        ///     发送短信
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public bool SendSms(UserContext userContext)
        {
            return true;
        }
    }
}