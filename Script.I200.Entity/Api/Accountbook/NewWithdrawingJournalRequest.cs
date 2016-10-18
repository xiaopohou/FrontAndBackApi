using Script.I200.Entity.Dto.Accountbook;

namespace Script.I200.Entity.Api.Accountbook
{
    /// <summary>
    /// 提现
    /// </summary>
    public class NewWithdrawingJournalRequest
    {
        /// <summary>
        /// 提现金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 提现账号Id
        /// </summary>
        public int WithdrawingAccountId { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public int CheckCode { get; set; }

        public AccountWithdrawalsDto ToDto()
        {
            return new AccountWithdrawalsDto()
            {
                AccountCreditCardId = WithdrawingAccountId,
                TradeMoney = Amount
            };
        }
    }
}