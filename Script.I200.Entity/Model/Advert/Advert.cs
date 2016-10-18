using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Script.I200.Entity.Model.Advert
{
    [Table("T_Advert")]
    public class Advert
    {
        /// <summary>
        /// 编号
        /// </summary>		
        public int Id { get; set; }
        /// <summary>
        /// 广告标题
        /// </summary>		
        public string AdTitle { get; set; }
        /// <summary>
        /// 广告投放范围；LOV：-1=全平台；0=免费店铺，1=付费店铺，2=高级版，3{001}=行业版{行业编号}，1000=自定义（关联表：T_Advert_Range）；
        /// </summary>		
        public int AdRangeType { get; set; }
         [NotMapped]
        public string AdRangeTypeName { get; set; }
        /// <summary>
        /// 广告描述
        /// </summary>		
        public string AdDes { get; set; }
        /// <summary>
        /// 广告位置代号；用于定位某一固定位置广告，如商城顶部广告：mallTopBanner
        /// </summary>		
        public string AdPositionCode { get; set; }
        /// <summary>
        /// 广告位置说明
        /// </summary>		
        public string AdPosition { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>		
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>		
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 是否启用；LOV：0=关闭（default），1=启用；
        /// </summary>
        public int Enable { get; set; }
        [NotMapped]
        public string EnableName { get; set; }
        /// <summary>
        /// 展示方式；LOV：0=单一图片，1=多图轮询；
        /// </summary>		
        public int DisplayMode { get; set; }
         [NotMapped]
        public string DisplayModeName { get; set; }
        /// <summary>
        /// 展示终端；LOV：0=web，1=IOS,2=Android,3=iPAD；
        /// </summary>		
        public int DisplayTerminal { get; set; }
         [NotMapped]
        public string DisplayTerminalName { get; set; }
        /// <summary>
        /// 广告尺寸:宽度px
        /// </summary>		
        public int AdWidth { get; set; }
        /// <summary>
        /// 广告尺寸：高度px
        /// </summary>		
        public int AdHieght { get; set; }
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
