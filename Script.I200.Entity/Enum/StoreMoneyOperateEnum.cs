using System.ComponentModel;

namespace Script.I200.Entity.Enum
{
    public enum StoreMoneyOperateEnum
    {
        /// <summary>
        ///     充值
        /// </summary>
        [Description("充值")] Recharge = 1,

        /// <summary>
        ///     消费
        /// </summary>
        [Description("消费")] Consumption = 2
    }
}