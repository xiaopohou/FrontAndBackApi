using System;
using Script.I200.Entity.API;

namespace Script.I200.Entity.Api.Accountbook
{
    /// <summary>
    /// 提现记录列表查询参数
    /// </summary>
    public class WithdrawingJournalSearchParam : PaginationParamBase
    {
        public string Search { get; set; }

        public int? Status { get; set; }

        public int PayType { get; set; }

        public int TradeType { get; set; }

        public DateTime? StartDate { get; set; }

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
                Status = Status
            };
        }
    }
}