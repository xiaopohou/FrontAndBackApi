using Script.I200.Entity.API;

namespace Script.I200.Entity.Api.Accountbook
{
    /// <summary>
    /// 收单记录列表查询返回结果实体
    /// </summary>
    public class BillingJournalSearchResult:PaginationBase<BillingJournalSearchResultItem>
    {
        /// <summary>
        /// 累计收单金额
        /// </summary>
        public double TotalBillingAmount { get; set; }
    }
}
