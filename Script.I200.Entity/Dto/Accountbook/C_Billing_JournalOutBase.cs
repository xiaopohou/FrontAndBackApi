using System;

namespace Script.I200.Entity.Dto.Accountbook
{
    public class C_Billing_JournalOutBase
    {
        /// <summary>
        /// 流水单编号
        /// </summary>		
        public int Id { get; set; }
        /// <summary>
        /// 对外流水单编号
        /// </summary>		
        public string JournalOutId { get; set; }
        /// <summary>
        /// 业务编号；关联：业务编号，关联：T_Billing_Business.Id
        /// </summary>		
        public int BillingBusinessId { get; set; }
        /// <summary>
        /// 店铺编号
        /// </summary>		
        public int AccountId { get; set; }
        /// <summary>
        /// 第三方支付编号；关联：ThirdPartyPayment.Id
        /// </summary>		
        public int ThirdPartyPaymentId { get; set; }
        /// <summary>
        /// 第三方支付交易编号
        /// </summary>		
        public string ThirdPartyPaymentJournalId { get; set; }
        /// <summary>
        /// 交易金额；LOV：负值=结算、花费，正值=收入
        /// </summary>		
        public decimal TradeMoney { get; set; }

        /// <summary>
        /// 交易类型；LOV：0=入账，1=出账
        /// </summary>		
        public int TradeType { get; set; }
        /// <summary>
        /// 交易编号（业务可跟踪编号）；如：订单编号
        /// </summary>		
        public string TradeId { get; set; }
        /// <summary>
        /// 交易标题（原因）；如：出售可乐10瓶
        /// </summary>		
        public string TradeTitle { get; set; }
        /// <summary>
        /// 状态；LOV：0=关闭（无效），1=新建（有效），2=支付成功，3=支付失败，4=冻结资金，5=解冻资金中，6=成功解冻资金，7=失败解冻资金，8=提现中，1000=交易成功
        /// </summary>		
        public int Status { get; set; }
        /// <summary>
        /// 流水备注
        /// </summary>		
        public string Remark { get; set; }
        /// <summary>
        /// 开始支付时间
        /// </summary>		
        public DateTime TransferTime { get; set; }
        /// <summary>
        /// 结束支付时间
        /// </summary>		
        public DateTime CompleteTime { get; set; }
        /// <summary>
        /// 冻结开始时间
        /// </summary>		
        public DateTime FrozenStartTime { get; set; }
        /// <summary>
        /// 冻结结束时间
        /// </summary>		
        public DateTime FrozenEndTime { get; set; }
        /// <summary>
        /// 实际解冻时间
        /// </summary>		
        public DateTime UnFrozenTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>		
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 实际支付金额；
        /// </summary>		
        public decimal PayMoney { get; set; }
        /// <summary>
        /// 手续费费率（应收）
        /// </summary>		
        public decimal PoundageScale { get; set; }
        /// <summary>
        /// 手续费金额（应收）
        /// </summary>		
        public decimal PoundageScaleValue { get; set; }
        /// <summary>
        /// 手续费费率（实收）
        /// </summary>		
        public decimal PoundageScaleDiscount { get; set; }
        /// <summary>
        /// 手续费金额（实收）
        /// </summary>		
        public decimal PoundageScaleValueDiscount { get; set; }

    }
}
