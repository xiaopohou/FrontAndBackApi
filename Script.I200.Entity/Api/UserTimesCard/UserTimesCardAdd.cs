using System;

namespace Script.I200.Entity.Api.UserTimesCard
{
    /// <summary>
    /// 创建用户计次卡传入参数
    /// </summary>
    public class UserTimesCardAdd
    {
        /// <summary>
        /// 会员Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 会员姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 用户计次卡Id
        /// </summary>
        public int UserCardId { get; set; }

        /// <summary>
        /// 绑定服务商品的Id
        /// </summary>
        public int BindGoodsId { get; set; }

        /// <summary>
        /// 用户计次卡名称
        /// </summary>
        public string CardName { get; set; }

        /// <summary>
        /// 店铺计次卡Id
        /// </summary>
        public int AccCardId { get; set; }
        /// <summary>
        /// 绑定次数
        /// </summary>
        public int StoreTimes { get; set; }
        /// <summary>
        /// 实收金额
        /// </summary>
        public decimal ReceiveMoney { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public int PayType { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? Expiredate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 是否发送充次短信
        /// </summary>
        public bool SendMsg { get; set; }
    }
}
