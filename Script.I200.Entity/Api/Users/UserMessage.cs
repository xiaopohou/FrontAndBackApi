using System;

namespace Script.I200.Entity.Api.Users
{
    /// <summary>
    /// 店铺会员短消息
    /// </summary>
    public class UserMessage
    {
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// 发送内容
        /// </summary>
        public string Content { get; set; }
    }
}
