using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.TimesCard
{
    /// <summary>
    /// 店铺计次卡
    /// </summary>
    [Table("T_Act_TimesCard")]
    public class AccountTimesCard
    {
        [Key]
        [Identity]
        [Column("id")]
        public int Id { get; set; }

        [Column("accId")]
        public int AccId { get; set; }

        [Column("bindGoodsId")]
        public int BindGoodsId { get; set; }

        [NotMapped]
        public string BindGoodsName { get; set; }

        [StringLength(maximumLength: 25, MinimumLength = 1, ErrorMessage = "店铺计次卡的名称长度必须小于等于25个字符且不为空")]
        [Column("cardName")]
        public string CardName { get; set; }

        [DataType(DataType.Date, ErrorMessage = "非法的时间格式")]
        [Column("editTime")]
        public DateTime EditTime { get; set; }

        [DataType(DataType.Date, ErrorMessage = "非法的时间格式")]
        [Column("addTime")]
        public DateTime AddTime { get; set; }
    }
}
