using System;

namespace Script.I200.Entity.Api.Users
{
    /// <summary>
    /// 积分明细
    /// </summary>
    public class UserIntegralDetail
    {
        /// <summary>
        /// 获取时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 积分类型
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 变化积分
        /// </summary>
        public int EditIntegral { get; set; }

        /// <summary>
        /// 最终积分
        /// </summary>
        public int FinalIntegral { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string OperatorID { get; set; }
    }
}
