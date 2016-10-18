using System;

namespace Script.I200.Entity.Dto.Accountbook
{
    public class C_AccountWithdrawalsShowDto
    {
        /// <summary>
        /// 提现流水单编号
        /// </summary>		
        public int Id { get; set; }
        /// <summary>
        /// 对外流水单编号
        /// </summary>		
        public string WithdrawalsTradeId { get; set; }
        /// <summary>
        /// 店铺ID
        /// </summary>		
        public int AccountId { get; set; }
        /// <summary>
        /// 店铺提现申请人
        /// </summary>		
        public int WithdrawUserId { get; set; }
        /// <summary>
        /// 店铺绑定卡号编号；关联：T_Account_CreditCard.Id
        /// </summary>		
        public int AccountCreditCardId { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>		
        public decimal TradeMoney { get; set; }
        /// <summary>
        /// 交易标题（原因）；如：提取现金到某某账号
        /// </summary>		
        public string TradeTitle { get; set; }
        /// <summary>
        /// 状态；LOV：0=关闭（无效），1=新建（有效），2=审核成功，3=未通过审核，4=支付中，5=支付成功，6=支付失败
        /// </summary>		
        public int Status { get; set; }
        /// <summary>
        /// 提现备注
        /// </summary>		
        public string Remark { get; set; }

        /// <summary>
        /// 第三方支付编号；关联：ThirdPartyPayment.Id
        /// </summary>		
        public int ThirdPartyPaymentId { get; set; }
        /// <summary>
        /// 账号持有人姓名
        /// </summary>		
        public string MasterCardName { get; set; }
        /// <summary>
        /// 账号信息
        /// </summary>		
        public string MasterCardAccount { get; set; }
        /// <summary>
        /// 手续费标准金额
        /// </summary>		
        public decimal PoundageScaleValue { get; set; }
        /// <summary>
        /// 手续费金额（实收）
        /// </summary>		
        public decimal PoundageScaleValueDiscount { get; set; }
        /// <summary>
        /// 业务流水编号；关联：T_Billing_Journal.JournalOutId
        /// </summary>		
        public string BillingJournalId { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>		
        public DateTime AuditTime { get; set; }
        /// <summary>
        /// 审核操作人
        /// </summary>		
        public string AuditOperater { get; set; }
        /// <summary>
        /// 开始支付时间
        /// </summary>		
        public DateTime TransferTime { get; set; }
        /// <summary>
        /// 结束支付时间
        /// </summary>		
        public DateTime CompleteTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; }
    }
}
