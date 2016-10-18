using System.ComponentModel;

namespace Script.I200.Entity.Enum
{
    public enum BetaAdvanceEnum
    {
        /// <summary>
        /// 没有使用高级版试用版
        /// </summary>
        NoUseBetaAdvance = 0,
        /// <summary>
        /// 正在使用高级版试用版
        /// </summary>
        [Description("试用高级版")]
        UsingBetaAdvance = 1,
        /// <summary>
        /// 已使用高级版试用版
        /// </summary>
        UsedBetaAdvance = 2,
        /// <summary>
        /// 直接购买高级版没有使用高级版试用版
        /// </summary>
        UseAdvanceNoUseBetaAdvance = 3,
        /// <summary>
        /// 使用高级版试用版过程中购买行业版或高级版
        /// </summary>
        UseAdvanceUsingBetaAdvance = 4
    }
}
