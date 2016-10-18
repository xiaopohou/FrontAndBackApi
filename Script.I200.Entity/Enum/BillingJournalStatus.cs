namespace Script.I200.Entity.Enum
{
    public enum BillingJournalStatus
    {
        /// <summary>
        ///关闭（无效）（关闭提现，未通过审核，支付失败）
        /// </summary>
        Close = 0,
        /// <summary>
        ///新建（有效）（新建提现）
        /// </summary>
        Create = 1,
        /// <summary>
        ///支付成功
        /// </summary>
        CreateSuccess = 2,
        /// <summary>
        ///支付失败
        /// </summary>
        CreateFailed = 3,
        /// <summary>
        ///冻结资金
        /// </summary>
        FrozenMoney = 4,
        /// <summary>
        ///解冻资金中
        /// </summary>
        UnfrozeningMoney = 5,
        /// <summary>
        ///成功解冻资金
        /// </summary>
        FrozenSuccess = 6,
        /// <summary>
        ///失败解冻资金
        /// </summary>
        FrozenFailed = 7,
        /// <summary>
        ///提现中（审核成功，支付中）
        /// </summary>
        Withdrawing = 8,
        /// <summary>
        ///交易成功（提现成功）
        /// </summary>
        JournalSuccess = 1000
    }
}
