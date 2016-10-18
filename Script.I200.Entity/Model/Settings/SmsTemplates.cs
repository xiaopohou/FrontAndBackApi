using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.Settings
{
    /// <summary>
    ///     短信模板
    /// </summary>
    [Table("T_Essay")]
    public class SmsTemplates
    {
        [Identity]
        [Key]
        public string Id { get; set; }

        /// <summary>
        ///     模板内容
        /// </summary>
        [Column("works")]
        public string Template { get; set; }

        /// <summary>
        ///     模板分类
        /// </summary>
        [Column("grade")]
        public string SmsCategory { get; set; }

        /// <summary>
        ///     店铺Id
        /// </summary>
        [Column("accountid")]
        public int AccountId { get; set; }

        /// <summary>
        ///     主题
        /// </summary>
        [Column("stamp")]
        public string Category { get; set; }

        /// <summary>
        /// </summary>
        public int Toleration { get; set; }

        /// <summary>
        /// </summary>
        public string Coding { get; set; }
    }
}