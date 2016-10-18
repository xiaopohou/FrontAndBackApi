using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Script.I200.Entity.Model.Modules
{
    [Table("T_Module")]
    public class Module
    {
        /// <summary>
        /// 模块id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 模块标识符
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 菜单 key
        /// </summary>
        public string ParentMenuKey { get; set; }

        /// <summary>
        /// 模块链接页
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 是否收费；LOV：0=免费（default）,1=收费
        /// </summary>
        public int IsPaid { get; set; }

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
