using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.User
{
    [Table("T_User_Birthday")]
    public class UserBirthday
    {
        /// <summary>
        ///     生日Id
        /// </summary>
        [Identity]
        [Key]
        public int BirthdayId { get; set; }

        /// <summary>
        ///     店铺Id
        /// </summary>
        public int AccId { get; set; }

        /// <summary>
        ///     用户Id
        /// </summary>
        public int UId { get; set; }

        /// <summary>
        ///     公历/阴历（1代表公历，2代表阴历）
        /// </summary>
        public int IsLunar { get; set; }

        /// <summary>
        ///     名称
        /// </summary>
        public string BdName { get; set; }

        /// <summary>
        ///     生日年份
        /// </summary>
        public int BdYear { get; set; }

        /// <summary>
        ///     生日月份
        /// </summary>
        public int BdMonth { get; set; }

        /// <summary>
        ///     生日天数
        /// </summary>
        public int BdDay { get; set; }

        /// <summary>
        ///     更新时间
        /// </summary>
        public DateTime EditTime { get; set; }
    }
}