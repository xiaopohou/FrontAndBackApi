using System.Collections.Generic;
using Script.I200.Entity.Api.Accountbook;
using Script.I200.Entity.API;
using Script.I200.Entity.Dto.Accountbook;

namespace Script.I200.Application.Proxy
{
    public interface IAccountbookProxyService
    {
        ResponseSerializationModel<C_AccountCreditCardShowDto> AddWithdrawingAccount(UserContext userContext,
            WithdrawingAccountRequest account);

        /// <summary>
        /// 获取提现账户列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        ResponseSerializationModel<List<C_AccountCreditCardShowDto>> GetWithdrawingAccountList(UserContext userContext);

        /// <summary>
        /// 获取提现记录列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="searchParam"></param>
        /// <returns></returns>
        ResponseSerializationModel<PageDataView<C_AccountWithdrawalsShowDto>> SearchWithdrawingJournals(
            UserContext userContext, WithdrawingJournalSearchParam searchParam);

        /// <summary>
        /// 获取收单记录列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="searchParam"></param>
        /// <returns></returns>
        ResponseSerializationModel<PageDataView<C_Billing_JournalOutBase>> SearchBillingJournals(
            UserContext userContext, BillingJournalSearchParam searchParam);

        /// <summary>
        /// 获取资金详情列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="searchResult"></param>
        /// <returns></returns>
        ResponseSerializationModel<PageDataView<C_Account_AccountBook_Log_Record>> SearchAccountbookJournals(
            UserContext userContext, WithdrawingJournalSearchParam searchResult);

        /// <summary>
        /// 获取支付方式列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        ResponseSerializationModel<List<C_BillingThirdPartyPaymentDto>> GetPaymentTypeList(UserContext userContext);

        /// <summary>
        /// 获取提现账户余额
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        ResponseSerializationModel<C_AccountBook> GetWithdrawingBalance(UserContext userContext);

        /// <summary>
        /// 新增店铺提现流水
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        ResponseSerializationModel<string> AddWithdrawingJournal(UserContext userContext,
            NewWithdrawingJournalRequest request);

        /// <summary>
        /// 获取提现额外信息
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        ResponseSerializationModel<AccountWithdrawalsDto> GetExtraWithdrawingJournalInfo(UserContext userContext);

        /// <summary>
        /// 业务列表记录
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        ResponseSerializationModel<List<C_BillingBusinessShowDto>> GetBillingBusiness(UserContext userContext);
    }
}