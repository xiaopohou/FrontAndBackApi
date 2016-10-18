using System;

namespace Script.I200.Entity.Api.Account
{
    public class AccountOverviewDetail
    {
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 销售总额
        /// </summary>
        public decimal SalesMoney { get; set; }

        /// <summary>
        /// 销售毛利
        /// </summary>
        public decimal SalesGrossProfit { get; set; }
    }
}
