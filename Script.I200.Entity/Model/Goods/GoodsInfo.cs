using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.Goods
{
    [Table("T_GoodsInfo")]
    public class GoodsInfoSummary
    {
        [Key]
        [Identity]
        [Column("gid")]
        public int Id { get; set; }

        [Column("accID")]
        public int AccId { get; set; }

        [Column("gName")]
        public string GName { get; set; }

        [Column("isService")]
        public int IsService { get; set; }

        public int IsExtend { get; set; }
    }
}
