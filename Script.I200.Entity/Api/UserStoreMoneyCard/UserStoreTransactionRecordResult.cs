using Script.I200.Entity.API;

namespace Script.I200.Entity.Api.UserStoreMoneyCard
{
    /// <summary>
    /// 会员储值交易记录结果集
    /// </summary>
    public class UserStoreTransactionRecordResult : PaginationBase<UserStoreTransactionRecordResultItem>
    {
        /// <summary>
        /// 总计储值(累计)
        /// </summary>
        public decimal TotalStoreMoney { get; set; }

        /// <summary>
        /// 总计消费
        /// </summary>
        public decimal TotalShopping { get; set; }

        /// <summary>
        /// 总计储值余额
        /// </summary>
        public decimal TotalBalance { get; set; }

        /// <summary>
        /// 总会员数
        /// </summary>
        public int TotalUserCount { get; set; }
    }
}