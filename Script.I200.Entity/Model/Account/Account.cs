using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.Account
{
    /// <summary>
    ///     店铺账号信息（B端）
    /// </summary>
    [Table("T_Account")]
    public class Account
    {
        /// <summary>
        ///     Id
        /// </summary>
        [Key]
        [Identity]
        public long Id { get; set; }

        /// <summary>
        ///     真实姓名
        /// </summary>
        public string UserRealName { get; set; }

        /// <summary>
        ///     用户密码
        /// </summary>
        public string Userpasswd { get; set; }

        /// <summary>
        ///     手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        ///     用户邮件
        /// </summary>
        public string Useremail { get; set; }

        /// <summary>
        ///     随机数
        /// </summary>
        public string RandomNumber { get; set; }

        /// <summary>
        ///     店铺名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        ///     店铺地址
        /// </summary>
        public string CompanyAddress { get; set; }

        /// <summary>
        ///     所属服务商
        /// </summary>
        public string ServiceManager { get; set; }

        /// <summary>
        ///     注册时间
        /// </summary>
        public DateTime RegTime { get; set; }

        /// <summary>
        ///     Web端登陆时间
        /// </summary>
        public int LoginTimeWeb { get; set; }

        /// <summary>
        ///     上一次登录时间
        /// </summary>
        public DateTime LoginTimeLast { get; set; }

        /// <summary>
        ///     状态
        /// </summary>
        public int State { get; set; }

        /// <summary>
        ///     身份证号码
        /// </summary>
        public string SfzNumber { get; set; }

        /// <summary>
        /// </summary>
        public int Dxstatus { get; set; }

        /// <summary>
        ///     积分规则
        /// </summary>
        public string Proportion { get; set; }

        public string Nexttotal { get; set; }
        public int Reguserid { get; set; }

        /// <summary>
        ///     店铺简称
        /// </summary>
        public string ShotName { get; set; }

        public string Subjection { get; set; }

        /// <summary>
        ///     是否是总店（如果是总店，当前的maxShop为当前店铺Id）
        /// </summary>
        [Column("max_shop")]
        public int MaxShop { get; set; }

        public DateTime Logintimebreak { get; set; }
        public string Loginbrslast { get; set; }
        public string BbSusername { get; set; }
        public int EmailChk { get; set; }
        public int PhoneChk { get; set; }

        [Column("Parent_AccountId")]
        public int ParentAccountId { get; set; }

        [Column("Reg_Code")]
        public string RegCode { get; set; }
        public int Type { get; set; }

        /// <summary>
        ///     图片Url
        /// </summary>
        public string Imgurl { get; set; }

        [Column("manager_name")]
        public string ManagerName { get; set; }

        /// <summary>
        ///     服务商Id
        /// </summary>
        public string AgentId { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 店铺类型
        /// </summary>
        public string ShopType { get; set; }

        [Column("py_full")]
        public string PyFull { get; set; }

        [Column("py_code")]
        public string PyCode { get; set; }

        /// <summary>
        /// 指导步骤
        /// </summary>
        [Column("Guider_Step")]
        public string GuiderStep { get; set; }

        [Column("BBS_Uid")]
        public int BbsUid { get; set; }

        [Column("weixin_openid")]
        public string WeixinOpenid { get; set; }

        /// <summary>
        ///     推荐人Id
        /// </summary>
        public int RecommendId { get; set; }

        public string Flag { get; set; }

        /// <summary>
        /// </summary>
        public string FromName { get; set; }

        /// <summary>
        ///     证件Url
        /// </summary>
        public string BusinessLicenseUrl { get; set; }

        /// <summary>
        ///     当前店铺版本
        /// </summary>
        public int UserPowerVersion { get; set; }

        /// <summary>
        ///     企业Id
        /// </summary>
        public string EnterpriseId { get; set; }
    }
}