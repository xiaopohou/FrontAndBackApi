using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.User
{
    [Table("T_Account_User")]
    public class AccountUser
    {
        [Key]
        [Identity]
        public int Id { get; set; }

        /// <summary>
        ///     账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        ///     密码
        /// </summary>
        [Column("passwd")]
        public string Password { get; set; }

        /// <summary>
        ///     姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     性别
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        ///     店铺Id
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// 总店Id
        /// </summary>
        [NotMapped]
        public int MasterId { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        [NotMapped]
        public string CompanyName { get; set; }

        /// <summary>
        ///     角色（管理员、收银员）
        /// </summary>
        public string Grade { get; set; }

        public int Integral { get; set; }
        public int Recharge { get; set; }

        [Column("dele_user")]
        public int DeleUser { get; set; }

        [Column("dele_sell")]
        public int DeleSell { get; set; }

        [Column("zc_insert")]
        public int ZcInsert { get; set; }

        [Column("shop_insert")]
        public int ShopInsert { get; set; }

        [Column("shop_class")]
        public int ShopClass { get; set; }

        [Column("regtime")]
        public DateTime Regtime { get; set; }

        public string PhoneNumber { get; set; }
        public string UserEmail { get; set; }
        public string Perssion { get; set; }
        public string PhoneConty { get; set; }
        public string PhoneCity { get; set; }
        public string TrueConty { get; set; }
        public string TrueCity { get; set; }
        public int UserPower { get; set; }
    }
}