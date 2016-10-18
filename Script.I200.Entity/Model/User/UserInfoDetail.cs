using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.User
{
    [Table("T_UserInfo")]
    public class UserInfoDetail
    {
        /// <summary>
        ///     用户Id（C端用户）
        /// </summary>
        [Key]
        [Identity]
        public int Uid { get; set; }

        /// <summary>
        ///     用户编号
        /// </summary>
        public string UNumber { get; set; }

        /// <summary>
        ///     用户密码
        /// </summary>
        public string UPwd { get; set; }

        /// <summary>
        ///     用户姓名
        /// </summary>
        public string UName { get; set; }

        /// <summary>
        ///     用户性别
        /// </summary>
        public int USex { get; set; }

        /// <summary>
        ///     用户邮箱
        /// </summary>
        public string UEmail { get; set; }

        /// <summary>
        ///     用户电话号码
        /// </summary>
        public string UPhone { get; set; }

        /// <summary>
        ///     用户注册时间
        /// </summary>
        public DateTime URegTime { get; set; }

        /// <summary>
        ///     用户QQ
        /// </summary>
        public string Uqq { get; set; }

        /// <summary>
        ///     用户积分
        /// </summary>
        public int UIntegral { get; set; }

        /// <summary>
        ///     用户已使用积分
        /// </summary>
        public int UIntegralUsed { get; set; }

        /// <summary>
        ///     店铺Id
        /// </summary>
        public int AccId { get; set; }

        /// <summary>
        ///     用户地址
        /// </summary>
        public string UAddress { get; set; }

        /// <summary>
        ///     用户爱好
        /// </summary>
        public string ULike { get; set; }

        /// <summary>
        ///     用户备注信息
        /// </summary>
        public string URemark { get; set; }

        /// <summary>
        ///     用户储值余额
        /// </summary>
        public decimal UStoreMoney { get; set; }

        /// <summary>
        ///     用户分组
        /// </summary>
        public int UGroup { get; set; }

        /// <summary>
        ///     用户拼音首字母缩写
        /// </summary>
        public string Upy { get; set; }

        /// <summary>
        ///     用户拼音缩写
        /// </summary>
        public string UPinYin { get; set; }

        public int IsTime { get; set; }
        public int KeepTime { get; set; }
        public int UNick { get; set; }
        public int URank { get; set; }
        public DateTime UInsertTime { get; set; }

        /// <summary>
        ///     操作人
        /// </summary>
        public int UOperator { get; set; }

        /// <summary>
        ///     用户储值次数
        /// </summary>
        public int UStoreTimes { get; set; }

        /// <summary>
        ///     用户上一次购买时间
        /// </summary>
        public DateTime ULastBuyDate { get; set; }

        public string UAuthCode { get; set; }
        public DateTime UAuthCodeTime { get; set; }
        public int ULockRank { get; set; }
        public string UPortrait { get; set; }
        public string Weixin { get; set; }
        public string AliPay { get; set; }
        public int UFlag { get; set; }
        public string UOtherPhone { get; set; }

        [NotMapped]
        public int OwnerId { get; set; }
        [NotMapped]
        public string UserGroupName { get; set; }
        [NotMapped]
        public string UserGradeName { get; set; }
    }

}