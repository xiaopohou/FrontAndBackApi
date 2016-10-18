using Script.I200.Entity.Api.Accountbook;
using Script.I200.Entity.API;

namespace Script.I200.Application.Accountbook
{
    public interface IAccountbookService
    {
        /// <summary>
        /// 绑定提现账号
        /// </summary>
        /// <param name="userContext">店铺相关信息</param>
        /// <param name="account">提现账号信息</param>
        ResponseModel AddWithdrawingAccount(UserContext userContext, WithdrawingAccountRequest account);

        /// <summary>
        /// 获取提现账户列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        ResponseModel GetWithdrawingAccountList(UserContext userContext);

        /// <summary>
        /// 获取提现记录列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="searchParam"></param>
        /// <returns></returns>
        ResponseModel SearchWithdrawingJournals(UserContext userContext, WithdrawingJournalSearchParam searchParam);

        /// <summary>
        /// 获取收单记录列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="searchParam"></param>
        /// <returns></returns>
        ResponseModel SearchBillingJournals(UserContext userContext, BillingJournalSearchParam searchParam);

        /// <summary>
        /// 获取资金详情列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="searchResult"></param>
        /// <returns></returns>
        ResponseModel SearchAccountbookJournals(UserContext userContext, WithdrawingJournalSearchParam searchResult);

        /// <summary>
        /// 获取支付方式列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        ResponseModel GetPaymentTypeList(UserContext userContext);

        /// <summary>
        /// 获取提现账户余额
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        ResponseModel GetWithdrawingBalance(UserContext userContext);

        /// <summary>
        /// 新增店铺提现流水
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        ResponseModel AddWithdrawingJournal(UserContext userContext,
            NewWithdrawingJournalRequest request);

        /// <summary>
        /// GetBillingBusiness
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        ResponseModel GetBillingBusiness(UserContext userContext);

        /// <summary>
        ///  获取收单和提现状态
        /// </summary>
        /// <returns></returns>
        ResponseModel GetWithdrawingStatusAndBillingStatus();

        /// <summary>
        /// 发送提现验证码
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        ResponseModel SendWithdrawingAuthCode(UserContext userContext);

        /// <summary>
        /// 发送绑定银行卡提现验证码
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        ResponseModel SendBindWithdrawingAccountAuthCode(UserContext userContext, string phone);

        ResponseModel GetCurrentOperatorPower(UserContext userContext, int type);
    }
}