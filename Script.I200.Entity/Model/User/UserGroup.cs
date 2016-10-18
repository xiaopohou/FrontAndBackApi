using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.User
{
    /// <summary>
    /// 会员分组
    /// </summary>
    [Table("T_User_Group")]
    public class UserGroup
    {
        [Key]
        [Identity]
        [Column("groupID")]
        public int GroupId { get; set; }
        [Column("accID")]
        public int AccId { get; set; }
        [Required(ErrorMessage = "会员分组名称不能为空")]
        [StringLength(maximumLength: 100, ErrorMessage = "会员分组名称长度必须小于等于100个字符")]
        [Column("gpName")]
        public string GroupName { get; set; }
        [Column("gpAddTime")]
        public DateTime GroupAddTime { get; set; }
    }
}
