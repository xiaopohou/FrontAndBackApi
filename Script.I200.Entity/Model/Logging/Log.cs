using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;
using Script.I200.Entity.Enum;

namespace Script.I200.Entity.Model.Logging
{
    [Table("Logs")]
    public class Log 
    {
        [Identity]
        [Key]
        public int LogId { get; set; }

        public string ShortMessage { get; set; }

        public string FullMessage { get; set; }

    
        public string IpAddress { get; set; }


        public string PageUrl { get; set; }

   
        public string ReferrerUrl { get; set; }


        public DateTime CreatedOnUtc { get; set; }


        public string RequestLogId { get; set; }

        public LogLevel LogLevel { get; set; }

    
    }
}
