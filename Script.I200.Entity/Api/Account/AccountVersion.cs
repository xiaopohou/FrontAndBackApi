using System;

namespace Script.I200.Entity.Api.Account
{
    public class AccountVersion
    {
        /// <summary>
        /// 店铺版本
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// 版本名称
        /// </summary>
        public string VersionName { get; set; }
        /// <summary>
        /// 版本子名称
        /// </summary>
        public string SubVersionName { get; set; }
        /// <summary>
        /// 版本过期时间
        /// </summary>
        public DateTime? VersionExpirationTime { get; set; }
    }
}
