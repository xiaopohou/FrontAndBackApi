using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.Coupon
{
    [Table("T_CouponInfo")]
    public class CouponInfo
    {

        /// <summary> 
        /// 优惠券批次ID 
        /// </summary>
        [Key]
        [Identity]
        public int Id { get; set; }


        /// <summary> 
        /// 店铺ID 
        /// </summary>
        public int AccId { get; set; }


        /// <summary> 
        /// 优惠券类别 
        /// </summary>
        public int CouponClass { get; set; }


        /// <summary> 
        /// 优惠券类别 
        /// </summary>
        public int CouponType { get; set; }


        /// <summary> 
        /// 优惠券规则 
        /// </summary>
        public int CouponRule { get; set; }


        /// <summary> 
        /// 优惠券规则Value 
        /// </summary>
        public int CouponRuleVal { get; set; }


        /// <summary> 
        ///  
        /// </summary>
        public int CouponValue { get; set; }


        /// <summary> 
        /// 优惠券批次状态 
        /// </summary>
        public int CouponStatus { get; set; }


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
        /// 操作员ID 
        /// </summary>
        public int OperatorId { get; set; }


        /// <summary> 
        /// 优惠券规则描述 
        /// </summary>
        public string CouponRuleRemark { get; set; }


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
