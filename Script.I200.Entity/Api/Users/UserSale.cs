using System;
using System.Collections.Generic;

namespace Script.I200.Entity.Api.Users
{
    /// <summary>
    /// 店铺会员消费记录
    /// </summary>
    public class UserSale
    {
        /// <summary>
        /// 累计消费（当前结果集） 
        /// </summary>
        public decimal BuyMoney { get; set; }

        public List<UserSaleItem> Items { get; set; }
    }

    /// <summary>
    /// 店铺会员消费记录Item
    /// </summary>
    public class UserSaleItem
    {
        /// <summary>
        /// 复合销售，LOV:1=复合，0=单一
        /// </summary>
        public int Complex { get; set; }

        /// <summary>
        /// 销售内容
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 总计实收金额
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 支付方式，LOV:1=现金支付，2=刷卡支付，3=储值支付，4=按次支付，5=欠款，6=支付宝，7={null}，8=微信支付，9=京东支付，10=百度支付
        /// </summary>
        public int PayType { get; set; }

        /// <summary>
        /// 支付状态，LOV:1=未付款，0=已付款
        /// </summary>
        public int PayState { get; set; }

        /// <summary>
        /// 销售时间
        /// </summary>
        public DateTime SaleTime { get; set; }

        public List<UserSaleDetail> Items { get; set; }
    }

    /// <summary>
    /// 店铺会员消费明细
    /// </summary>
    public class UserSaleDetail
    {
        /// <summary>
        /// 销售内容
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 总计实收金额
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 支付方式，LOV:1=现金支付，2=刷卡支付，3=储值支付，4=按次支付，5=欠款，6=支付宝，7={null}，8=微信支付，9=京东支付，10=百度支付
        /// </summary>
        public int PayType { get; set; }

        /// <summary>
        /// 支付状态，LOV:1=未付款，0=已付款
        /// </summary>
        public int PayState { get; set; }

        /// <summary>
        /// 销售时间
        /// </summary>
        public DateTime SaleTime { get; set; }
    }
}
