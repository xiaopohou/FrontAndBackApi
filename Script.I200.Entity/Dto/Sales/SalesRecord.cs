using System;
using System.Collections.Generic;

namespace Script.I200.Entity.Dto.Sales
{
    /// <summary>
    /// 历史销售记录列表
    /// </summary>
    public class SalesRecord
    {
        /// <summary>
        /// 销售概要ID
        /// </summary>		
        public int saleID { get; set; }

        /// <summary>
        /// 销售流水号
        /// </summary>		
        public string saleNo { get; set; }

        /// <summary>
        /// 是否零售
        /// </summary>		
        public int isRetail { get; set; }

        /// <summary>
        /// 销售类型(零售/会员)
        /// </summary>
        public string salesType { get; set; }

        /// <summary>
        /// 会员ID
        /// </summary>		
        public int uid { get; set; }

        /// <summary>
        /// 会员姓名
        /// </summary>
        public string userName { get; set; }

        /// <summary>
        /// 销售时间
        /// </summary>		
        public DateTime saleTime { get; set; }

        /// <summary>
        /// 销售时间(HH:mm)
        /// </summary>
        public string saleShortTime { get; set; }

        /// <summary>
        /// 记录插入时间
        /// </summary>		
        public DateTime insertTime { get; set; }

        /// <summary>
        /// 商品种类
        /// </summary>		
        public int saleKind { get; set; }

