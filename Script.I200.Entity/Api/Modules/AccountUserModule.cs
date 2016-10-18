namespace Script.I200.Entity.Api.Modules
{
    public class AccountUserModule
    {
        /// <summary>
        /// 模块id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string name { get; set; }


        /// <summary>
        /// 菜单key
        /// </summary>
        public string ParentMenuKey { get; set; }

        /// <summary>
        /// 模块标识符
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// 模块链接页
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 是否收费 0=免费,1=收费
        /// </summary>
        public int isPaid { get; set; }
    }
}
