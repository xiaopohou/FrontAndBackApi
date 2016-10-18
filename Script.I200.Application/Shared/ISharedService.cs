using Script.I200.Entity.API;
using Script.I200.Entity.Enum;

namespace Script.I200.Application.Shared
{
    public interface ISharedService
    {
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <returns></returns>
        ResponseModel SendVerificationCode(UserContext userContext, int context, CaptchaPhoneEmailEnum typeEnum, string phoneOrEmail);

        /// <summary>
        /// 获取验证账户
        /// </summary>
        /// <returns></returns>
        ResponseModel GetVerificationAccount(UserContext userContext);

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <returns></returns>
       ResponseModel CheckVerificationCode(UserContext userContext, int context, int channel, int code, string phone);

        /// <summary>
        /// 获取绑定账户的额外信息
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        ResponseModel GetWithdrawingExtraInfo(UserContext userContext);

        /// <summary>
        /// 获取店铺人员信息
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        ResponseModel GetAccountUserInfo(UserContext userContext);

        /// <summary>
        /// 导出Excel数据
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        ResponseModel ExportExcelData(UserContext userContext);

        /// <summary>
        ///     发送短信
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        bool SendSms(UserContext userContext);
    }
}