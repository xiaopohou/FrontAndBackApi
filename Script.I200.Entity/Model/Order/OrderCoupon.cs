using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.Order
{
    [Table("T_Order_CouponList")]
    public class T_Order_CouponList
    {
        [Key]
        [Identity]
        /// <summary>
        /// 优惠券Id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 优惠券批次ID
        /// </summary>
        public int groupId { get; set; }

        /// <summary>
        /// 优惠券Code
        /// </summary>
        public string couponId { get; set; }

        /// <summary>
        /// 优惠券Val
        /// </summary>
        public int couponValue { get; set; }

        /// <summary>
        /// 优惠券状态
        /// </summary>
        public int couponStatus { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime createDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime endDate { get; set; }

        /// <summary>
        /// 绑定日期
        /// </summary>
        public DateTime receiveDate { get; set; }

        /// <summary>
        /// 使用日期
        /// </summary>
        public DateTime? usedDate { get; set; }

        /// <summary>
        /// 绑定店铺Id
        /// </summary>
        public int toAccId { get; set; }

        /// <summary>
        /// 使用店铺Id
        /// </summary>
        public int useAccId { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 绑定方式
        /// </summary>
        public int bindWay { get; set; }

        /// <summary>
        /// 附加信息
        /// </summary>
        public string flag { get; set; }
    }
    
}
