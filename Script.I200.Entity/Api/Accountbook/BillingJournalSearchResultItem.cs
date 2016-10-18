using System;

namespace Script.I200.Entity.Api.Accountbook
{
    /// <summary>
    /// 收单记录列表查询返回结果项
    /// </summary>
    public class BillingJournalSearchResultItem
    {
        /// <summary>
        /// 交易时间
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 业务Id
        /// </summary>
        public int BusinessId { get; set; }

        /// <summary>
        /// 业务名称
        /// </summary>
        public string BusinessName { get; set; }

        /// <summary>
        /// 交易单号
        /// </summary>
        public string OutTradeNumber { get; set; }

        /// <summary>
        /// 流水单号
        /// </summary>
        public string JournalNumber { get; set; }

        /// <summary>
        /// 销售项目
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 收款金额
        /// </summary>
        public decimal AmountDue { get; set; }

        /// <summary>
        ///手续费
        /// </summary>
        public decimal Poundage { get; set; }

        /// <summary>
        /// 到账金额
        /// </summary>
        public decimal AmountReceived { get; set; }

        /// <summary>
        /// 交易状态
        /// </summary>
        public int Status { get; set; }
    }
}
