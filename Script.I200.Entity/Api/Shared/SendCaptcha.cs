namespace Script.I200.Entity.Api.Shared
{
    public class SendCaptcha
    {
        /// <summary>
        /// 1提现，2绑定银行卡，3解绑银行卡
        /// </summary>
        public int CaptchaEnum { get; set; }

        /// <summary>
        /// 1手机，2邮箱
        /// </summary>
        public int PhoneEmailEnum { get; set; }
    }

    public class CheckCaptcha : SendCaptcha
    {
        /// <summary>
        /// 验证码
        /// </summary>
        public int CaptchaCode { get; set; }
    }

    /// <summary>
    /// 验证zhnaghu
    /// </summary>
    public class CheckAccount
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Email { get; set; }
    }
}