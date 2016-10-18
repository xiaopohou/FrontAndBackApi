using System;
using Script.I200.Entity.Dto.Captcha;
using Script.I200.Entity.Enum;
using Script.I200.Entity.Model.User;

namespace Script.I200.Application.AuthCode
{
    public class MockAuthCodeService : IAuthCodeService
    {
        public UserPhoneEmail GetAuthPhoneEmailByAccId(int accId, bool IsEncrypt = false)
        {
            return new UserPhoneEmail()
            {
                Phone = "18502138674",
                Email = "liutengfei@yuanbei.biz"
            };
        }

 

        public CaptchaHelperModel CheckCaptchaCode(CaptchaEnum captchaEnum, int accId, CaptchaPhoneEmailEnum typeEnum,
            int captchaCode, string phone)
        {
            return new CaptchaHelperModel(true, String.Empty, typeEnum);
        }


        public CaptchaHelperModel SendAuthCode(CaptchaEnum captchaEnum, int accId, CaptchaPhoneEmailEnum typeEnum, string phoneOrEmail)
        {
            return new CaptchaHelperModel(true, string.Empty, typeEnum);
        }
    }
}