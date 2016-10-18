using System;
using Script.I200.Entity.API;

namespace Script.I200.Entity.Api.Expenses
{
    /// <summary>
    /// 支出列表查询参数
    /// </summary>
    public class ExpensesSearchParam : PaginationParamBase
    {
        /// <summary>
        /// 支出大分类Id
        /// </summary>
        public int? MainCategoryId { get; set; }

        /// <summary>
        /// 支出小分类Id
        /// </summary>
        public int? SubCategoryId { get; set; }

        /// <summary>
        /// 筛选开始时间 格式YYYY-MM-DD或者YYYY-MM-DD hh:mm:ss
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 支出人员Id
        /// </summary>
        public int? Staff { get; set; }

        /// <summary>
        /// 筛选结束时间 格式YYYY-MM-DD或者YYYY-MM-DD hh:mm:ss
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}