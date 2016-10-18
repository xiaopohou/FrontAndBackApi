using System;

namespace Script.I200.Entity.Api.Accountbook
{
    public class C_SearchParams
    {
        /// <summary>
        /// 分页页码
        /// </summary>
        public int? PageIndex { get; set; }

        /// <summary>
        /// 分页页大小
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// 实体关键词搜索
        /// </summary>
        public string KeyWords { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public int? BillingBusinessType { get; set; }

        /// <summary>
        /// 查询状态
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 实体搜索开始时间
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 实体搜索结束时间
        /// </summary>
        private DateTime? endDate { get; set; }

        /// <summary>
        /// 实体搜索结束时间
        /// </summary>
        public DateTime? EndDate
        {
            get
            {
                if (endDate != null)
                {
                    return DateTime.Parse(this.endDate.Value.ToString("yyyy-MM-dd") + " 23:59:59");
                }

                return null;
            }
            set { this.endDate = value; }
        }
    }
}