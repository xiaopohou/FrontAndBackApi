using System;
using System.Collections.Generic;
using Script.I200.Entity.Api.Accountbook;
using Script.I200.Entity.API;
using Script.I200.Entity.Dto.Accountbook;
using Script.I200.Entity.Enum;

namespace Script.I200.Application.Proxy
{
    public class AccountbookProxyService : BaseProxyService, IAccountbookProxyService
    {
        /// <summary>
        /// 增加(绑定)提现账户
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public ResponseSerializationModel<C_AccountCreditCardShowDto> AddWithdrawingAccount(UserContext userContext,
            WithdrawingAccountRequest account)
        {
            var requestEntity = account == null ? null : account.ToDto();
            return RestPost<C_AccountCreditCardShowDto, AccountCreditCardDto>(BaseUrl + "AccountCreditCard", userContext,
                requestEntity,
                RestHead(userContext, BankPlatBusinessEnum.Withdrawal));
        }

        /// <summary>
        /// 获取提现账户列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public ResponseSerializationModel<List<C_AccountCreditCardShowDto>> GetWithdrawingAccountList(
            UserContext userContext)
        {
            return RestGet<List<C_AccountCreditCardShowDto>>(BaseUrl + "AccountCreditCard", userContext,
                RestHead(userContext, BankPlatBusinessEnum.Withdrawal));
        }


        /// <summary>
        /// 获取提现记录列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="searchParam"></param>
        /// <returns></returns>
        public ResponseSerializationModel<PageDataView<C_AccountWithdrawalsShowDto>> SearchWithdrawingJournals(
            UserContext userContext,
            WithdrawingJournalSearchParam searchParam)
        {
            var requestEntity = searchParam == null ? null : searchParam.ToDto();
            return
                RestPost<PageDataView<C_AccountWithdrawalsShowDto>, C_SearchParams>(
                    BaseUrl + "AccountWithdrawals/Search", userContext, requestEntity,
                    RestHead(userContext, BankPlatBusinessEnum.Withdrawal));
        }

        /// <summary>
        /// 获取收单记录列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="searchParam"></param>
        /// <returns></returns>
        public ResponseSerializationModel<PageDataView<C_Billing_JournalOutBase>> SearchBillingJournals(
            UserContext userContext, BillingJournalSearchParam searchParam)
        {
            var requestEntity = searchParam == null ? null : searchParam.ToDto();
            return RestPost<PageDataView<C_Billing_JournalOutBase>, C_SearchParams>(BaseUrl + "BillingJournal/Search",
                userContext, requestEntity,
                RestHead(userContext, BankPlatBusinessEnum.Acquir));
        }

        /// <summary>
        ///  获取资金详情列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="searchResult"></param>
        /// <returns></returns>
        public ResponseSerializationModel<PageDataView<C_Account_AccountBook_Log_Record>> SearchAccountbookJournals(
            UserContext userContext,
            WithdrawingJournalSearchParam searchResult)
        {
            var requestEntity = searchResult == null ? null : searchResult.ToDto();
            return
                RestPost<PageDataView<C_Account_AccountBook_Log_Record>, C_SearchParams>(
                    BaseUrl + "AccountBookLog/Search", userContext, requestEntity,
                    RestHead(userContext, BankPlatBusinessEnum.Withdrawal));
        }

        /// <summary>
        /// 获取支付方式列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public ResponseSerializationModel<List<C_BillingThirdPartyPaymentDto>> GetPaymentTypeList(
            UserContext userContext)
        {
            var type = Convert.ToInt32(PaymentTypeEnum.Settlement);
            return RestGet<List<C_BillingThirdPartyPaymentDto>>(BaseUrl + "BillingThirdPartyPayment/type/" + type + "",
                userContext,
                RestHead(userContext, BankPlatBusinessEnum.Withdrawal));
        }

        /// <summary>
        /// 获取提现账户余额
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public ResponseSerializationModel<C_AccountBook> GetWithdrawingBalance(UserContext userContext)
        {
            return RestGet<C_AccountBook>(BaseUrl + "AccountBook", userContext,
                RestHead(userContext, BankPlatBusinessEnum.Withdrawal));
        }

        /// <summary>
        /// 新增店铺提现流水
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseSerializationModel<string> AddWithdrawingJournal(UserContext userContext, NewWithdrawingJournalRequest request)
        {
            var requestEntity = request == null ? null : request.ToDto();
            return RestPost<string, AccountWithdrawalsDto>(BaseUrl + "AccountWithdrawals", userContext, requestEntity,
                RestHead(userContext, BankPlatBusinessEnum.Withdrawal));
        }

        /// <summary>
        /// 获取提现额外信息
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public ResponseSerializationModel<AccountWithdrawalsDto> GetExtraWithdrawingJournalInfo(UserContext userContext)
        {
            return RestGet<AccountWithdrawalsDto>(BaseUrl + "AccountCreditCard", userContext,
                RestHead(userContext, BankPlatBusinessEnum.Withdrawal));
        }

        /// <summary>
        /// 业务列表记录
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public ResponseSerializationModel<List<C_BillingBusinessShowDto>> GetBillingBusiness(UserContext userContext)
        {
            return RestGet<List<C_BillingBusinessShowDto>>(BaseUrl + "BillingBusiness", userContext,
                RestHead(userContext, BankPlatBusinessEnum.Withdrawal));
        }
    }
}