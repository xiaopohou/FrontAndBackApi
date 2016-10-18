namespace Script.I200.Entity.Enum
{
    /// <summary>
    /// 用户积分筛选类型
    /// </summary>
    public enum UserIntegralSearchTypeEnum
    {
        /// <summary>
        /// 全部
        /// </summary>
        All = -1,

        /// <summary>
        /// 获取
        /// </summary>
        Get = 1,

        /// <summary>
        /// 使用
        /// </summary>
        Used = 2
    }

    /// <summary>
    /// 用户积分设置类型
    /// </summary>
    public enum UserIntegralSetTypeEnum
    {
        /// <summary>
        /// 兑换
        /// </summary>
        Exchange = 1,

        /// <summary>
        /// 修改
        /// </summary>
        Change = 2,

        /// <summary>
        /// 添加（会员退货）
        /// </summary>
        Add = 3,
    }
}
