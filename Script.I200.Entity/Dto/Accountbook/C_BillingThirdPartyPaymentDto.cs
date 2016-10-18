namespace Script.I200.Entity.Api.Accountbook
{
    public class C_BillingThirdPartyPaymentDto
    {
        /// <summary>
        /// 实体编号
        /// </summary>		
        public int Id { get; set; }
        /// <summary>
        /// 第三方支付名称，如：微信支付、招行银行、中国银行
        /// </summary>		
        public string Name { get; set; }
        /// <summary>
        /// 全称，如：中国银行股份有限公司
        /// </summary>		
        public string FullName { get; set; }
        /// <summary>
        /// 英文名称；如：中国银行=Bank of China Limited
        /// </summary>		
        public string EnName { get; set; }
        /// <summary>
        /// 简称；如：中国银行=BOC
        /// </summary>		
        public string ShortName { get; set; }
        /// <summary>
        /// 第三方支付描述
        /// </summary>		
        public string Desc { get; set; }
        /// <summary>
        /// 入账应收标准费率；客户使用支付的费率
        /// </summary>		
        public decimal PoundageScaleForPay { get; set; }
        /// <summary>
        /// 出账应收标准费率；客户提款转账的费率
        /// </summary>		
        public decimal PoundageScaleForReceive { get; set; }
        /// <summary>
        /// 入账实收优惠费率；客户使用支付的费率
        /// </summary>		
        public decimal PoundageScaleForPayDiscount { get; set; }
        /// <summary>
        /// 出账实收优惠费率；客户提款转账的费率
        /// </summary>		
        public decimal PoundageScaleForReceiveDiscount { get; set; }
        /// <summary>
        /// 类型；LOV：0=收银与结算，1=收银，2=结算
        /// </summary>		
        public int Type { get; set; }
    }
}
