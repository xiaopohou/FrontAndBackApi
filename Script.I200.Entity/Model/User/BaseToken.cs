using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Script.I200.Entity.Model.User
{
    [Table("T_Token")]
    public class BaseToken
    {
        [Key]
        public int Id { get; set; }

        public int AccId { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Token { get; set; }

        public DateTime CreateTime { get; set; }

        public string OperatorIp { get; set; }

        [NotMapped]
        public string AppKey { get; set; }
    }
}