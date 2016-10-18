using Script.I200.Entity.API;

namespace Script.I200.Entity.Api.UserTimesCard
{
    /// <summary>
    ///     用户计次卡查询实体
    /// </summary>
    public class UserTimesCardSearchParam : PaginationParamBase
    {
        /// <summary>
        ///     用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     计次卡Id
        /// </summary>
        public string CardId { get; set; }

        /// <summary>
        ///     0 正常，1 已过期
        /// </summary>
        public int? Status { get; set; }

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