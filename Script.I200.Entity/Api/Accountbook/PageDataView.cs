using System.Collections.Generic;

namespace Script.I200.Entity.Api.Accountbook
{
    /// <summary>
    /// 分页视图对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageDataView<T>
    {
        public PageDataView()
        {
            this.Items = new List<T>();
        }

        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalNum { get; set; }

        /// <summary>
        /// 数据实体
        /// </summary>
        public List<T> Items { get; set; }

        /// <summary>
        /// 分页：当前页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 分页：每页条数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 分页：总页数
        /// </summary>
        public int TotalPageCount { get; set; }

        /// <summary>
        /// 总金额0.00
        /// </summary>
        public double TotalMoney { get; set; }

        /// <summary>
        /// 出账总金额0.00
        /// </summary>
        public double TotalMoneyOut { get; set; }

        /// <summary>
        /// 入账总金额0.00
        /// </summary>
        public double TotalMoneyIn { get; set; }
    }
}
