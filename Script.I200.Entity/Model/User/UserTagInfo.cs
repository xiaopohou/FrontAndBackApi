using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.User
{
    /// <summary>
    /// 会员标签基础信息
    /// </summary>
    [Table("T_TagInfo")]
    public class UserTagInfo
    {
        /// <summary>
        /// id
        /// </summary>
        [Key]
        [Identity]
        [Column("id")]
        public int Id { get; set; }
        /// <summary>
        /// 标签内容
        /// </summary>		
        [Required(ErrorMessage = "会员标签内容不能为空")]
        [Column("t_tag")]
        public string TagName { get; set; }
        /// <summary>
        /// 标签颜色
        /// </summary>		
        [Required(ErrorMessage = "会员标签颜色不能为空")]
        [Column("t_color")]
        public string TagColor { get; set; }
        /// <summary>
        /// 简拼
        /// </summary>		
        [Column("t_JP")]
        public string TagPy { get; set; }
        /// <summary>
        /// 全拼
        /// </summary>		
        [Column("t_PY")]
        public string TagPinYin { get; set; }
        /// <summary>
        /// 搜索检索值
        /// </summary>		
        [Column("t_search")]
        public string TagSerarch { get; set; }
        /// <summary>
        /// 类别（1、会员，2、商品
        /// </summary>		
        [Column("t_type")]
        public int TagType { get; set; }
        /// <summary>
        /// 店铺ID
        /// </summary>		
        [Column("accid")]
        public int AccId { get; set; }
        /// <summary>
        /// 录入时间
        /// </summary>		
        [Column("insertTime")]
        public DateTime InsertTime { get; set; }
        /// <summary>
        /// 录入人
        /// </summary>		
        [Column("peo_Name")]
        public int OperatorId { get; set; }
    }
}
