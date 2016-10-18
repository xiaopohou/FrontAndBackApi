using System;

namespace Script.I200.Entity.Api.Coupon
{
    public class UserCouponModel
    {
        /// <summary>
        /// 优惠券ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 优惠券概要ID
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// 店铺ID
        /// </summary>
        public int AccId { get; set; }

        /// <summary>
        /// 优惠券编码
        /// </summary>
        public string CouponId { get; set; }

        /// <summary>
        /// 优惠券Value
        /// </summary>
        public int CouponValue { get; set; }

        /// <summary>
        /// 优惠券状态
        /// </summary>
        public int CouponStatus { get; set; }

        /// <summary>
        /// 优惠券状态Desc
        /// </summary>
        public string CouponStatusDesc { get; set; }

        /// <summary>
        /// 优惠券生成日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 优惠券截止日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 优惠券使用时间
        /// </summary>
        public DateTime? UseDate { get; set; }

        /// <summary>
        /// 优惠券绑定会员ID
        /// </summary>
        public int ToUserId { get; set; }

        /// <summary>
        /// 优惠券绑定会员Name
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// 优惠券使用会员ID
        /// </summary>
        public int UseUserId { get; set; }

        /// <summary>
        /// 优惠券使用会员Name
        /// </summary>
        public string UseUserName { get; set; }

        /// <summary>
        /// 附加信息
        /// </summary>
        public string Flag { get; set; }

        /// <summary>
        /// 优惠券类型
        /// </summary>
        public string CouponClass { get; set; }

        /// <summary>
        /// 优惠券类别
        /// </summary>
        public string CouponType { get; set; }

        /// <summary>
        /// 优惠券规则Id
        /// </summary>
        public int CouponRuleId { get; set; }

        /// <summary>
        /// 优惠券规则描述
        /// </summary>
        public string CouponRuleDesc { get; set; }

        /// <summary>
        /// 优惠券规则描述
        /// </summary>
        public string CouponRuleRemark { get; set; }

        /// <summary>
        /// 优惠券规则Value
        /// </summary>
        public int CouponRuleVal { get; set; }

        /// <summary>
        /// 优惠券状态
        /// </summary>
        public int CouponInfoStatus { get; set; }

        /// <summary>
        /// 优惠券状态描述
        /// </summary>
        public string CouponInfoStatusDesc { get; set; }

        /// <summary>
        /// 优惠券描述
        /// </summary>
        public string CouponDesc { get; set; }

        /// <summary>
        /// 跨店使用 0-支持 1-禁用
        /// </summary>
        public int CrossShop { get; set; }
    }
}
