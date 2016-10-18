using System;
using CommonLib;
using Script.I200.Application.Logging;
using Script.I200.Application.Users;
using Script.I200.Controls;
using Script.I200.Core.Caching;
using Script.I200.Entity.Dto.Captcha;
using Script.I200.Entity.Enum;
using Script.I200.Entity.Model.User;

namespace Script.I200.Application.AuthCode
{
    public class AuthCodeService : IAuthCodeService
    {
        /// <summary>
        /// 错误次数限制
        /// </summary>
        private const int _checkWrongTimes = 10;

        /// <summary>
        /// 验证码过期时间（分钟）
        /// </summary>
        private const int _outTimeMinutes = 15;

        private IUserQueryService _userQueryService = new UserQueryService();
        private ILogger _logger = new NLogger();
        //private ICacheManager _cacheManager = new NullCache();

        public UserPhoneEmail GetAuthPhoneEmailByAccId(int accId, bool IsEncrypt = false)
        {
            return _userQueryService.GetShopAdministratorPhoneEmail(accId, true);
        }

        public CaptchaHelperModel SendAuthCode(CaptchaEnum captchaEnum, int accId, CaptchaPhoneEmailEnum typeEnum,
            string phoneOrEmail)
        {

            var redisCacheService = new RedisCacheService();
            if (string.IsNullOrWhiteSpace(phoneOrEmail))
            {
                throw new ArgumentNullException("phoneOrEmail", "手机号码邮箱地址为空");
            }


            //获取失败次数
            var strWrongTimesKey = GetWrongTimesKey(captchaEnum, accId);



            int iWrongTimes = redisCacheService.Get<int>(strWrongTimesKey);

            //判断验证码错误次数
            if (iWrongTimes > _checkWrongTimes)
            {
                return new CaptchaHelperModel(false, "验证码错误次数过多，请1小时后重试或联系客服", typeEnum);
            }

            //获取验证码key
            var strCaptchaKey = GetCaptchaKey(captchaEnum, accId, phoneOrEmail);
            if (string.IsNullOrEmpty(strCaptchaKey))
            {
                return new CaptchaHelperModel(false, "错误的手机号或邮箱", typeEnum);
            }


            //判断是否之前发过验证码
            int iCaptcha = redisCacheService.Get<int>(strCaptchaKey);
            if (iCaptcha == 0)
            {
                iCaptcha = Helper.GetInt32(Helper.GetRandomNum(), 111111);
            }

            var smsStr = string.Format("【生意专家】您本次获取的验证码为：{0}，此验证码{1}分钟内有效。维护您的数据安全是生意专家义不容辞的责任。", iCaptcha,
                _outTimeMinutes);
            var mailSend = new EmailSend();
            var smsSend = new SmsSend();
            var result = typeEnum == CaptchaPhoneEmailEnum.Phone
                ? smsSend.SendSys(phoneOrEmail, smsStr, 13)
                : mailSend.SendVerifiEmail(accId, "", phoneOrEmail, iCaptcha.ToString());

            if (result)
            {
                _logger.Debug("发送验证码成功：" + iCaptcha);
                redisCacheService.Set(strCaptchaKey, iCaptcha, _outTimeMinutes*60);
                return new CaptchaHelperModel(true, "发送验证码成功", CaptchaPhoneEmailEnum.Email);
            }
            else
            {
                return new CaptchaHelperModel(false, "发送失败", CaptchaPhoneEmailEnum.Email);
            }


        }


        public CaptchaHelperModel CheckCaptchaCode(CaptchaEnum captchaEnum, int accId, CaptchaPhoneEmailEnum typeEnum,
            int captchaCode, string phoneOrEmail)
        {
            var redisCacheService = new RedisCacheService();
            CaptchaHelperModel model;

            //获取失败次数
            var strWrongTimesKey = GetWrongTimesKey(captchaEnum, accId);
            int iWrongTimes = redisCacheService.Get<int>(strWrongTimesKey);

            //判断验证码错误次数
            if (iWrongTimes > 10)
            {
                return new CaptchaHelperModel(false, "验证码错误次数过多，请1小时后重试或联系客服", typeEnum);
            }

            //获取验证码key
            string strCaptchaKey = GetCaptchaKey(captchaEnum, accId, phoneOrEmail);
            if (string.IsNullOrWhiteSpace(strCaptchaKey))
            {
                return new CaptchaHelperModel(false, "验证码校验失败", typeEnum);
            }

            //获取发过的验证码
            int iCaptcha = redisCacheService.Get<int>(strCaptchaKey);
            if (iCaptcha == captchaCode)
            {
                _logger.Debug("验证验证码成功：" + iCaptcha);
                redisCacheService.RemoveKey(strWrongTimesKey);
                redisCacheService.RemoveKey(strCaptchaKey);
                return new CaptchaHelperModel(true, "验证码校验成功", typeEnum);

            }
            else if (iCaptcha == 0)
            {
                _logger.Debug("验证验证码失败：" + captchaCode);
                return new CaptchaHelperModel(false, "验证码过期，请重新申请发送验证码", typeEnum);
            }
            else
            {
                //失败添加失败次数
                redisCacheService.Set(strWrongTimesKey, iWrongTimes + 1, 60*60);
                return new CaptchaHelperModel(false, "验证码校验失败", typeEnum);
            }
        }

        /// <summary>
        /// 获取失败次数key
        /// </summary>
        /// <param name="captchaEnum"></param>
        /// <param name="accId"></param>
        /// <returns></returns>
        private static string GetWrongTimesKey(CaptchaEnum captchaEnum, int accId)
        {
            var strName = string.Empty;

            switch (captchaEnum)
            {
                case CaptchaEnum.BindCreditCard:
                case CaptchaEnum.UnbundCreditCard:
                case CaptchaEnum.Withdrawals:
                    strName = string.Format("I200{0}:{1}:{2}", CaptchaTypeEnum.BankPlatService,
                        CaptchaTypeEnum.WrongTimes,
                        accId);
                    break;
            }
            return strName;
        }

        /// <summary>
        /// 获取redis Key
        /// </summary>
        /// <param name="captchaEnum"></param>
        /// <param name="accId"></param>
        /// <param name="phoneEmali"></param>
        /// <returns></returns>
        private static string GetCaptchaKey(CaptchaEnum captchaEnum, int accId, string phoneEmali)
        {
            var strName = string.Empty;
            if (!string.IsNullOrEmpty(phoneEmali))
            {
                switch (captchaEnum)
                {
                    case CaptchaEnum.BindCreditCard:
                    case CaptchaEnum.UnbundCreditCard:
                    case CaptchaEnum.Withdrawals:
                        strName = string.Format("I200{0}:{1}:{2}:{3}", CaptchaTypeEnum.BankPlatService,
                            captchaEnum, accId, phoneEmali);
                        break;
                }
            }
            return strName;
        }

    }
}