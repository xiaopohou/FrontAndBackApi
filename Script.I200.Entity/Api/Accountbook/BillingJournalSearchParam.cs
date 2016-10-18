using System;
using Script.I200.Entity.API;

namespace Script.I200.Entity.Api.Accountbook
{
    /// <summary>
    /// 收单记录列表查询参数
    /// </summary>
    public class BillingJournalSearchParam : PaginationParamBase
    {
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Search { get; set; }

        /// <summary>
        /// 交易状态id
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 收款方式id
        /// </summary>
        public int Paytype { get; set; }

        /// <summary>
        /// 交易类型 0:入账, 1:出账
        /// </summary>
        public int TradeType { get; set; }

        /// <summary>
        /// 业务类型 2:微信收款 4:手机橱窗
        /// </summary>
        public int? BusinessType { get; set; }

        /// <summary>
        /// 筛选开始时间 格式YYYY-MM-DD或者YYYY-MM-DD hh:mm:ss
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 筛选结束时间 格式YYYY-MM-DD或者YYYY-MM-DD hh:mm:ss
        /// </summary>
        public DateTime? EndDate { get; set; }

        public C_SearchParams ToDto()
        {
            return new C_SearchParams()
            {
                StartDate = StartDate,
                EndDate = EndDate,
                KeyWords = Search,
                PageIndex = CurrentPage,
                PageSize = PageSize,
                Status = Status,
                BillingBusinessType = BusinessType
            };
        }
    }
}
