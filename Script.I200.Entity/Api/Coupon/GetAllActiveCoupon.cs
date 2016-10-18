namespace Script.I200.Entity.Api.Coupon
{
    public class GetAllActiveCoupon
    {
        /// <summary>
        /// 店铺ID
        /// </summary>
        public int AccId { get; set; }
        /// <summary>
        /// 优惠内容Value
        /// </summary>
        public int CouponValue { get; set; }
        /// <summary>
        /// 优惠券状态
        /// </summary>
        public int CouponStatus { get; set; }
        /// <summary>
        /// 优惠券描述
        /// </summary>
        public string CouponDesc { get; set; }
        /// <summary>
        /// 剩余可用数
        /// </summary>
        public int BalanceCount { get; set; }
    }
}
