namespace Script.I200.Entity.Dto.OnlineMall
{
    public class MaterialGoodsListRedisItem
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public int GoodsId { get; set; }
        /// <summary>
        /// 状态 0关闭 1开启 2测试 3仅PC开启 4仅移动端开启
        /// </summary>
        public int Status { get; set; }
    }
}
