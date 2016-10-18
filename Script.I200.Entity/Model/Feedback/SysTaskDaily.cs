using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.Feedback
{
    [Table("Sys_TaskDaily")]
    public class SysTaskDaily
    {
        /// <summary>
        ///     id
        /// </summary>
        [Key]
        [Identity]
        public int Id { get; set; }

        /// <summary>
        ///     店铺ID
        /// </summary>
        public int Accountid { get; set; }

        /// <summary>
        ///     内容
        /// </summary>
        [Column("t_mk")]
        public string Content { get; set; }

        /// <summary>
        ///     回访方式
        /// </summary>
        [Column("vm_id")]
        public int VisitType { get; set; }

        /// <summary>
        ///     任务操作时间
        /// </summary>
        [Column("dt_Time")]
        public DateTime OperateTime { get; set; }

        /// <summary>
        ///     任务等级
        /// </summary>
        [Column("dt_Level")]
        public int Level { get; set; }

        /// <summary>
        ///     任务状态
        /// </summary>
        [Column("dt_Status")]
        public int Status { get; set; }

        /// <summary>
        ///     处理人
        /// </summary>
        [Column("dt_Name")]
        public string DealPersonName { get; set; }

        /// <summary>
        ///     录入时间
        /// </summary>
        [Column("inertTime")]
        public DateTime InsertTime { get; set; }

        /// <summary>
        ///     录入人
        /// </summary>
        [Column("insetName")]
        public string InsertName { get; set; }

        /// <summary>
        ///     回访备注
        /// </summary>
        [Column("dt_remark")]
        public string Remark { get; set; }

        /// <summary>
        ///     记录的登录账号ID
        /// </summary>
        [Column("dt_logUid")]
        public int LoginUid { get; set; }

        /// <summary>
        ///     来源
        /// </summary>
        [Column("dt_Source")]
        public string Source { get; set; }
    }
}