using Script.I200.Entity.Dto.Captcha;
using Script.I200.Entity.Enum;
using Script.I200.Entity.Model.User;

namespace Script.I200.Application.AuthCode
{
    public interface IAuthCodeService
    {
        /// <summary>
        /// 获取接收验证码的账号
        /// </summary>
        /// <param name="accId">店铺Id</param>
        /// <param name="IsEncrypt">是否加密</param>
        /// <returns></returns>
        UserPhoneEmail GetAuthPhoneEmailByAccId(int accId, bool IsEncrypt = false);

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="captchaEnum"></param>
        /// <param name="accId"></param>
        /// <param name="typeEnum"></param>
        /// <param name="phoneOrEmail"></param>
        /// <returns></returns>
        CaptchaHelperModel SendAuthCode(CaptchaEnum captchaEnum, int accId, CaptchaPhoneEmailEnum typeEnum,
            string phoneOrEmail);

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="captchaEnum"></param>
        /// <param name="accId"></param>
        /// <param name="typeEnum"></param>
        /// <param name="captchaCode"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        CaptchaHelperModel CheckCaptchaCode(CaptchaEnum captchaEnum, int accId,
            CaptchaPhoneEmailEnum typeEnum, int captchaCode, string phone);
    }
}