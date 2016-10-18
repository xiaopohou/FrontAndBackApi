using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.Advert
{
    [Table("T_Advert_Log")]
    public class AdvertLog
    {
        /// <summary>
        /// 编号
        /// </summary>		
        [Key]
        [Identity]
        public int Id { get; set; }
        /// <summary>
        /// 广告编号
        /// </summary>		
        public int AdId { get; set; }
        /// <summary>
        /// 广告资源编号
        /// </summary>		
        public int AdResourcesId { get; set; }
        /// <summary>
        /// 店铺Id
        /// </summary>		
        public int AccountId { get; set; }
        /// <summary>
        /// Ip
        /// </summary>		
        public string Ip { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>		
        [NotMapped]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 日志类型；LOV：0=点击日志（default），1=浏览；
        /// </summary>		
        public int Type { get; set; }
    }
}
