using Script.I200.Entity.Api.Account;

namespace Script.I200.Entity.API
{
    public class UserContext
    {
        /// <summary>
        /// 店铺名称
        /// </summary>
        public int AccId { get; set; }
        
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string OperaterName { get; set; }

        /// <summary>
        /// 操作人Id
        /// </summary>
        public int Operater { get; set; }

        /// <summary>
        /// Ip 地址
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Token值
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// AppKey值
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public int Role { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public int Powers { get; set; }

        /// <summary>
        /// 请求跟踪Id
        /// </summary>
        public string TrackingId { get; set; }

        /// <summary>
        /// 总店Id
        /// </summary>
        public int MasterId { get; set; }

        /// <summary>
        /// 用户电话号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        public AccountVersion AccVersion { get; set; }
    }
}