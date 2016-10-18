namespace Script.I200.Entity.Dto.Accountbook
{
    public class AccountWithdrawalsDto
    {
        /// <summary>
        /// [*必填]店铺绑定卡号编号；关联：T_Account_CreditCard.Id
        /// </summary>		
        public int AccountCreditCardId { get; set; }
        /// <summary>
        /// [*必填]交易金额
        /// </summary>		
        public decimal TradeMoney { get; set; }
        /// <summary>
        /// [选填]交易标题（原因）；如：提取现金到某某账号
        /// </summary>		
        public string TradeTitle { get; set; }
        /// <summary>
        /// [选填]提现备注
        /// </summary>		
        public string Remark { get; set; }
    }
}
