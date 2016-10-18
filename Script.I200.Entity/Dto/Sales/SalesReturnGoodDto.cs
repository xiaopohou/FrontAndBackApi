using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Script.I200.Entity.Dto.Sales
{
    public class SalesReturnGoodDto
    {
        /// <summary>
        /// 退货类型，LOV：1=商品质量原因，客户退货；2=商品不一致，客户退货，3=其他原因，客户退货，4=输入错误，删除本条记录
        /// </summary>
        [Required(ErrorMessage = "退货类型不能为空")]
        [Range(typeof(int), "1", "4", ErrorMessage = "请输入类型1~4")]
        public int type { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
    }
}
