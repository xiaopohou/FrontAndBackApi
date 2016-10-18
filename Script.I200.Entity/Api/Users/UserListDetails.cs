using System;
using System.Collections.Generic;

namespace Script.I200.Entity.Api.Users
{
    public class UserListDetails
    {
        /// <summary>
        /// 会员ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 店铺ID
        /// </summary>
        public int AccId { get; set; }

        /// <summary>
        /// 会员卡号 最长20个字符
        /// </summary>
        public string UserNo { get; set; }

        /// <summary>
        /// 会员姓名 最长50个字符
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 会员最后一次消费时间
        /// </summary>
        public DateTime UserLastButDate { get; set; }

        /// <summary>
        /// 会员手机号
        /// </summary>
        public string UserPhone { get; set; }

        /// <summary>
        /// 店铺会员标签
        /// </summary>
        public List<UserTagsGet> Tags { get; set; }

        /// <summary>
        /// 会员头像
        /// </summary>
        public string UserAvatar { get; set; }
        /// <summary>
        /// 会员等级ID
        /// </summary>
        public int UserGradeId { get; set; }

        /// <summary>
        /// 会员分组ID
        /// </summary>
        public int UserGroupId { get; set; }

        /// <summary>
        /// 等级名称
        /// </summary>
        public string UserGradeName { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        public string UserGroupName { get; set; }

        /// <summary>
        /// 会员可用积分
        /// </summary>
        public int UserIntegral { get; set; }
        public decimal UserStoreMoney { get; set; }
        public decimal UserUnpaidMoney { get; set; }
        public int UserTimesCardCount { get; set; }
    }
}
