using System;
using System.ComponentModel;
using System.Linq;

namespace Script.I200.Entity.Enum
{
    public class UserBehaviorConfig
    {
        public int ConfigCount { get; set; }
        public UserBehaviorEventTypeEnum EventType { get; set; }
    }


    [AttributeUsage(AttributeTargets.Field)]
    public sealed class UserBehaviorConfigAttribute : Attribute
    {
        public UserBehaviorConfigAttribute(int configCount, UserBehaviorEventTypeEnum eventType)
        {
            ConfigCount = configCount;
            EventType = eventType;
        }

        public int ConfigCount { get; private set; }
        public UserBehaviorEventTypeEnum EventType { get; private set; }
    }


    /// <summary>
    ///     用户点击提示消息名称枚举
    /// </summary>
    public enum UserBehaviorNameEnum
    {
        /// <summary>
        ///     客服联系地址
        /// </summary>
        [Description("客服联系地址")] [UserBehaviorConfig(1, UserBehaviorEventTypeEnum.Click)] CustomerServiceConnection = 4
    }

    public static class UserBehaviorNameEnumEntensions
    {
        public static UserBehaviorConfig GetConfig(this UserBehaviorNameEnum eventType)
        {
            var type = typeof (UserBehaviorNameEnum);
            var memInfo = type.GetMember(UserBehaviorNameEnum.CustomerServiceConnection.ToString());
            var attribute =
                memInfo[0].GetCustomAttributes(typeof (UserBehaviorConfigAttribute), false).First() as
                    UserBehaviorConfigAttribute;
            return attribute == null
                ? null
                : new UserBehaviorConfig
                {
                    ConfigCount = attribute.ConfigCount,
                    EventType = attribute.EventType
                };
        }
    }
}