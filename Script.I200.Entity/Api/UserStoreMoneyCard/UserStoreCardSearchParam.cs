using Script.I200.Entity.API;

namespace Script.I200.Entity.Api.UserStoreMoneyCard
{
    public class UserStoreCardSearchParam : PaginationParamBase
    {
        /// <summary>
        /// 搜索关键词(根据ES搜索获取到UserId)
        /// </summary>
        public int UserId { get; set; }

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