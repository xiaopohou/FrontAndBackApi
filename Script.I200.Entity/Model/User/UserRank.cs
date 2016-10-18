using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.User
{
    /// <summary>
    ///     店铺用户等级配置
    /// </summary>
    [Table("T_User_Rank")]
    public class UserRank
    {
        /// <summary>
        ///     Id
        /// </summary>
        [Key]
        [Identity]
        public int RankId { get; set; }

        /// <summary>
        ///     店铺Id
        /// </summary>
        public int AccId { get; set; }

        /// <summary>
        ///     排名名称
        /// </summary>
        public string RankName { get; set; }

        /// <summary>
        ///     排名级别
        /// </summary>
        public int RankLv { get; set; }

        /// <summary>
        ///     最低积分
        /// </summary>
        public int IntegralLow { get; set; }

        /// <summary>
        ///     最高积分
        /// </summary>
        public int IntegralHigh { get; set; }

        /// <summary>
        ///     折扣
        /// </summary>
        public double Discount { get; set; }
    }
}