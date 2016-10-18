using Script.I200.Entity.API;

namespace Script.I200.Entity.Api.UserTimesCard
{
    /// <summary>
    /// 店铺计次卡筛选请求参数
    /// </summary>
    public class AccountTimeCardsSearchParam : PaginationParamBase
    {
        /// <summary>
        /// 计次卡名称
        /// </summary>
        public string CardName { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string Rank { get; set; }

        /// <summary>
        /// 正序或者逆序（Asc 、Desc）
        /// </summary>
        public string Sort { get; set; }
    }
}
