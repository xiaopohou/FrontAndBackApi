namespace Script.I200.Entity.Api.UserStoreMoneyCard
{
    /// <summary>
    ///  用户储值卡列表结果项
    /// </summary>
    public class UserStoreMoneySearchResultItem
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 总充值金额
        /// </summary>
        public decimal TotalStoreMoney { get; set; }

        /// <summary>
        /// 卡内余额
        /// </summary>
        public decimal Balance { get; set; }
    }
}