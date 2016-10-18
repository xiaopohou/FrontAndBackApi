using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Script.I200.Entity.Model.UserStoreCard
{
    [Table("T_User_StoreMoneyCards")]
    public class UserStoreCards
    {
        /// <summary>
        ///     Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     储值卡Id
        /// </summary>
        public int CardId { get; set; }

        /// <summary>
        ///     店铺Id
        /// </summary>
        [Key]
        public int AccId { get; set; }

        /// <summary>
        ///     用户Id
        /// </summary>
        [Key]
        public int UserId { get; set; }

        /// <summary>
        ///     会员姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        ///     储值卡名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     余额
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        ///     总充值金额
        /// </summary>
        public decimal TotalStoreMoney { get; set; }

        /// <summary>
        ///     总赠送金额
        /// </summary>
        public decimal TotalExtraMoney { get; set; }

        /// <summary>
        ///     创建人Id
        /// </summary>
        public int Creator { get; set; }

        /// <summary>
        ///     交易时间
        /// </summary>
        public DateTime CreateAt { get; set; }
    }
}