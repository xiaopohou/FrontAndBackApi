using System;

namespace Script.I200.Entity.Api.Expenses
{
    /// <summary>
    /// 收单记录列表查询返回结果项
    /// </summary>
    public class ExpensesSearchResultItem
    {
        /// <summary>
        /// 支出记录编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 支出时间
        /// </summary>
        public DateTime PayDate { get; set; }

        /// <summary>
        /// 支出金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 支出大分类
        /// </summary>
        public string MainCategoryName { get; set; }

        /// <summary>
        /// 支出小分类
        /// </summary>
        public string SubCategoryName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// 店铺id
        /// </summary>
        public int MerchanId { get; set; }

        /// <summary>
        /// 支出人员Id，店员
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 支出人员姓名
        /// </summary>
        public int UserName { get; set; }

        /// <summary>
        /// 录入人员id，店员
        /// </summary>
        public int OperatorId { get; set; }

        /// <summary>
        /// 录入人员ip
        /// </summary>
        public int OperatorIp { get; set; }
    }
}