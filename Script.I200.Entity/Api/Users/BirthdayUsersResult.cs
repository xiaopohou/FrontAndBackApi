namespace Script.I200.Entity.Api.Users
{
    /// <summary>
    ///     即将过生日会员的返回结果集
    /// </summary>
    public class BirthdayUsersResult
    {
        /// <summary>
        ///     用户Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///     会员姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        ///     生日
        /// </summary>
        public string Birthday { get; set; }

        /// <summary>
        /// 还剩多少天生日
        /// </summary>
        public int BirthdayDayFromNow { get; set; }

        /// <summary>
        /// 生日月份
        /// </summary>
        public int BdMonth { get; set; }

        /// <summary>
        /// 生日天数
        /// </summary>
        public int BdDay { get; set; }

        /// <summary>
        ///     头像地址
        /// </summary>
        public string PortraitUrl { get; set; }

        /// <summary>
        ///     是否阴历
        /// </summary>
        public bool IsLunar { get; set; }
    }
}