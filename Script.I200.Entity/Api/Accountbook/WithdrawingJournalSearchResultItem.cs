using System;

namespace Script.I200.Entity.Api.Accountbook
{
    /// <summary>
    /// 提现记录列表项
    /// </summary>
    public class WithdrawingJournalSearchResultItem
    {
        /// <summary>
        /// 流水号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 提现时间
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 提现金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 状态Id
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 到账时间
        /// </summary>
        public DateTime TransferedAt { get; set; }
    }
}
