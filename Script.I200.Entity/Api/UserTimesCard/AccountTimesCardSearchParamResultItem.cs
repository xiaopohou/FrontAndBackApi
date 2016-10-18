namespace Script.I200.Entity.Api.UserTimesCard
{
    public class AccountTimesCardSearchParamResultItem
    {
        /// <summary>
        ///     计次卡Id
        /// </summary>
        public string CardId { get; set; }

        /// <summary>
        ///     计次卡名称
        /// </summary>
        public string CardName { get; set; }

        /// <summary>
        ///     绑定服务商品Id
        /// </summary>
        public int BindGoodsId { get; set; }

        /// <summary>
        ///     服务项目
        /// </summary>
        public string BindGoodsName { get; set; }

        /// <summary>
        ///     总计发卡数量
        /// </summary>
        public int TotalUserCards { get; set; }

        /// <summary>
        ///     总计可使用计次卡数量
        /// </summary>
        public int TotalAvailableCards { get; set; }
    }
}