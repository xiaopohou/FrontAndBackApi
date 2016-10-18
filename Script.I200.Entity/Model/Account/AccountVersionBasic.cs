using System;

namespace Script.I200.Entity.Model.Account
{
    public class AccountVersionBasic
    {
        /// <summary>
        /// 店铺版本
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// 试用版版本
        /// </summary>
        public int BetaAdvance { get; set; }

        /// <summary>
        /// 行业版名称
        /// </summary>
        public string IndustryName { get; set; }
        /// <summary>
        /// 版本过期时间
        /// </summary>
        public DateTime? VersionExpirationTime { get; set; }
    }
}
