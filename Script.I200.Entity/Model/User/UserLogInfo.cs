using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.User
{
    [Table("T_User_LogInfo")]
    public class UserLogInfo
    {
        /// <summary>
        ///     日志Id
        /// </summary>
        [Key]
        [Identity]
        public int UserLogId { get; set; }

        /// <summary>
        ///     店铺Id
        /// </summary>
        public int AccId { get; set; }

        /// <summary>
        ///     归属店铺Id
        /// </summary>
        public int? OriginalAccId { get; set; }

        /// <summary>
        ///     会员Id
        /// </summary>
        public int UId { get; set; }

        /// <summary>
        ///     1：储值变更;2：计次卡变更;3：积分变更
        /// </summary>
        public int LogType { get; set; }

        /// <summary>
        ///     1：购买商品;2：兑换积分;3：修改积分;4：会员退货;5：计次卡充值;6：积分抵现
        /// </summary>
        public int ItemType { get; set; }

        /// <summary>
        ///     原始值
        /// </summary>
        public decimal? OriginalVal { get; set; }

        /// <summary>
        ///     变动值
        /// </summary>
        public decimal? EditVal { get; set; }

        /// <summary>
        ///     最终值
        /// </summary>
        public decimal? FinalVal { get; set; }

        /// <summary>
        ///     日志时间
        /// </summary>
        public DateTime? LogTime { get; set; }

        /// <summary>
        ///     操作时间
        /// </summary>
        public DateTime? OperatorTime { get; set; }

        /// <summary>
        ///     操作人员Id
        /// </summary>
        public int? OperatorId { get; set; }

        /// <summary>
        ///     操作人员Ip
        /// </summary>
        public string OperatorIp { get; set; }

        /// <summary>
        ///     备注信息
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        ///     附加信息
        /// </summary>
        public string Flag { get; set; }

        /// <summary>
        ///     实收金额
        /// </summary>
        public decimal? EditMoney { get; set; }

        /// <summary>
        ///     变动状态
        /// </summary>
        public int? FlagStatus { get; set; }

        /// <summary>
        ///     变动状态日期
        /// </summary>
        public DateTime? FlagStatusTime { get; set; }

        /// <summary>
        ///     支付方式(1: 现金， 2：银行卡 3：微信 4：支付宝)
        /// </summary>
        public int EditMoneyType { get; set; }

        /// <summary>
        ///     绑定卡Id
        /// </summary>
        public int? BindCardId { get; set; }

        /// <summary>
        ///     添加指定操作人ID
        /// </summary>
        public int? AddedLgUserId { get; set; }
    }
}