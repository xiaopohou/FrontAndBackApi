using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Script.I200.Entity.Model.BaseData
{
    [Table("T_AddressBase")]
    public class AddressBase
    {
        [Key]
        public int Id { get; set; }

        public int SelfId { get; set; }

        public string Name { get; set; }

        public int ParentId { get; set; }

        public int Level { get; set; }

        public int Status { get; set; }

    }
}
