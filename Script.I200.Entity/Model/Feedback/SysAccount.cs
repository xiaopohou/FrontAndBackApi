using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.Feedback
{
    [Table("Sys_Account")]
    public class SysAccount
    {
        [Key]
        [Identity]
        public int Id { get; set; }

        public int AccId { get; set; }

        [Column("a_QQ")]
        public string QQ { get; set; }

        [Column("a_WeiXin")]
        public string Weixin { get; set; }

        [Column("a_Tel")]
        public string Telephone { get; set; }

        [Column("a_Address")]
        public string Address { get; set; }

        [Column("a_Industry")]
        public string Industry { get; set; }

        [Column("a_Name")]
        public string Name { get; set; }

        [Column("a_IdentityNumber")]
        public string IdentityNumber { get; set; }

        [Column("a_ShopSize")]
        public string ShopSize { get; set; }

        [Column("a_Operate")]
        public string Operate { get; set; }

        [Column("a_Duration")]
        public string Duration { get; set; }

        [Column("a_OtherSoftware")]
        public string OtherSoftware { get; set; }

        [Column("a_Remark")]
        public string Remark { get; set; }

        [Column("a_OtherSoftwareType")]
        public int OtherSoftwareType { get; set; }

        [Column("a_CustomerSourceType")]
        public int CustomerSourceType { get; set; }

        [Column("a_CustomerSource")]
        public string CustomerSource { get; set; }

        [Column("feedbackTel")]
        public string FeedbackTel { get; set; }

        [Column("feedbackQQ")]
        public string FeedbackQq { get; set; }

        [Column("a_UseCause")]
        public string UseCause { get; set; }

        [Column("sysAddress")]
        public string SysAddress { get; set; }
    }
}