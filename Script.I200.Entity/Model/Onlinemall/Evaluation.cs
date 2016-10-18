using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Script.I200.Entity.Model.Onlinemall
{
    [Table("T_Product_Evaluation")]
    public class MobileEvaluation: MobileEvaluationBase
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public int productID { get; set; }

        /// <summary>
        /// 商品类别,当前0表示虚拟商品，1表示实物
        /// 不同于OnlineMallGoodsTypeEnum，此处特殊处理
        /// </summary>
        public int productType { get; set; }
        /// <summary>
        /// 是否显示(0显示，1不显示，保存时默认为1)
        /// </summary>
        public int isDisplay { get; set; }

        /// <summary>
        /// 是否被删除(0已删除，1未删除，保存时默认为1)
        /// </summary>
        public int isDelete { get; set; }
        /// <summary>
        /// 店铺ID
        /// </summary>
        public int accId { get; set; }
        
    }

    public class MobileEvaluationBase
    {
        /// <summary>
        /// 评论内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 新增时间
        /// </summary>
        public DateTime createTime { get; set; }
        /// <summary>
        /// 虚拟商户名
        /// </summary>
        public string DummyName { get; set; }
    }
}
