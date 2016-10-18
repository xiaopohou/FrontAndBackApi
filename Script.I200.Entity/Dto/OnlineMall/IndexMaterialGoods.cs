using System.Collections.Generic;
using Script.I200.Entity.Model.Onlinemall;

namespace Script.I200.Entity.Dto.OnlineMall
{
    public class IndexMaterialGoods
    {
        public int Id { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public int GoodsId { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string AliasName { get; set; }
        /// <summary>
        /// 原价
        /// </summary>
        public decimal OriginalPrice { get; set; }
        /// <summary>
        /// 原价题目
        /// </summary>
        public string OriginalPriceText { get; set; }
        /// <summary>
        /// 售价
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 外部标识
        /// </summary>
        public string CoverIcon { get; set; }
        /// <summary>
        /// 外部图片
        /// </summary>
        public List<T_MaterialGoodsPic> CoverImage { get; set; }
        /// <summary>
        /// 状态 0关闭 1开启 2测试 3仅PC开启 4仅移动端开启
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int RankNo { get; set; }
        public int ClassId { get; set; }
    }
}
