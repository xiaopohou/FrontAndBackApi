namespace Script.I200.Entity.Api.Accountbook
{
    public class WithdrawingStatusAndBillingStatus
    {
        /// <summary>
        /// 提现列表状态
        /// </summary>
        public enum WithdrawingStatus
        {
            /// <summary>
            /// 提现中
            /// </summary>
            WithdrawingOn = 4,

            /// <summary>
            /// 提现成功
            /// </summary>
            WithdrawingSuccess = 5,

            /// <summary>
            /// 提现失败
            /// </summary>
            WithdrawingFail = 6
        }

        /// <summary>
        /// 收单记录列表状态
        /// </summary>
        public enum BillingStatus
        {
            /// <summary>
            /// 交易中
            /// </summary>
            BillingOn=1,

            /// <summary>
            /// 冻结资金
            /// </summary>
            FeeFrozen=4,

            /// <summary>
            /// 支付成功
            /// </summary>
            PaySuccess = 1000
        }
    }
}