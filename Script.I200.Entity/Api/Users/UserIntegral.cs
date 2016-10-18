using System.Collections.Generic;

namespace Script.I200.Entity.Api.Users
{
    public class UserIntegral
    {
        /// <summary>
        /// 累计积分
        /// </summary>
        public int TotalIntegral { get; set; }

        /// <summary>
        /// 有效积分
        /// </summary>
        public int AvailableIntegral { get; set; }

        /// <summary>
        /// 已兑换积分
        /// </summary>
        public int UnAvailableIntegral { get; set; }

        /// <summary>
        /// 积分明细
        /// </summary>
        public List<UserIntegralDetail> Items { get; set; }
    }

    
}
