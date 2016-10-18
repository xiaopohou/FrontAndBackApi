namespace Script.I200.Entity.Enum
{
    public class CouponEnum
    {
        /// <summary>
        /// 优惠券类别couponClass
        /// </summary>
        public enum CouponClass : int
        {
            系统类别 = 1,
            店铺类别 = 2
        }

        /// <summary>
        /// 优惠券类型couponType
        /// </summary>
        public enum CouponType : int
        {
            优惠券类型1 = 1,
            优惠券类型2 = 2
        }

        /// <summary>
        /// 优惠券规则couponRule
        /// </summary>
        public enum CouponRule : int
        {
            无限制 = 1,
            限定购买金额 = 2
        }

        /// <summary>
        /// 优惠券概要状态
        /// </summary>
        public enum CouponInfoStatus : int
        {
            正常 = 0,
            已过期 = 1,
            已作废 = 3
        }

        /// <summary>
        /// 优惠券使用状态
        /// </summary>
        public enum CouponListStatus : int
        {
            未使用 = 0,
            已经使用 = 1,
            已绑定 = 2,
            已作废 = 3
        }
    }
}
