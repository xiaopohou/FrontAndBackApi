using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.Mood
{
    [Table("T_LogSign")]
    public class LogSign
    {
        /// <summary>
        /// id
        /// </summary>		
        [Key]
        [Identity]
        public int id { get; set; }

        /// <summary>
        /// 店铺ID
        /// </summary>		
        public string accountID { get; set; }

        /// <summary>
        /// 签到时间
        /// </summary>		
        public DateTime CreatTime { get; set; }

        /// <summary>
        /// 签到类型
        /// </summary>		
        public int SignType { get; set; }

        /// <summary>
        /// 连续签到天数
        /// </summary>		
        public int SerialDay { get; set; }

        /// <summary>
        /// 签到增加会员空间
        /// </summary>		
        public int Add_Storage { get; set; }

        /// <summary>
        /// 签到增加短信条数
        /// </summary>		
        public int Add_Sms { get; set; }

        /// <summary>
        /// 签到增加店铺积分
        /// </summary>		
        public int Add_Integral { get; set; }

    }
}
