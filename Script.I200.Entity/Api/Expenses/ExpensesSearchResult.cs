using Script.I200.Entity.API;
using Script.I200.Entity.Model.Expenses;

namespace Script.I200.Entity.Api.Expenses
{
    /// <summary>
    /// 支出记录返回结果实体
    /// </summary>
    public class ExpensesSearchResult : PaginationBase<PayRecord>
    {
        /// <summary>
        /// 累计支出金额
        /// </summary>
        public decimal TotalExpensesAmount { get; set; }
    }
}