using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.Account
{
    /// <summary>
    /// 店铺Logo
    /// </summary>
    [Table("t_Logo")]
    public class AccountLogo
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Key]
        [Identity]
        public int Id { get; set; }

        /// <summary>
        /// 店铺ID
        /// </summary>
        public int ShopperId { get; set; }

        /// <summary>
        /// 店铺Logo路径
        /// </summary>
        public string ImgUrl { get; set; }
    }
}
