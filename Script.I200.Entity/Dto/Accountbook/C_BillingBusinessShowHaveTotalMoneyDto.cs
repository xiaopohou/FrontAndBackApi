namespace Script.I200.Entity.Dto.Accountbook
{
    public class C_BillingAmount
    {
        /// <summary>
        /// 业务Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 业务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 业务交易金额
        /// </summary>
        public decimal TotalMoney { get; set; }

        /// <summary>
        /// 业务交易笔数
        /// </summary>
        public int TotalBillingJournalsNum { get; set; }
    }
}