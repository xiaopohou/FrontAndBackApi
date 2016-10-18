using System.ComponentModel;

namespace Script.I200.Entity
{
    public enum AccountVersionEnum
    {
        /// <summary>
        /// 免费版
        /// </summary>
        [Description("免费版")]
        Basic = 1,

        /// <summary>
        /// 高级版
        /// </summary>
        [Description("高级版")]
        Advanced = 3,


        /// <summary>
        /// 行业版
        /// </summary>
        [Description("行业版")]
        Industry = 5
    }
}
