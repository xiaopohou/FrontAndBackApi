using System;

namespace Script.I200.Entity.Dto.OrderCoupon
{
    public class MaterialCouponInfo
    {
        /// <summary>
        /// 优惠券组ID
        /// </summary>
        public int GroupId { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public int CouponMoney { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public int EndTimeSeconds { get; set; }
        /// <summary>
        /// 优惠券结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 是否可领取
        /// </summary>
        public bool IsCanReceive { get; set; }
    }
}
