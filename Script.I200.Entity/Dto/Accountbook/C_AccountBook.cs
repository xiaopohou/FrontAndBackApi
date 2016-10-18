using System;
using System.Collections.Generic;

namespace Script.I200.Entity.Dto.Accountbook
{
    /// <summary>
    /// 店铺账号信息表
    /// </summary>
    public class C_AccountBook
    {
        /// <summary>
        /// 账簿编号
        /// </summary>		
        public int Id { get; set; }
        /// <summary>
        /// 店铺编号
        /// </summary>		
        public int AccountId { get; set; }
        /// <summary>
        /// 某种总金额款项；如：可提现
        /// </summary>		
        public decimal Money { get; set; }
        /// <summary>
        /// 总金额类型；LOV：0=可提现；1=无法提现
        /// </summary>		
        public int Type { get; set; }
        /// <summary>
        /// 账户状态；LOV：0=正常账号，1=冻结，2=锁定
        /// </summary>		
        public int Status { get; set; }
        /// <summary>
        /// 账簿备注账号等级；LOV：0~5
        /// </summary>		
        public int Flag { get; set; }
        /// <summary>
        /// 账簿备注
        /// </summary>		
        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>		
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 某种总金额款项(被冻结)；如：可提现
        /// </summary>		
        public decimal TotalFrozenPayMoney { get; set; }


        /// <summary>
        /// 交易总金额分类汇总
        /// </summary>
        public List<C_BillingAmount> BillingBusinessList { get; set; }


    }
}
