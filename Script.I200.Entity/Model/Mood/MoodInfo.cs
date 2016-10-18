using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.Mood
{
    [Table("tb_user_mood")]
    public class MoodInfo
    {
        /// <summary>
        /// ID
        /// </summary>		
        [Key]
        [Identity]
        public int ID { get; set; }

        /// <summary>
        /// 店铺ID
        /// </summary>		
        public int AccountID { get; set; }

        /// <summary>
        /// 表情图片
        /// </summary>		
        public string MoodPic { get; set; }

        /// <summary>
        /// 心情内容
        /// </summary>		
        public string Mood { get; set; }

        /// <summary>
        /// 时间
        /// </summary>		
        public DateTime Time { get; set; }

    }
}
