using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.Modules
{
    [Table("T_Module_AccountUser")]
    public class AccountUserModule
    {
        /// <summary>
        /// 编号
        /// </summary>		
        [Key]
        [Identity]
        public int Id { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>		
         [NotMapped]
        public int Name { get; set; }

         /// <summary>
         /// 模块标识符
         /// </summary>		
         [NotMapped]
         public string Icon { get; set; }

         /// <summary>
         /// 模块链接页
         /// </summary>
        [NotMapped] 
        public string Url { get; set; }

         /// <summary>
         /// 是否收费；LOV：0=免费（default）,1=收费
         /// </summary>
        [NotMapped] 
        public int IsPaid { get; set; }

        /// <summary>
        /// 店铺Id
        /// </summary>		
        public int AccountId { get; set; }

        /// <summary>
        /// 店铺店员Id
        /// </summary>		
        public int AccountUserId { get; set; }

        /// <summary>
        /// 模块Id
        /// </summary>		
        public int ModuleId { get; set; }

        /// <summary>
        /// 是否启用；LOV：0=关闭（default），1=启用；
        /// </summary>
        public int Enable { get; set; }

        /// <summary>
        /// 模块顺序，升续
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>		
        public DateTime UpdateTime { get; set; }
    }
}
