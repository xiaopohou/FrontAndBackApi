using Script.I200.Entity.API;

namespace Script.I200.Entity.Api.UserStoreMoneyCard
{
    /// <summary>
    ///  获取用户储值卡列表结果集
    /// </summary>
    public class UserStoreMoneySearchResult : PaginationBase<UserStoreMoneySearchResultItem>
    {
        /// <summary>
        /// 汇总储值会员数量
        /// </summary>
        public int TotalUsers { get; set; }

        /// <summary>
        /// 汇总储值金额
        /// </summary>
        public decimal TotalStoreMoney { get; set; }

        /// <summary>
        /// 汇总赠送金额
        /// </summary>
        public decimal TotalExtraMoney { get; set; }

        /// <summary>
        /// 汇总卡内余额
        /// </summary>
        public decimal TotalBalance { get; set; }
    }
}