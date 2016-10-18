using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.Coupon
{
    [Table("T_CouponList")]
    public class CouponList
    {

        /// <summary> 
        /// 优惠券详细列表ID 
        /// </summary>
        [Key]
        [Identity]
        public int Id { get; set; }


        /// <summary> 
        /// 优惠券批次ID 
        /// </summary>
        public int GroupId { get; set; }


        /// <summary> 
        /// 店铺ID 
        /// </summary>
        public int AccId { get; set; }


        /// <summary> 
        /// 优惠券Code 
        /// </summary>
        public string CouponId { get; set; }


        /// <summary> 
        /// 优惠内容Value 
        /// </summary>
        public int CouponValue { get; set; }


        /// <summary> 
        /// 优惠券状态 
        /// </summary>
        public int CouponStatus { get; set; }


        /// <summary> 
        /// 创建日期 
        /// </summary>
        public DateTime CreateDate { get; set; }


        /// <summary> 
        /// 过期日期 
        /// </summary>
        public DateTime EndDate { get; set; }


        /// <summary> 
        /// 使用日期 
        /// </summary>
        public DateTime UsedDate { get; set; }


        /// <summary> 
        /// 绑定会员ID 
        /// </summary>
        public int ToUserId { get; set; }


        /// <summary> 
        /// 使用会员ID 
        /// </summary>
        public int UseUserId { get; set; }


        /// <summary> 
        /// 附加信息 
        /// </summary>
        public string Flag { get; set; }


        /// <summary> 
        /// 绑定会员名称 
        /// </summary>
        public string ToUserName { get; set; }


        /// <summary> 
        /// 使用会员名称 
        /// </summary>
        public string UseUserName { get; set; }


        /// <summary> 
        /// 领取时间 
        /// </summary>
        public DateTime ReceiveDate { get; set; }

    }
}








