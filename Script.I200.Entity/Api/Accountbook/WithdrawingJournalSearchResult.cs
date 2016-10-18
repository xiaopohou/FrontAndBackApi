using Script.I200.Entity.API;

namespace Script.I200.Entity.Api.Accountbook

{
    /// <summary>
    /// 提现记录列表结果实体
    /// </summary>
    public class WithdrawingJournalSearchResult : PaginationBase<WithdrawingJournalSearchResultItem>
    {
        /// <summary>
        /// 累计提现金额
        /// </summary>
        public double TotalWithdrawingAmount { get; set; }
    }
}