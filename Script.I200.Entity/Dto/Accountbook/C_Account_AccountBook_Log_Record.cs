using System;

namespace Script.I200.Entity.Dto.Accountbook
{
    public class C_Account_AccountBook_Log_Record
    {
        /// <summary>
        /// 业务流水编号；关联：T_Billing_Journal.Id
        /// </summary>
        public int BillingJournalId { get; set; }
        /// <summary>
        /// 实际入账金额；注：如果官方承担收费费，TradeMoney=EnterMoney
        /// </summary>		
        public decimal EnterMoney { get; set; }
        /// <summary>
        /// 最终账户金额
        /// </summary>		
        public decimal FinalMoney { get; set; }
        /// <summary>
        /// 交易类型；LOV：0=支付，1=收取
        /// </summary>
        public int TradeType { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 业务编号；关联：业务编号，关联：T_Billing_Business.Id
        /// </summary>
        public int BillingBusinessId { get; set; }
        /// <summary>
        /// 状态；LOV：0=关闭（无效），1=新建（有效），2=支付成功，3=支付失败，4=冻结资金，5=解冻资金中，6=成功解冻资金，7=失败解冻资金，8=提现中，1000=交易成功
        /// </summary>
        public int JournalStatus { get; set; }
        /// <summary>
        /// 状态；LOV：0=关闭（无效），1=新建（有效），2=审核成功，3=未通过审核，4=支付中，5=支付成功，6=支付失败
        /// </summary>
        public int WithdrawStatus { get; set; }
        /// <summary>
        /// 业务名称，如：提现、手机橱窗
        /// </summary>	
        public string BillingBusinessName { get; set; }
    }
}
