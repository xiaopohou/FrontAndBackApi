using Script.I200.Entity.API;

namespace Script.I200.Entity.Api.UserTimesCard
{
    public class UserTimesCardTransactionRecordResult : PaginationBase<UserTimesCardTransactionRecordResultItem>
    {
        /// <summary>
        /// 合计会员
        /// </summary>
        public int TotalUsers { get; set; }

        /// <summary>
        /// 合计充次
        /// </summary>
        public int TotalInchargeCount { get; set; }

        /// <summary>
        /// 合计充次金额
        /// </summary>
        public decimal TotalInchargeAmount { get; set; }

        /// <summary>
        /// 合计消费次数
        /// </summary>
        public int TotalConsumingCount { get; set; }
    }
}