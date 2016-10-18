using Script.I200.Entity.API;

namespace Script.I200.Entity.Api.UserTimesCard
{
    /// <summary>
    ///     店铺计次卡
    /// </summary>
    public class AccountTimesCardSearchParamResult : PaginationBase<AccountTimesCardSearchParamResultItem>
    {
        /// <summary>
        ///     总计卡的种类
        /// </summary>
        public int TotalTimesCardTypes { get; set; }
    }
}