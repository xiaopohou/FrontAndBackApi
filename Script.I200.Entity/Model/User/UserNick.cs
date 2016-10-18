using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.User
{
    [Table("T_User_NickName")]
    public class UserNickName
    {

        /// <summary> 
        ///  
        /// </summary>
        [Key]
        [Identity]
        public int nickID { get; set; }


        /// <summary> 
        ///  
        /// </summary>
        public int accID { get; set; }


        /// <summary> 
        ///  
        /// </summary>
        public string nickName { get; set; }


        /// <summary> 
        ///  
        /// </summary>
        public DateTime insertTime { get; set; }

    }
}
