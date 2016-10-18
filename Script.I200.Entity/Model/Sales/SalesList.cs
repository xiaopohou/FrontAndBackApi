using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Script.I200.Entity.Model.Sales
{

    [Table("T_Sale_List")]
    public class SalesList
    {
        /// <summary>
        /// 销售列表ID
        /// </summary>		
        public long saleListID { get; set; }

        /// <summary>
        /// 大分类名称
        /// </summary>		
        public string maxClass { get; set; }

        /// <summary>
        /// 小分类名称
        /// </summary>		
        public string minClass{ get; set; }

        /// <summary>
        /// 商品规格
        /// </summary>		
        public string Specification{ get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>		
        public string GoodsName { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>		
        public decimal GoodsNum { get; set; }

        /// <summary>
        /// 商品单价
        /// </summary>		
        public decimal Price { get; set; }

        /// <summary>
        /// 销售日期
        /// </summary>		
        public DateTime saleTime { get; set; }

        /// <summary>
        /// 会员ID
        /// </summary>		
        public long userID { get; set; }

        /// <summary>
        /// 店铺ID
        /// </summary>		
        public long accID { get; set; }

        /// <summary>
        /// 商品折扣
        /// </summary>		
        public decimal Discount { get; set; }

        /// <summary>
        /// 流水号
        /// </summary>		
        public string saleNo { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>		
        public string Remark { get; set; }

        /// <summary>
        /// 大分类ID
        /// </summary>		
        public int maxClassID { get; set; }

        /// <summary>
        /// 小分类ID
        /// </summary>		
        public int minClassID { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>		
        public int GoodsID { get; set; }

        /// <summary>
        /// 是否积分
        /// </summary>		
        public int isIntegral { get; set; }

        /// <summary>
        /// 商品进价
        /// </summary>		
        public decimal CostPrice { get; set; }

        /// <summary>
        /// 实收金额
        /// </summary>		
        public decimal RealMoney { get; set; }

        /// <summary>
        /// 记录插入日期
        /// </summary>		
        public DateTime insertTime { get; set; }

        /// <summary>
        /// 应付金额
        /// </summary>		
        public decimal AbleMoney { get; set; }

        /// <summary>
        /// 收银员ID
        /// </summary>		
        public int CashierID { get; set; }

        /// <summary>
        /// 销售概要表ID
        /// </summary>		
        public int saleID { get; set; }

        /// <summary>
        /// 是否零售
        /// </summary>		
        public int isRetail { get; set; }

        /// <summary>
        /// 操作员时间
        /// </summary>		
        public DateTime operatorTime { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>		
        public int operatorID { get; set; }

        /// <summary>
        /// 是否退货
        /// </summary>		
        public int returnStatus { get; set; }

        /// <summary>
        /// 退货原因
        /// </summary>		
        public int returnFlag { get; set; }

        /// <summary>
        /// 退货描述
        /// </summary>		
        public string returnDesc { get; set; }

        /// <summary>
        /// 退货备注信息
        /// </summary>		
        public string returnRemark { get; set; }

        /// <summary>
        /// 退货时间
        /// </summary>		
        public DateTime returnTime { get; set; }

        /// <summary>
        /// 商品信息更新时间
        /// </summary>		
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 与实收金额差值平摊到该商品金额
        /// </summary>		
        public decimal FixMoney { get; set; }

        /// <summary>
        /// pt
        /// </summary>		
        public int pt { get; set; }

        /// <summary>
        /// zt
        /// </summary>		
        public int zt { get; set; }

        /// <summary>
        /// ifpay
        /// </summary>		
        public int ifpay { get; set; }

        /// <summary>
        /// 是否商品扩展属性
        /// </summary>
        public int IsExtend { get; set; }

        /// <summary>
        /// 商品Sku属性Id
        /// </summary>
        public int SkuId { get; set; }

        /// <summary>
        /// 计次卡Id
        /// </summary>
        public int TimeCardId { get; set; }
    }
}
