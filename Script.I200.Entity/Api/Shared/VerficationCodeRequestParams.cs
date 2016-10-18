using Script.I200.Entity.Enum;

namespace Script.I200.Entity.Api.Shared
{
    public class VerficationCodeRequestParams
    {
        /// <summary>
        /// 验证场景 
        /// </summary>
        public int Context { get; set; }

        /// <summary>
        /// 送达手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Email地址
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 发送渠道
        /// </summary>
        public CaptchaPhoneEmailEnum Channel { get; set; }
    }
}
