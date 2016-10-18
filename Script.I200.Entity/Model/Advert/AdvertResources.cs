using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Script.I200.Entity.Model.Advert
{
    [Table("T_Advert_Resources")]
    public class AdvertResources
    {
        /// <summary>
        /// 编号
        /// </summary>		
        public int Id { get; set; }
        /// <summary>
        /// 广告编号
        /// </summary>		
        public int AdId { get; set; }
        /// <summary>
        /// 广告链接
        /// </summary>		
        public string AdLink { get; set; }
        /// <summary>
        /// 广告图片
        /// </summary>		
        public string AdImageUrl { get; set; }
        /// <summary>
        /// 广告资源有效性；LOV：0=关闭（default），1=有效；
        /// </summary>		
        public int State { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>		
        public DateTime UpdateTime { get; set; }
    }
}
