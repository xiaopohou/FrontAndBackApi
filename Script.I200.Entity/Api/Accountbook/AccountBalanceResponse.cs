namespace Script.I200.Entity.Api.Accountbook
{
    /// <summary>
    /// 获取账户余额返回实体
    /// </summary>
    public class AccountBalanceResponse
    {
        /// <summary>
        /// 账户余额
        /// </summary>
        public decimal TotalBalance { get; set; }

        /// <summary>
        /// 可提现金额
        /// </summary>
        public decimal AvailableBalance { get; set; }


        /// <summary>
        /// 总交易金额 
        /// </summary>
        public decimal TotalAmount { get; set; }


        /// <summary>
        /// 微信收单入账总金额
        /// </summary>
        public decimal WechatGatheringAmount { get; set; }

        /// <summary>
        /// 手机橱窗入账总金额
        /// </summary>
        public decimal MobileShopGatheringAmount { get; set; }

        /// <summary>
        /// 微信收单交易笔数
        /// </summary>
        public int WechatTotalBillingJournalsNum { get; set; }

        /// <summary>
        /// 手机橱窗交易笔数
        /// </summary>
        public int MobileShopTotalBillingJournalsNum { get; set; }
    }
}