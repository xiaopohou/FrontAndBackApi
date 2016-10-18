using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.Account
{
     [Table("T_Business")]
    public class Business
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 店铺ID
        /// </summary>		
        [Key]
        [Identity]
        public int accountid { get; set; }
	
        public string randomnumber { get; set; }
	
        public int dxborthday { get; set; }

        public int dxreguser { get; set; }

        public int dxelectron { get; set; }

        /// <summary>
        /// 最大会员数量[关键]
        /// </summary>		
        public int gsreguser { get; set; }

        /// <summary>
        /// 短信剩余条数[关键]
        /// </summary>		
        public int dxunity { get; set; }

        /// <summary>
        /// 店铺版本[关键][0,1-免费 2-标 3-高]
        /// </summary>		
        public int aotjb { get; set; }

        /// <summary>
        /// 短信状态
        /// </summary>		
        public int dxzt { get; set; }

        /// <summary>
        /// 最在会员ID号
        /// </summary>		
        public int new_reguser { get; set; }

        /// <summary>
        /// 当日销售
        /// </summary>		
        public int drxs { get; set; }

        /// <summary>
        /// 短信配置
        /// </summary>		
        public int dxpz { get; set; }

        /// <summary>
        /// 会员注册
        /// </summary>		
        public int hyzc { get; set; }

        /// <summary>
        /// 版本开始时间[关键]
        /// </summary>		
        public DateTime starttime { get; set; }

        /// <summary>
        /// 版本结束时间[关键]
        /// </summary>		
        public DateTime endtime { get; set; }

        /// <summary>
        /// 邀请短信数量[?]
        /// </summary>		
        public int insms { get; set; }

        /// <summary>
        /// 邀请邮件数量[?]
        /// </summary>		
        public int inemail { get; set; }

        /// <summary>
        /// 短信可免费申请数量
        /// </summary>		
        public int sms_apply_num { get; set; }

        /// <summary>
        /// 店铺积分
        /// </summary>		
        public int integral { get; set; }

        /// <summary>
        /// 店铺活跃状态
        /// </summary>		
        public int active { get; set; }

        /// <summary>
        /// 绑定短信通道号
        /// </summary>		
        public int sms_channel { get; set; }

        /// <summary>
        /// 免费短信申请次数
        /// </summary>		
        public int Sms_use_num { get; set; }

        /// <summary>
        /// 店铺可初始化次数[1-默认值]
        /// </summary>		
        public int dataInitCnt { get; set; }

        /// <summary>
        /// 回访状态
        /// </summary>		
        public int revisitStatus { get; set; }

        public int Temp_SmsBuyNum { get; set; }

        /// <summary>
        /// 导出数据验证
        /// </summary>
        public int DataExport { get; set; }

        /// <summary>
        /// 安全提醒
        /// </summary>
        public int SecurityRemind { get; set; }

        /// <summary>
        /// 商品预警数量
        /// </summary>
        public decimal goodsWarning { get; set; }

        /// <summary>
        /// 自定义模板打印logo类别
        /// 0：不打印
        /// 1：打印店铺logo
        /// 2：打印自定义小票logo
        /// </summary>
        public string PrintLogoShowType { get; set; }
    }
}
