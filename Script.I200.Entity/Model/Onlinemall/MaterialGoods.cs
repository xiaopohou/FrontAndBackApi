using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.Onlinemall
{
    [Table("T_MaterialGoods2")]
    public class T_MaterialGoods2
    {
        /// <summary> 
        ///   
        /// </summary> 
        [Key]
        [Identity]
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
        /// 外部图片  
        /// </summary> 
        public string CoverImage { get; set; }

        /// <summary> 
        /// 外部标识  
        /// </summary> 
        public string CoverIcon { get; set; }

        /// <summary> 
        /// 头部循环图片  
        /// </summary> 
        public string HeadThumb { get; set; }

        /// <summary> 
        /// 详情（暂时为图片）  
        /// </summary> 
        public string Description { get; set; }

        /// <summary> 
        /// 规格参数  
        /// </summary> 
        public string Specification { get; set; }

        /// <summary> 
        /// 使用帮助  
        /// </summary> 
        public string Manual { get; set; }

        /// <summary> 
        /// 状态0关闭1开启2测试  
        /// </summary> 
        public int Status { get; set; }

        /// <summary> 
        /// 排序  
        /// </summary> 
        public int RankNo { get; set; }

        /// <summary> 
        /// 添加时间  
        /// </summary> 
        public DateTime InsertTime { get; set; }

        /// <summary> 
        /// 修改时间  
        /// </summary> 
        public DateTime UpdateTime { get; set; }

        /// <summary> 
        /// 操作IP  
        /// </summary> 
        public string OperatorIp { get; set; }

        /// <summary> 
        /// 操作ID  
        /// </summary> 
        public string OperatorId { get; set; }
        /// <summary>
        /// 分类ID
        /// </summary>
        public int ClassId { get; set; }
    }
    
}
