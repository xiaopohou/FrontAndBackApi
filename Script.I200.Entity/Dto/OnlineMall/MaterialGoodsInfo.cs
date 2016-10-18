using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Entity.Model.Onlinemall;

namespace Script.I200.Entity.Dto.OnlineMall
{
    /// <summary>
    /// 硬件全部信息
    /// </summary>
    [Table("T_MaterialGoods2")]
    public class MaterialGoodsInfo
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
        /// 商品小标题
        /// </summary>
        public string SubTitle { get; set; }
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
        /// 头部循环图片
        /// </summary>
        public List<T_MaterialGoodsPic> HeadThumb { get; set; }
        /// <summary>
        /// 详情（暂时为图片）
        /// </summary>
        public List<T_MaterialGoodsPic> Description { get; set; }
        /// <summary>
        /// 移动端详情（暂时为图片）
        /// </summary>
        public List<T_MaterialGoodsPic> MobileDescription { get; set; }
        /// <summary>
        /// 使用帮助
        /// </summary>
        public string Manual { get; set; }
        /// <summary>
        /// 规格参数
        /// </summary>
        public string Specification { get; set; }
        /// <summary>
        /// 状态 0关闭 1开启 2测试 3仅PC开启 4仅移动端开启
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int RankNo { get; set; }
        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime InsertTime { get; set; }
        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        public string OperatorId { get; set; }
        public string OperatorIp { get; set; }
        public int ClassId { get; set; }
    }
}
