using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.TimesCard
{
    /// <summary>
    /// 用户计次卡表
    /// </summary>
    [Table("T_User_StoreTimes")]
    public class UserStoreTimes
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        [Identity]
        public int StId { get; set; }

        /// <summary>
        /// 店铺Id
        /// </summary>
        public int AccId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [Column("uid")]
        public int UserId { get; set; }

        /// <summary>
        /// 会员名称
        /// </summary>
        [NotMapped]
        public string UserName { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [NotMapped]
        public string Phone { get; set; }

        /// <summary>
        /// 充值次数
        /// </summary>
        [RegularExpression(@"^\+?[1-9][0-9]*$", ErrorMessage = "充值次数只能填写大于0的整数")]
        public int StoreTimes { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [Range(typeof(decimal), "0.00", "999999.99", ErrorMessage = "请输入小于100万的金额")]
        [Column("StoreMoney")]
        public decimal ReceiveMoney { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        [NotMapped]
        public int PayType { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime EditTime { get; set; }

        /// <summary>
        /// 计次卡名称
        /// </summary>
        public string CardName { get; set; }

        /// <summary>
        /// 绑定商品Id
        /// </summary>
        public int BindGoodsId { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        [Column("closeDate")]
        public DateTime? ExpireDate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(maximumLength: 100, ErrorMessage = "备注的长度必须小于等于100个字符")]
        [NotMapped]
        public string Remark { get; set; }

        /// <summary>
        /// 是否发送短信
        /// </summary>
        [NotMapped]
        public bool SendMsg { get; set; }

        /// <summary>
        ///  店铺计次卡Id
        /// </summary>
        public int AccTimesCardId { get; set; }
    }
}