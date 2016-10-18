namespace Script.I200.Entity.Api.Users
{
    /// <summary>
    /// 获取店铺会员标签
    /// </summary>
    public class UserTagsGet
    {
        public int UserId { get; set; }

        /// <summary>
        /// 标签Id
        /// </summary>
        public int TagId { get; set; }

        /// <summary>
        /// 标签名称
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// 标签颜色
        /// </summary>
        public string TagColor { get; set; }
    }
}
