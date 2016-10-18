namespace CommonLib.UserSearch
{
    public class UserInfo 
    {
        /// <summary>
        /// 用户所在店铺Id
        /// </summary>
        public string ShopId { get; set; }

        /// <summary>
        /// 所在店铺顶级账号Id
        /// </summary>
        public string MaxShopId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户姓名全拼
        /// </summary>
        public string UPinYinFull { get; set; }

        /// <summary>
        /// 用户姓名首字母拼写
        /// </summary>
        public string UPinYinShort { get; set; }

        /// <summary>
        /// 用户编号 
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 用户电话 
        /// </summary>
        public string Phone { get; set; }

    }

    public class UserSearchModel
    {
        public long uid { get; set; }
        public string uPhone { get; set; }
        public string uNumber { get; set; }
        public string uName { get; set; }
        public long AccId { get; set; }
        public long masterId { get; set; }
    }


    public class UserBasic
    {
        public long user_id { get; set; }

        public string user_name { get; set; }

        public string user_cardno { get; set; }

        public string user_phone { get; set; }

        public string user_initials { get; set; }

        public string user_pinyin { get; set; }

        public long account_id { get; set; }

        public long master_id { get; set; }
    }
}
