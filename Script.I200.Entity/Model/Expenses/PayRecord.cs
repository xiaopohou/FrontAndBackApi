using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.Expenses
{
    /// <summary>
    /// 支出记录表
    /// </summary>
    [Table("t_PayRecord")]
    public class PayRecord
    {
        [Key]
        [Identity]
        public int Id { get; set; }

        [DataType(DataType.Date, ErrorMessage = "非法的时间格式")]
        public DateTime PayDate { get; set; }

        [Range(typeof(decimal), "0.00", "999999.99", ErrorMessage = "请输入小于100万的金额")]
        [Column("PaySum")]
        public decimal Amount { get; set; }

        [NotMapped]
        public int MainCategoryId { get; set; }

        [NotMapped]
        public int SubCategoryId { get; set; }

        [Column("PayMaxType")]
        public string MainCategoryName { get; set; }

        [StringLength(maximumLength: 100, ErrorMessage = "备注的长度必须小于等于100个字符")]
        [Column("PayName")]
        public string Notes { get; set; }

        [Column("PayMinType")]
        public string SubCategoryName { get; set; }

        [Column("ShopperId")]
        public int MerchanId { get; set; }

        [Column("insertUserId")]
        public int UserId { get; set; }

        [Column("insertUserName")]
        public string UserName { get; set; }

        public int OperatorId { get; set; }

        public string OperatorIp { get; set; }
    }
}