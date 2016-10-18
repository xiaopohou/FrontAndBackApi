using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.User
{
    [Table("T_Sms_List")]
    public class UserSms
    {
        [Key]
        [Identity]
        public int Id { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string phoneNum { get; set; }

        /// <summary>
        /// 发送内容
        /// </summary>
        public string smsContent { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime sendtime { get; set; }

        /// <summary>
        /// 店铺号
        /// </summary>
        public int accID { get; set; }

        /// <summary>
        /// 发送类型
        /// </summary>
        public int smsType { get; set; }

        /// <summary>
        /// 发送状态
        /// </summary>
        public int smsStatus { get; set; }

        public int smsChannel { get; set; }

        /// <summary>
        /// 会员id
        /// </summary>
        public int userID { get; set; }

        public int priority { get; set; }

        public int realCnt { get; set; }

        public int notifyID { get; set; }

        /// <summary>
        /// 是否免费 0-收费 1-免费
        /// </summary>
        public int isFree { get; set; }

        /// <summary>
        /// 失败错误描述
        /// </summary>
        public string errDesc { get; set; }

        public string smsReport { get; set; }

        public int Seqid { get; set; }

        public int IspType { get; set; }
    }
}
