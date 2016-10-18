using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.User
{
    [Table("T_TagKey")]
    public class UserTagKey
    {
        /// <summary> 
        ///  
        /// </summary>
        [Key]
        [Identity]
        public int id { get; set; }


        /// <summary> 
        ///  
        /// </summary>
        public int tk_tagId { get; set; }


        /// <summary> 
        ///  
        /// </summary>
        public int tk_valId { get; set; }


        /// <summary> 
        ///  
        /// </summary>
        public int tk_type { get; set; }


        /// <summary> 
        ///  
        /// </summary>
        public int accid { get; set; }


        /// <summary> 
        ///  
        /// </summary>
        public DateTime insertTime { get; set; }


        /// <summary> 
        ///  
        /// </summary>
        public int peo_Name { get; set; }

    }
}








