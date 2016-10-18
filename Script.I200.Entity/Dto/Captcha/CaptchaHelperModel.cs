using Script.I200.Entity.Enum;

namespace Script.I200.Entity.Dto.Captcha
{
    public class CaptchaHelperModel
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMessage { get; set; }

        /// <summary>
        /// 方式
        /// </summary>
        public string Type { get; set; }

        public CaptchaHelperModel()
        {
            IsSuccess = false;
        }

        public CaptchaHelperModel(bool isSuccess, string errMessage, CaptchaPhoneEmailEnum typeenum)
        {
            IsSuccess = isSuccess;
            ErrMessage = errMessage;
            Type = typeenum.ToString();
        }
    }
}
