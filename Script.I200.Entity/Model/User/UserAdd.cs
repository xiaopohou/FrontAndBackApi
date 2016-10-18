using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.User
{
    /// <summary>
    /// 会员新增
    /// </summary>
    [Table("T_UserInfo")]
    public class UserHandle
    {
        public UserHandle()
        {
            RegTime = DateTime.Now;
            Integral = 0;
            IntegralUsed = 0;
            UserLike = "";
            UserStoreMoney = 0M;
            IsTime = 0;
            KeepTime = 0;
            InsertTime = DateTime.Now;
            StoreTimes = 0;
            Flag = 0;
            MasterId = 0;
        }

        [Key]
        [Identity]
        [Column("uid")]
        public int Id { get; set; }

        public int AccId { get; set; }

        ///<summary>
        ///会员卡号 最长20个字符 
        ///</summary>
        [Required(ErrorMessage = "会员卡号不能为空")]
        [StringLength(maximumLength: 18, ErrorMessage = "会员卡号长度必须小于等于18个字符")]
        [Column("uNumber")]
        public string UserNo { get; set; }
        ///<summary>
        ///会员头像
        ///</summary>
        [Column("uPortrait")]
        public string UserAvatar { get; set; }
        ///<summary>
        ///会员姓名 最长50个字符
        ///</summary>
        [Required(ErrorMessage = "会员姓名不能为空")]
        [StringLength(maximumLength: 20, ErrorMessage = "会员姓名长度必须小于等于20个字符")]
        [Column("uName")]
        public string UserName { get; set; }
        ///<summary>
        ///会员称谓 （通过 v0/user/getusernicknames 获取下拉框数据）
        ///</summary>
        [Column("uNick")]
        public int NickId { get; set; }
        ///<summary>
        ///会员手机号
        ///</summary>
        [Required(ErrorMessage = "会员手机号不能为空")]
        [StringLength(maximumLength: 20, ErrorMessage = "会员手机号长度必须小于等于20个字符")]
        [Column("uPhone")]
        public string UserPhone { get; set; }
        ///<summary>
        ///会员等级   （通过 v0/user/getusergrade 获取下拉框数据）
        ///</summary>
        [Column("uRank")]
        public int UserGrade { get; set; }
        ///<summary>
        ///会员分组   （通过 v0/user/getusergroups 获取下拉框数据）
        ///</summary>
        [Column("uGroup")]
        public int UserGroup { get; set; }
        ///<summary>
        ///备注 
        ///</summary>
        [Column("uRemark")]
        [StringLength(maximumLength: 400, ErrorMessage = "备注长度必须小于等于400个字符")]
        public string Remark { get; set; }
        ///<summary>
        ///是否发送欢迎短信( true: 发送 false: 不发送)
        ///</summary>
        [NotMapped]
        public bool IsSendMsg { get; set; }
        ///<summary>
        ///总店ID
        ///</summary>
        [NotMapped]
        public int MasterId { get; set; }
        ///<summary>
        ///会员生日(yyyy-MM-dd)
        ///</summary>
        [NotMapped]
        public UserBirthday Birthday { get; set; }
        ///<summary>
        ///其它电话号码
        ///</summary>
        [Column("uOtherPhone")]
        public string OtherPhone { get; set; }
        ///<summary>
        ///邮箱 
        ///</summary>
        [StringLength(maximumLength: 50, ErrorMessage = "邮箱长度必须小于等于50个字符")]
        [Column("uEmail")]
        public string Email { get; set; }
        ///<summary>
        ///QQ号码
        ///</summary>
        [StringLength(maximumLength: 50, ErrorMessage = "QQ号码长度必须小于等于50个字符")]
        [Column("uQQ")]
        public string QQ { get; set; }
        ///<summary>
        ///微信号码
        ///</summary>
        [StringLength(maximumLength: 50, ErrorMessage = "微信号码长度必须小于等于50个字符")]
        [Column("weixin")]
        public string WeChat { get; set; }
        ///<summary>
        ///地址
        ///</summary>
        [StringLength(maximumLength: 200, ErrorMessage = "地址码长度必须小于等于200个字符")]
        [Column("uAddress")]
        public string Address { get; set; }
        /// <summary>
        /// 拼音
        /// </summary>
        [Column("uPinYin")]
        public string PinYin { get; set; }
        /// <summary>
        /// 拼音缩写
        /// </summary>
        [Column("uPY")]
        public string PY { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [Column("uSex")]
        public int Sex { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        [Column("uRegTime")]
        public DateTime RegTime { get; set; }
        /// <summary>
        /// 积分数
        /// </summary>
        [Column("uIntegral")]
        public int Integral { get; set; }
        /// <summary>
        /// 已使用积分数
        /// </summary>
        [Column("uIntegralUsed")]
        public int IntegralUsed { get; set; }
        /// <summary>
        /// 会员喜好
        /// </summary>
        [Column("uLike")]
        public string UserLike { get; set; }
        /// <summary>
        /// 会员储值余额
        /// </summary>
        [Column("uStoreMoney")]
        public decimal UserStoreMoney { get; set; }
        public int IsTime { get; set; }
        public int KeepTime { get; set; }
        /// <summary>
        /// 插入时间
        /// </summary>
        [Column("uInsertTime")]
        public DateTime InsertTime { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        [Column("uOperator")]
        public int OperatorId { get; set; }
        /// <summary>
        /// 计次卡次数
        /// </summary>
        [Column("uStoreTimes")]
        public int StoreTimes { get; set; }
        /// <summary>
        /// 上次购物时间
        /// </summary>
        [NotMapped]
        public DateTime? UserLastBuyDate { get; set; }
        /// <summary>
        /// GradeId!=1时为1，其他为0
        /// </summary>
        [Column("uLockRank")]
        public int UserLockRank { get; set; }
        /// <summary>
        /// 来源标记 0-网页 1-iPhone 2-Android 3-iPad
        /// todo:不明确是否还使用
        /// </summary>
        [Column("uFlag")]
        public int Flag { get; set; }
    }
}
