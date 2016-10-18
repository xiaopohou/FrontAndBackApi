using System;

namespace Script.I200.Entity.API.Accountbook
{
    /// <summary>
    /// 资金详情列表查询返回实体
    /// </summary>
    public class AccountbookJournalSearchResultItem
    {
        /// <summary>
        /// 流水号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 到账时间
        /// </summary>
        public DateTime TransferedAt { get; set; }

        /// <summary>
        /// 交易类型
        /// </summary>
        public int TradeType { get; set; }

        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal AmountTrade { get; set; }

        /// <summary>
        /// 交易后金额
        /// </summary>
        public decimal Balance { get; set; }
    }

    public class AccountbookJournalSearchResult : PaginationBase<AccountbookJournalSearchResultItem>
    {

        /// <summary>
        /// 总入账
        /// </summary>
        public double TotalAmountIn { get; set; }

        /// <summary>
        /// 总出账
        /// </summary>
        public double TotalAmoutOut { get; set; }

  
    }
}