using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.Expenses
{
    /// <summary>
    /// 支出分类表
    /// </summary>
    [Table("T_Pay_class")]
    public class PayClass
    {
        [Key]
        [Identity]
        public int Id { get; set; }

        [Required(ErrorMessage="分类名称不能为空")]
        [StringLength(maximumLength: 20, ErrorMessage = "支出分类的名称长度必须小于等于20个字符")]
        public string Name { get; set; }

        [Column("classid")]
        public int SuperId { get; set; }

        [Column("accountid")]
        public int MerchantId { get; set; }

        [Column("Fettle")]
        public int? Status { get; set; }
    }
}