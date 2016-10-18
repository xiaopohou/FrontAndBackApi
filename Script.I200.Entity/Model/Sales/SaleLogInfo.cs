using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.Sales
{
    [Table("T_Sale_LogInfo")]
    public class SaleLogInfo
    {
        /// <summary>
        /// 日志ID
        /// </summary>		
        [Key]
        [Identity]
        public int saleLogID { get; set; }

        /// <summary>
        /// 店铺ID
        /// </summary>		
        public int accID { get; set; }

        /// <summary>
        /// 销售表ID
        /// </summary>		
        public int saleID { get; set; }

        /// <summary>
        /// 销售列表ID
        /// </summary>		
        public int saleListID { get; set; }

        /// <summary>
        /// 日志类别
        /// <para>
        /// {"1":"退货记录","2":"删除记录","3":"还款记录","4":"修改记录"}
        /// </para>
        /// </summary>		
        public int logType { get; set; }

        /// <summary>
        /// 日志小类别
        /// <para>
        /// {"logType":"1",{"1":"质量原因","2":"商品不一致","3":"其他原因"}}
        /// </para>
        /// <para>{"logType":"2",{"1":"销售记录删除"}}</para>
        /// <para>{"logType":"3",{"1":"还款"}</para>
        /// <para>{"logType":"4",{"1":"修改销售人员","2":"修改备注"}}</para>
        /// </summary>		
        public int itemType { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>		
        public decimal OriginalVal { get; set; }

        /// <summary>
        /// 修改值
        /// </summary>		
        public decimal EditVal { get; set; }

        /// <summary>
        /// 剩余值
        /// </summary>		
        public decimal FinalVal { get; set; }

        /// <summary>
        /// 日志时间
        /// </summary>		
        public DateTime LogTime { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>		
        public DateTime operatorTime { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>		
        public int operatorID { get; set; }

        /// <summary>
        /// 操作IP
        /// </summary>		
        public string operatorIP { get; set; }

        /// <summary>
        /// 备注说明
        /// </summary>		
        public string Remark { get; set; }

        /// <summary>
        /// Flag
        /// </summary>		
        public string Flag { get; set; }

        public int addedLgUserId { get; set; }
    }
}
