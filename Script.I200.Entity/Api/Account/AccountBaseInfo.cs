namespace Script.I200.Entity.Api.Account
{
    public class AccountBaseInfo
    {
        /// <summary>
        /// 店铺ID
        /// </summary>
        public long accountId { get;set; }

        /// <summary>
        /// 店铺名称
        /// </summary>
        public string accountName { get; set; }

        /// <summary>
        /// 店铺联系电话
        /// </summary>
        public string accountContactPhone { get; set; }

        /// <summary>
        /// 店铺版本 1=免费版，3=高级版，5=行业版
        /// </summary>
        public int accountLicense { get; set; }

        /// <summary>
        /// 店铺版本名称
        /// </summary>

        public string accountLicenseName { get; set; }

        /// <summary>
        /// 店铺连锁 0=非连锁，1=连锁
        /// </summary>
        public int accountEnterprise { get; set; }

        /// <summary>
        /// 店铺Logo
        /// </summary>
        public string accountLogo { get; set; }

         /// <summary>
        /// 店铺Avatar
        /// </summary>
        public string accountAvatar { get; set; }

        /// <summary>
        /// 店铺积分
        /// </summary>
        public int accountIntegral { get; set; }

        /// <summary>
        /// 店铺今日积分
        /// </summary>
        public int accountIntegralToday { get; set; }

        /// <summary>
        /// 店铺有效礼金券
        /// </summary>
        public int accountActiveCoupon { get; set; }

        /// <summary>
        /// 店铺资金账户余额
        /// </summary>
        public decimal accountTotalMoney { get; set; }

        /// <summary>
        /// 店铺当前登录用户
        /// </summary>
        public accountLoginUser accountLoginUser { get; set; }

    }

    public class accountLoginUser
    {
        /// <summary>
        /// 店铺登录用户Id
        /// </summary>
        public int userId { get; set; }

        /// <summary>
        /// 店铺登录用户名称
        /// </summary>
        public string userName { get; set; }

        /// <summary>
        /// 店铺登录用户权限 1=管理员，2=店员
        /// </summary>
        public int role { get; set; }
    }
}
