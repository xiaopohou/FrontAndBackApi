using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.Onlinemall
{
    [Table("T_MaterialGoodsPic")]
    public class T_MaterialGoodsPic
    {
        /// <summary> 
        ///   
        /// </summary> 
        [Key]
        [Identity]
        public int Id { get; set; }

        /// <summary> 
        ///   
        /// </summary> 
        public int GoodsId { get; set; }

        /// <summary> 
        /// 图片地址  
        /// </summary> 
        public string PicUrl { get; set; }

        /// <summary> 
        /// 排序  
        /// </summary> 
        public int RankNo { get; set; }

        /// <summary> 
        /// 0显示1隐藏  
        /// </summary> 
        public int Status { get; set; }

        /// <summary> 
        /// 类别0头部图片1详情图片2封面图片
        /// </summary> 
        public int Type { get; set; }

        /// <summary> 
        /// 插入时间  
        /// </summary> 
        public DateTime InsertTime { get; set; }

        /// <summary> 
        /// 操作人ID  
        /// </summary> 
        public string OperatorId { get; set; }

        /// <summary> 
        /// 操作人IP  
        /// </summary> 
        public string OperatorIp { get; set; }

    }
}
