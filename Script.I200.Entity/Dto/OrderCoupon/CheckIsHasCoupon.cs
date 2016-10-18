using System.ComponentModel.DataAnnotations.Schema;

namespace Script.I200.Entity.Dto.OrderCoupon
{
    [Table("T_Order_CouponList")]
    public class CheckIsHasCoupon
    {
        /// <summary>
        /// 优惠券Id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 优惠券批次ID
        /// </summary>
        public int groupId { get; set; }
        /// <summary>
        /// 绑定店铺Id
        /// </summary>
        public int toAccId { get; set; }
        /// <summary>
        /// 优惠券状态
        /// </summary>
        public int couponStatus { get; set; }
        /// <summary>
        /// 优惠券Code
        /// </summary>
        public string couponId { get; set; }
    }
}
