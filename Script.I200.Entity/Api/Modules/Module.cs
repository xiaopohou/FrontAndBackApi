using System.Collections.Generic;

namespace Script.I200.Entity.Api.Modules
{
    public class ModuleList
    {
        /// <summary>
        /// 免费模块
        /// </summary>
        public List<Module> Free { get; set; }

        /// <summary>
        /// 付费模块
        /// </summary>
        public List<Module> Paid { get; set; }
    }

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
        /// 父级菜单Key
        /// </summary>
        public string ParentMenuKey { get; set; }

        /// <summary>
        /// 模块标识符
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 模块链接页
        /// </summary>
        public string Url { get; set; }
    }
}
