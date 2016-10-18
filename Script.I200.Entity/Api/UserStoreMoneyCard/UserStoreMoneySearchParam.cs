using System;
using Script.I200.Entity.API;

namespace Script.I200.Entity.Api.UserStoreMoneyCard
{
    /// <summary>
    /// 用户储值历史查询实体
    /// </summary>
    public class UserStoreMoneySearchParam : PaginationParamBase
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 会员Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 记录类型(1.充值 2.消费)
        /// </summary>
        public int? OperateType { get; set; }
    }
}