using Script.I200.Entity.Enum;

namespace Script.I200.Entity.Dto.Accountbook
{
    public class AccountbookRequest
    {
        /// <summary>
        /// 请求方式枚举
        /// </summary>
        public BankPlatBusinessEnum BusinessEnum { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Operater { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OperaterName { get; set; }
        /// <summary>
        /// 店铺ID
        /// </summary>
        public string AccId { get; set; }

        /// <summary>
        /// 请求名称
        /// </summary>
        public string RequestName { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public string RequestJson { get; set; }

        /// <summary>
        /// 加密签名
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string Timestamp { get; set; }
        /// <summary>
        /// 6位随机数
        /// </summary>
        public string Nonce { get; set; }

        /// <summary>
        /// 加密key
        /// </summary>
        public string BusinessKey { get; set; }
    }
}
