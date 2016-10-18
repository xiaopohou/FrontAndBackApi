using Script.I200.Entity.API;

namespace Script.I200.Entity.Api.UserTimesCard
{
    /// <summary>
    /// 用户计次卡结果集
    /// </summary>
    public class UserTimesCardSearchParamResult : PaginationBase<UserTimesCardSearchParamResultItem>
    {
        /// <summary>
        /// 合计会员
        /// </summary>
        public int TotalUsers { get; set; }

        /// <summary>
        /// 合计计次卡
        /// </summary>
        public int TotalCards { get; set; }

        /// <summary>
        /// 合计可用次数
        /// </summary>
        public int TotalAvaiable { get; set; }

        /// <summary>
        /// 合计充次金额
        /// </summary>
        public decimal TotalStoreMoney { get; set; }
    }
}