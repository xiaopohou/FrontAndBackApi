namespace Script.I200.Entity.Enum
{
    public static class OrderCouponEnum
    {
        /// <summary>
        /// 优惠券类别CouponType
        /// </summary>
        public enum CouponType : int
        {
            抵值券 = 1,
            功能券 = 2
        }

        /// <summary>
        /// 绑定类型BindType
        /// </summary>
        public enum BindType : int
        {
            无限制类型 = 0,
            绑定类型 = 1,
            绑定产品 = 2
        }

        /// <summary>
        /// 限制类型RuleType
        /// </summary>
        public enum RuleType : int
        {
            无限制金额 = 0,
            满减抵值 = 1
        }

        /// <summary>
        /// 优惠券使用状态
        /// 尚未使用 = 0,
        /// 已经使用 = 1,
        /// 已绑定 = 2,
        /// 已作废 = 3,
        /// 已过期 = -1
        /// </summary>
        public enum CouponStatus : int
        {
            /// <summary>
            /// 尚未使用
            /// </summary>
            NotYetUsed = 0,
            /// <summary>
            /// 已经使用
            /// </summary>
            Used = 1,
            /// <summary>
            /// 已绑定
            /// </summary>
            Bound = 2,
            /// <summary>
            /// 已作废
            /// </summary>
            Cancelled = 3,
            /// <summary>
            /// 已过期
            /// </summary>
            Expired = -1
        }

        /// <summary>
        /// 绑定途径
        /// 平台绑定 = 1,
        /// 用户绑定 = 2,
        /// 积分兑换 = 3
        /// </summary>
        public enum CouponBindWay : int
        {
            /// <summary>
            /// 平台绑定
            /// </summary>
            ServerBind = 1,
            /// <summary>
            /// 用户绑定
            /// </summary>
            UserBind = 2,
            /// <summary>
            /// 积分兑换
            /// </summary>
            IntegralBind = 3
        }
    }
}
