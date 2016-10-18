using System;

namespace Script.I200.Entity.Dto.User
{
    public class UserLog
    {
        public long Id { get; set; }

        /// <summary>
        /// 店铺Id
        /// </summary>
        public long AccId { get; set; }

        /// <summary>
        /// 归属店铺Id
        /// </summary>
        public long OriginAccId { get; set; }

        /// <summary>
        /// 会员Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        public int LogType { get; set; }

        /// <summary>
        /// 日志分类
        /// </summary>
        public int ItemType { get; set; }

        /// <summary>
        /// 原始值
        /// </summary>
        public decimal OriginValue { get; set; }

        /// <summary>
        /// 变动值
        /// </summary>
        public decimal EditValue { get; set; }

        /// <summary>
        /// 最终值
        /// </summary>
        public decimal FinalValue { get; set; }

        /// <summary>
        /// 日志时间
        /// </summary>
        public DateTime LogTime { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperatorTime { get; set; }

        /// <summary>
        /// 操作人员Id
        /// </summary>
        public long OperatorId { get; set; }

        /// <summary>
        /// 操作人员Ip
        /// </summary>
        public string OperatorIp { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 附加信息
        /// </summary>
        public string Flag { get; set; }

        /// <summary>
        /// 变动金额
        /// </summary>
        public decimal EditMoney { get; set; }

        /// <summary>
        /// 变动状态
        /// </summary>
        public int FlagStatus { get; set; }

        /// <summary>
        /// 变动状态日期
        /// </summary>
        public DateTime FlagStatusTime { get; set; }

        /// <summary>
        /// 金额类型
        /// </summary>
        public int EditMoneyType { get; set; }

        /// <summary>
        /// 绑定几次卡Id
        /// </summary>
        public long BindCardId { get; set; }

        /// <summary>
        /// 添加指定操作人ID
        /// </summary>
        public long AddedLgUserId { get; set; }
    }
}
