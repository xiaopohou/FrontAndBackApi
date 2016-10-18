using Script.I200.Entity.API;

namespace Script.I200.Entity.Api.Users
{
    public class UserListSearch : PaginationParamBase
    {
        public UserListSearch()
        {
            UserGroup = -1;
        }

        /// <summary>
        /// 关键词
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        /// 会员ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 会员分组
        /// </summary>
        public int UserGroup { get; set; }

        /// <summary>
        /// 会员等级 
        /// </summary>
        public int UserGrade { get; set; }
        
        /// <summary>
        /// 标签列表
        /// </summary>
        public int[] UserTags { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SortColumn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SortRank { get; set; }
    }
}
