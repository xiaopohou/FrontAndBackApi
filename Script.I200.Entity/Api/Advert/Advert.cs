using System.Collections.Generic;

namespace Script.I200.Entity.Api.Advert
{
    public class Advert
    {
        /// <summary>
        /// 广告标题
        /// </summary>		
        public string title { get; set; }
        /// <summary>
        /// 展示方式；LOV：0=单一图片，1=多图轮询；
        /// </summary>		
        public int display { get; set; }
        /// <summary>
        /// 广告尺寸:宽度px
        /// </summary>		
        public int width { get; set; }
        /// <summary>
        /// 广告尺寸：高度px
        /// </summary>		
        public int height { get; set; }
        /// <summary>
        /// 广告资源
        /// </summary>
        public List<AdvertResources> items { get; set; }
    }
}
