using System;
using System.Collections.Generic;
using Script.I200.Entity.Model.User;

namespace Script.I200.Entity.Api.Users
{
    /// <summary>
    /// 店铺会员详情
    /// </summary>
    public class UserDetail:UserListDetails
    {
        /// <summary>
        /// 会员生日(yyyy-MM-dd)
        /// </summary>
        public UserBirthday Birthday { get; set; }

        /// <summary>
        /// QQ号码
        /// </summary>
        public string QQ { get; set; }

        /// <summary>
        /// 微信号码
        /// </summary>
        public string WeChat { get; set; }


        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegTime { get; set; }

        /// <summary>
        /// 会员备注
        /// </summary>
        public string Remark { get; set; }
        
        /// <summary>
        /// 会员累计积分
        /// </summary>
        public int UserIntegralAll { get; set; }

        
        /// <summary>
        /// 其他的纪念日（只显示不修改）
        /// </summary>
        public List<UserBirthday> OtherMemorialDay { get; set; }
        
    }

    
}
