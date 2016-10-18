using System.ComponentModel;

namespace Script.I200.Entity.Enum
{
    /// <summary>
    ///     店铺短信模板枚举
    /// </summary>
    public enum AccountSmsTeampltesEnum
    {
        /// <summary>
        ///     充值感谢
        /// </summary>
        [Description("充值提醒")] RechargeThanks = 1,

        /// <summary>
        ///     储值消费感谢
        /// </summary>
        [Description("储值电子账单")] StoreConsumptionThanks = 2,

        /// <summary>
        ///     消费感谢
        /// </summary>
        [Description("电子账单")] ConsumptionThanks = 3,

        /// <summary>
        ///     今日营业汇报
        /// </summary>
        [Description("店铺经营")] BusinessReportToday = 4,

        /// <summary>
        ///     新会员欢迎
        /// </summary>
        [Description("会员注册")] WelcomeNewUser = 5,

        /// <summary>
        ///     计次消费感谢
        /// </summary>
        [Description("计次电子账单")] TimesCardConsumptionThanks = 6,

        /// <summary>
        ///     生日祝福
        /// </summary>
        [Description("生日祝福")] HappyBirthday = 7,

        /// <summary>
        ///     销售后记录会员
        /// </summary>
        [Description("销售后会员")] SaleRecordUser = 8,


        /// <summary>
        ///     赠送优惠券
        /// </summary>
        GiveCoupon = 9,

        /// <summary>
        ///     计次卡充值
        /// </summary>
        [Description("计次卡充值")] TimesCardStore = 10
    }
}