        /// <summary>
        /// 销售数量
        /// </summary>		
        public double saleNum { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>		
        public int payType { get; set; }

        /// <summary>
        /// 支付方式名称
        /// </summary>
        public string payTypeName { get; set; }

        /// <summary>
        /// 应付金额
        /// </summary>		
        public decimal AbleMoney { get; set; }

        /// <summary>
        /// 实付金额
        /// </summary>		
        public decimal RealMoney { get; set; }

        /// <summary>
        /// 应付与实付金额差值
        /// </summary>		
        public decimal DiffMoney { get; set; }

        /// <summary>
        /// StoreTimes
        /// </summary>		
        public int StoreTimes { get; set; }

        /// <summary>
        /// 储值支付金额
        /// </summary>		
        public decimal StoreMoney { get; set; }

        /// <summary>
        /// 现金支付金额
        /// </summary>		
        public decimal CashMoney { get; set; }

        /// <summary>
        /// CardMoney
        /// </summary>		
        public decimal CardMoney { get; set; }

        /// <summary>
        /// 未支付金额
        /// </summary>		
        public decimal UnpaidMoney { get; set; }

        /// <summary>
        /// 优惠券金额
        /// </summary>
        public int? CouponMoney { get; set; }

        /// <summary>
        /// 流水单号
        /// </summary>
        public long SerialNum { get; set; }

        /// <summary>
        /// 销售详细列表
        /// </summary>
        public List<SalesDetail> SalesList { get; set; }
        /// <summary>
        /// 挂单名称
        /// </summary>
        public string CartReName { get; set; }
        /// <summary>
        /// 积分兑换金额
        /// </summary>
        public decimal IntegralMoney { get; set; }
        /// <summary>
        /// 消耗积分数
        /// </summary>
        public decimal IntegralUsed { get; set; }

        /// <summary>
        /// 整单备注
        /// </summary>
        public string WholeRemark { get; set; }

        /// <summary>
        /// 支付状态 0:等待 1:成功 -10:确认失败 -11:用户支付中 -1:错误
        /// </summary>
        public int PayStatus { get; set; }

        /// <summary>
        /// 支付状态描述
        /// </summary>
        public string PayStatusDesc { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 微信支付订单号
        /// </summary>
        public string TradeNo { get; set; }

    }

    /// <summary>
    /// 历史销售详细列表
    /// </summary>
    public class SalesDetail
    {
        /// <summary>
        /// 销售列表ID
        /// </summary>		
        public int saleListID { get; set; }

        /// <summary>
        /// saleID
        /// </summary>		
        public int saleID { get; set; }

        /// <summary>
        /// 流水号
        /// </summary>		
        public string saleNo { get; set; }

        /// <summary>
        /// 会员ID
        /// </summary>		
        public int userID { get; set; }

        /// <summary>
        /// 会员姓名
        /// </summary>
        public string userName { get; set; }

        /// <summary>
        /// 是否零售
        /// </summary>		
        public int isRetail { get; set; }

        /// <summary>
        /// 大分类名称
        /// </summary>		
        public string maxClass { get; set; }

        /// <summary>
        /// 小分类名称
        /// </summary>		
        public string minClass { get; set; }

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
        /// 商品名称
        /// </summary>		
        public string GoodsName { get; set; }

        /// <summary>
        /// 商品规格
        /// </summary>		
        public string Specification { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>		
        public decimal GoodsNum { get; set; }

        /// <summary>
        /// 商品单价
        /// </summary>		
        public decimal Price { get; set; }

        /// <summary>
        /// 商品折扣
        /// </summary>		
        public decimal Discount { get; set; }

        /// <summary>
        /// 应付金额
        /// </summary>		
        public decimal AbleMoney { get; set; }

        /// <summary>
        /// 与实收金额差值平摊到该商品金额
        /// </summary>		
        public decimal FixMoney { get; set; }

        /// <summary>
        /// 实收金额
        /// </summary>		
        public decimal RealMoney { get; set; }

        /// <summary>
        /// 是否积分
        /// </summary>		
        public int isIntegral { get; set; }

        /// <summary>
        /// 收银员
        /// </summary>		
        public string CashierName { get; set; }
        /// <summary>
        /// 收银员ID
        /// </summary>
        public int CashierID { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>		
        public string Remark { get; set; }

        /// <summary>
        /// 销售日期
        /// </summary>		
        public DateTime saleTime { get; set; }
        /// <summary>
        /// 年月日时间
        /// </summary>
        public string saleTimeYMD { get; set; }

        /// <summary>
        /// 销售时间(HH:mm)
        /// </summary>
        public string saleShortTime { get; set; }

        /// <summary>
        /// 记录插入日期
        /// </summary>		
        public DateTime insertTime { get; set; }

        /// <summary>
        /// 退货标记
        /// </summary>
        public int returnStatus { get; set; }

        /// <summary>
        /// 退货类型
        /// </summary>
        public int returnFlag { get; set; }

        /// <summary>
        /// 退货原因
        /// </summary>
        public string returnReason { get; set; }

        /// <summary>
        /// 退货时间
        /// </summary>
        public DateTime returnTime { get; set; }

        /// <summary>
        /// 换货标记 1-被换 2-换
        /// </summary>
        public int exchangesFlag { get; set; }
        /// <summary>
        /// 换货补差额
        /// </summary>
        public decimal exchangesMakingUpDifferences { get; set; }
        /// <summary>
        /// 换货Id
        /// </summary>
        public int exchangesSalesListId { get; set; }
        //被换商品名
        public string exchangesGoodsName { get; set; }
        /// <summary>
        /// 补差额说明
        /// </summary>
        public string exchangesMakingUpDiffStr { get; set; }

        /// <summary>
        /// 流水单号
        /// </summary>
        public long SerialNum { get; set; }

        /// <summary>
        /// 商品图片 第一张图片 （因为 已经使用 不能舍弃）
        /// </summary>
        public string PicUrl { get; set; }

        /// <summary>
        /// 商品图片列表
        /// </summary>
        public List<T_GoodsPicBasis> PicUrls { get; set; }

        /// <summary>
        /// 商品进价
        /// </summary>
        public decimal CostPrice { get; set; }

        /// <summary>
        /// 计次卡Id
        /// </summary>
        public int TimeCardId { get; set; }
    }

    /// <summary>
    /// 商品图片基础信息
    /// </summary>
    public partial class T_GoodsPicBasis
    {

        /// <summary>
        /// id
        /// </summary>		
        public int id { get; set; }

        /// <summary>
        /// 产品图片
        /// </summary>		
        public string gPicUrl { get; set; }

        /// <summary>
        /// 图片排序
        /// </summary>		
        public int gPicOrder { get; set; }
    }
}
