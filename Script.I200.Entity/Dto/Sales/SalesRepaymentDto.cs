using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Script.I200.Entity.Dto.Sales
{
    /// <summary>
    /// 还款Dto
    /// </summary>
    public class SalesRepaymentDto
    {
        /// <summary>
        /// 还款金额
        /// </summary>
        [Required(ErrorMessage = "还款金额不能为空")]
        [Range(typeof(decimal), "0.00", "999999.99", ErrorMessage = "请输入小于100万的金额")]
        public decimal money { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 操作员
        /// </summary>
        public int operatorID { get; set; }

        /// <summary>
        /// 是否发短信，LOV:1=发送，0=不发送
        /// </summary>
        public int sendsms { get; set; }
    }
}
