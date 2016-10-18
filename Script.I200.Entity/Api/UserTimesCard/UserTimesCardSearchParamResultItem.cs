using System;

namespace Script.I200.Entity.Api.UserTimesCard
{
    /// <summary>
    /// 用户计次卡结果项
    /// </summary>
    public class UserTimesCardSearchParamResultItem
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 卡名称
        /// </summary>
        public string CardName { get; set; }

        /// <summary>
        /// 用户计次卡Id
        /// </summary>
        public int UserCardId { get; set; }

        /// <summary>
        /// 店铺计次卡Id
        /// </summary>
        public int AccCardId { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? ExpireDate { get; set; }

        /// <summary>
        /// 剩余次数
        /// </summary>
        public int LeaveTimes { get; set; }

        /// <summary>
        /// 绑定服务商品Id
        /// </summary>
        public int BindGoodsId { get; set; }

        /// <summary>
        /// 绑定服务商品名称
        /// </summary>
        public string BindGoodsName { get; set; }

        /// <summary>
        /// 用户计次卡状态
        /// </summary>
        public int Status { get; set; }
    }
}