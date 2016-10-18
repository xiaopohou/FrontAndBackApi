using Script.I200.Entity.Dto.Common;

namespace Script.I200.Entity.Dto.Accountbook
{
    /// <summary>
    /// 申请提现其他信息
    /// </summary>
    public class WithdrawOtherData
    {
        /// <summary>
        /// 是否有营业执照
        /// </summary>
        public bool IsHasBusinessUrl { get; set; }
        /// <summary>
        /// 又拍云SDK
        /// </summary>
        public YouPaiSDK youPaiSDK { get; set; }
    }
    
}
