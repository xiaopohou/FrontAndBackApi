namespace Script.I200.Entity.Api.Users
{
    /// <summary>
    /// 店铺会员消费统计
    /// </summary>
    public class UserStatistics
    {
        /// <summary>
        /// 会员消费次数
        /// </summary>
        public int BuyTimes { get; set; }

        /// <summary>
        /// 会员购物数量
        /// </summary>
        public int BuyCount { get; set; }

        /// <summary>
        /// 会员欠款金额
        /// </summary>
        public decimal UnPaidMoney { get; set; }

        /// <summary>
        /// 会员欠款笔数
        /// </summary>
        public int UnPaidCount { get; set; }

        /// <summary>
        /// 会员消费金额
        /// </summary>
        public decimal ConsumeMoney { get; set; }

        /// <summary>
        /// 会员客单价
        /// </summary>
        public decimal ConsumeAvgMoney { get; set; }
    }
}
