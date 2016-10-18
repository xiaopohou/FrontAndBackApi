namespace Script.I200.Entity.Api.Account
{
    public class AccountOverview
    {
        /// <summary>
        /// 销售总额
        /// </summary>
        public decimal SalesMoney { get; set; }

        /// <summary>
        /// 生日会员数
        /// </summary>
        public int UserBirthdayCount { get; set; }

        /// <summary>
        /// 售出商品数
        /// </summary>
        public int GoodsSalesCount { get; set; }

        /// <summary>
        /// 库存预警商品数
        /// </summary>
        public int GoodsStockCount { get; set; }

        /// <summary>
        /// 剩余短信数
        /// </summary>
        public int SmsCount { get; set; }

        /// <summary>
        /// 本月支出
        /// </summary>
        public decimal ThisMonthExpense { get; set; }

        /// <summary>
        /// 资金账可提现余额
        /// </summary>
        public decimal TotalMoeny { get; set; }

        /// <summary>
        /// 会员总数
        /// </summary>
        public int UsersNum { get; set; }

        /// <summary>
        /// 今日手机橱窗订单总数
        /// </summary>
        public int TodayMobileOrdersNum { get; set; }
    }
}
