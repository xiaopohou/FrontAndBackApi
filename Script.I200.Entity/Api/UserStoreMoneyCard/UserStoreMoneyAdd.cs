using System.ComponentModel.DataAnnotations;

namespace Script.I200.Entity.Api.UserStoreMoneyCard
{
    public class UserStoreMoneyAdd
    {
        /// <summary>
        ///用户Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///用户手机
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 充值金额
        /// </summary>
        [Range(typeof(decimal), "0.00", "999999.99", ErrorMessage = "请输入小于100万的金额")]
        public decimal RechargeMoney { get; set; }

        /// <summary>
        /// 实收金额
        /// </summary>
        [Range(typeof(decimal), "0.00", "999999.99", ErrorMessage = "请输入小于100万的金额")]
        public decimal RealMoney { get; set; }

        /// <summary>
        /// 销售人员
        /// </summary>
        public int Salesman { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public int PayType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(maximumLength: 100, ErrorMessage = "备注的长度必须小于等于100个字符")]
        public string Remark { get; set; }

        /// <summary>
        /// 是否发送短信
        /// </summary>
        public bool SendMsg { get; set; }
    }
}