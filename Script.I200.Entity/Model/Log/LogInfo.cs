using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.Log
{
    [Table("T_LogInfo")]
    public class LogInfo
    {
        /// <summary>
        /// 编号
        /// </summary>		
        [Key]
        [Identity]
        public int id { get; set; }

        /// <summary>
        /// 店铺ID
        /// </summary>		
        public long accID { get; set; }

        /// <summary>
        /// 记录类型 
        /// <para>{1:积分消费,2:抽奖活动,3:'',4:营销活动,5:支持生意专家,6:分享生意专家,7:每日签到,8:每日心情,9:关注微信,10:推荐好友}</para>
        /// </summary>		
        public int LogType { get; set; }

        /// <summary>
        /// 更改内容
        /// </summary>		
        public string Keys { get; set; }

        /// <summary>
        /// 原始值
        /// </summary>		
        public string OriginalVal { get; set; }

        /// <summary>
        /// 修改值
        /// </summary>		
        public string EditVal { get; set; }

        /// <summary>
        /// 最终值
        /// </summary>		
        public string FinialVal { get; set; }

        /// <summary>
        /// 记录时间
        /// </summary>		
        public DateTime CreatTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>		
        public string ReMark { get; set; }

        /// <summary>
        /// Flags
        /// </summary>		
        public string Flags { get; set; }

    }
}
