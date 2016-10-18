using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Script.I200.Entity.Api.Coupon
{
    /// <summary>
    /// 优惠券概要信息Item
    /// </summary>
    public class CouponInfoItem
    {
        /// <summary>
        /// 优惠券批次ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 店铺ID
        /// </summary>
        public int AccId { get; set; }

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
        /// 优惠内容Value
        /// </summary>
        public int CouponValue { get; set; }

        /// <summary>
        /// 优惠券状态
        /// </summary>
        public int CouponStatus { get; set; }

        /// <summary>
        /// 优惠券状态描述
        /// </summary>
        public string CouponStatusDesc { get; set; }

        /// <summary>
        /// 优惠券描述
        /// </summary>
        public string CouponDesc { get; set; }

        /// <summary>
        /// 限制最大数量
        /// </summary>
        public int MaxLimitNum { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 截至日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 创建人员ID
        /// </summary>
        public int OperatorId { get; set; }

        /// <summary>
        /// 创建人员姓名
        /// </summary>
        public string OperatorName { get; set; }

        /// <summary>
        /// 优惠码数量
        /// </summary>
        public int CouponListCount { get; set; }

        /// <summary>
        /// 剩余可用数
        /// </summary>
        public int BalanceCount { get; set; }

        /// <summary>
        /// 原始链接
        /// </summary>
        public string LongUrl { get; set; }

        /// <summary>
        /// 短链接
        /// </summary>
        public string ShortUrl { get; set; }

        /// <summary>
        /// 是否支持跨店 0-否 1-是
        /// </summary>
        public int CrossShop { get; set; }

    }
}
