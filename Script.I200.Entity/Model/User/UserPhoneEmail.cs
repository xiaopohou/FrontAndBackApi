using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Script.I200.Entity.Model.User
{

    [Table("T_Account_User")]
    public class UserPhoneEmail
    {
        public UserPhoneEmail()
        {
            Phone = string.Empty;
            Email = string.Empty;
        }

        [Column("PhoneNumber")]
        public string Phone { get; set; }


        [Column("UserEmail")]
        public string Email { get; set; }

        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 店铺账号
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// 用户角色 
        /// </summary>
        public string Grade { get; set; }
    }
}
