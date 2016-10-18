using System;
using Script.I200.Entity.API;

namespace Script.I200.Entity.Api.UserTimesCard
{
    public class UserTimesCardTransactionRecordSearchParam : PaginationParamBase
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 操作类型 （1：充次 2：消费）
        /// </summary>
        public int? OperateType { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}