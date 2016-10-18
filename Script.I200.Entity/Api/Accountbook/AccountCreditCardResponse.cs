namespace Script.I200.Entity.Api.Accountbook
{
    /// <summary>
    /// 获取提现账户列表实体
    /// </summary>
    public class AccountCreditCardResponse
    {
        /// <summary>
        /// 提现账号Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 持卡人姓名
        /// </summary>
        public string PayeeName { get; set; }

        /// <summary>
        /// 绑定的银行电话号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 银行卡账号，**** **** **** 5207 （前12位为*号，只显示后4位）
        /// </summary>
        public string PayeeAccount { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 提现账号状态 0: 审核中 1:已认证（只有已认证的提现账号才可以以提现）
        /// </summary>
        public int State { get; set; }
    }
}