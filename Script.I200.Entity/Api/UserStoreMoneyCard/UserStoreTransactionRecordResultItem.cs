using System;

namespace Script.I200.Entity.Api.UserStoreMoneyCard
{
    /// <summary>
    ///     用户储值交易记录查询结果项
    /// </summary>
    public class UserStoreTransactionRecordResultItem
    {
        /// <summary>
        ///     交易时间
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        ///     用户Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///     用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     操作类型
        /// </summary>
        public int? OperateType { get; set; }

        /// <summary>
        ///     操作类型文本
        /// </summary>
        public string OperateTypeText { get; set; }

        /// <summary>
        ///     变动金额
        /// </summary>
        public decimal EditMoney { get; set; }

        /// <summary>
        ///     卡内余额
        /// </summary>
        public decimal StoreMoney { get; set; }

        /// <summary>
        ///     销售人员
        /// </summary>
        public string Salesman { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        public string Remark { get; set; }
    }
}