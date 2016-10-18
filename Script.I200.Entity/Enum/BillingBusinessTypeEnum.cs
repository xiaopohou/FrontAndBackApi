namespace Script.I200.Entity.Enum
{
    public enum BillingBusinessTypeEnum : int
    {
        WechatGathering = 2,
        MobileShop = 4,
    }

    public class BillingBusinessType
    {
        public static string GetBusinessName(BillingBusinessTypeEnum businessType)
        {
            switch ((int) businessType)
            {
                case 2:
                    return "微信收款";
                case 4:
                    return "手机橱窗";
                default:
                    return "错误类型";
            }
        }
    }
}