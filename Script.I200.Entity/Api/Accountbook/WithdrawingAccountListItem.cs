namespace Script.I200.Entity.Api.Accountbook
{
    /// <summary>
    /// 提现账号列表对象
    /// </summary>
    public class WithdrawingAccountListObject
    {
        /// <summary>
        /// 提现账号Id
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// 提现账号姓名
        /// </summary>
        public string PayeeName { get; set; }

        /// <summary>
        /// 提现账号
        /// </summary>
        public string PayeeAccount { get; set; }
    }
}