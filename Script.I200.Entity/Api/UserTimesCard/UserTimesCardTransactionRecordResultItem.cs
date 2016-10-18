using System;

namespace Script.I200.Entity.Api.UserTimesCard
{
    /// <summary>
    /// 用户计次卡交易记录结果集
    /// </summary>
    public class UserTimesCardTransactionRecordResultItem
    {
        /// <summary>
        /// 交易时间
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 会员名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 操作类型 （1：充次 2：消费）
        /// </summary>
        public int OperateType { get; set; }

        /// <summary>
        /// 店铺计次卡名
        /// </summary>
        public string AccCardName { get; set; }

        /// <summary>
        /// 变动次数
        /// </summary>
        public int EditTimes { get; set; }

        /// <summary>
        /// 发生金额
        /// </summary>
        public decimal ReceiveMoney { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public int PayType { get; set; }

        /// <summary>
        /// 销售人员
        /// </summary>
        public string Salesman { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        public string Remark { get; set; }

    }
}