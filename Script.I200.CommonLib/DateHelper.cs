using System;
using System.Text;

namespace CommonLib
{
    public static class DateHelper
    {
        #region GetDateRangeNotice 日期区间提示
        /// <summary>
        /// 时间区间提示
        /// </summary>
        /// <param name="limit">30-本月 90-过去三个月</param>
        /// <returns></returns>
        public static string GetDateRangeNotice(int limit)
        {
            var nowDate = DateTime.Now;
            var begin = "";
            var end = DateTime.Now.ToString("yyyy-MM-dd"); ;

            StringBuilder result = new StringBuilder();
            if (limit == 30)
            {
                begin = nowDate.AddDays(1 - nowDate.Day).ToString("yyyy-MM-dd");
                result.Append(begin);
                result.Append("至");
                result.Append(end);

            }
            else if (limit == 90)
            {
                begin = nowDate.AddDays(1 - nowDate.Day).AddMonths(-2).ToString("yyyy-MM-dd");
                result.Append(begin);
                result.Append("至");
                result.Append(end);
            }
            return result.ToString();
        }
        #endregion
    }
}
