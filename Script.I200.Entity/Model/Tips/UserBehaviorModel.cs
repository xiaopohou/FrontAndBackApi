using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.Tips
{
    [Table("T_UserBehavior")]
    public class UserBehaviorModel
    {
        [Key]
        [Identity]
        public int Id { get; set; }

        public int ElementId { get; set; }

        public string ElementName { get; set; }

        public int EventType { get; set; }

        public int ConfigCount { get; set; }

        public int AccId { get; set; }

        public int AccUserId { get; set; }

        public int TotalCount { get; set; }
    }
}