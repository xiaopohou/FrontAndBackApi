using System.Web.Http;
using Script.I200.Application.Accountbook;
using Script.I200.Entity.Api.Accountbook;
using Script.I200.Entity.Api.Shared;
using Script.I200.Entity.API;
using Script.I200.Entity.Enum;

namespace Script.I200.WebAPI.Controllers
{
    /// <summary>
    /// 资金账户接口
    /// </summary>
    [RoutePrefix("v0/accountbook")]
    public class AccountbookController : BaseApiController
    {
        private readonly IAccountbookService _accountbookService;

        public AccountbookController() : this(new AccountbookService())
        {
        }

        public AccountbookController(IAccountbookService accountbookService)
        {
            _accountbookService = accountbookService;
        }

        /// <summary>
        /// 获取提现账户余额
        /// </summary>
        /// <returns></returns>
        [Route("balance")]
        [HttpGet, HttpOptions]
        public ResponseModel GetBalance()
        {
            var data = _accountbookService.GetWithdrawingBalance(GetUserContext());
            return Success(data);
        }

        /// <summary>
        /// 获取资金详情列表
        /// </summary>
        /// <param name="searchResult"></param>
        /// <returns></returns>
        [Route("journals")]
        [HttpGet, HttpOptions]
        public ResponseModel GetJournals([FromUri] WithdrawingJournalSearchParam searchResult)
        {
            var list = _accountbookService.SearchAccountbookJournals(GetUserContext(), searchResult);
            return Success(list);
        }

        /// <summary>
        /// 获取提现记录列表
        /// </summary>
        /// <param name="searchParam"></param>
        /// <returns></returns>
        [Route("withdrawing-journals")]
        [HttpGet, HttpOptions]
        public ResponseModel GetWithdrawingJournals([FromUri] WithdrawingJournalSearchParam searchParam)
        {
            var result = _accountbookService.SearchWithdrawingJournals(GetUserContext(), searchParam);
            return Success(result);
        }

        /// <summary>
        /// 新增店铺提现流水
        /// </summary>
        /// <param name="newWithdrawingJournalRequest"></param>
        /// <returns></returns>
        [Route("withdrawing-journals")]
        [HttpPost, HttpOptions]
        public ResponseModel AddWithdrawingJournal([FromBody] NewWithdrawingJournalRequest newWithdrawingJournalRequest)
        {
            var result = _accountbookService.AddWithdrawingJournal(GetUserContext(), newWithdrawingJournalRequest);
            return Success(result);
        }

        /// <summary>
        /// 获取收单记录列表
        /// </summary>
        /// <param name="searchParam"></param>
        /// <returns></returns>
        [Route("billing-journals")]
        [HttpGet, HttpOptions]
        public ResponseModel GetBillingJournals([FromUri] BillingJournalSearchParam searchParam)
        {
            var list = _accountbookService.SearchBillingJournals(GetUserContext(), searchParam);
            return Success(list);
        }

        /// <summary>
        /// 获取支付方式列表
        /// </summary>
        /// <returns></returns>
        [Route("billing-payments")]
        [HttpGet, HttpOptions]
        public ResponseModel GetPaymentTypeList()
        {
            var list = _accountbookService.GetPaymentTypeList(GetUserContext());
            return Success(list);
        }

        /// <summary>
        /// 获取提现账户列表
        /// </summary>
        /// <returns></returns>
        [Route("withdrawing-accounts")]
        [HttpGet, HttpOptions]
        public ResponseModel GetWithdrawingAccountList()
        {
            return Success(_accountbookService.GetWithdrawingAccountList(GetUserContext()));
        }

        /// <summary>
        /// 绑定店铺提现账号
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("withdrawing-accounts")]
        [HttpPost, HttpOptions]
        public ResponseModel AddWithdrawingAccount([FromBody]WithdrawingAccountRequest model)
        {
            return Success(_accountbookService.AddWithdrawingAccount(GetUserContext(), model));
        }

        /// <summary>
        /// 业务列表记录
        /// </summary>
        /// <returns></returns>
        [Route("billingbusiness")]
        [HttpGet, HttpOptions]
        public ResponseModel GetBillingBusiness()
        {
            return Success(_accountbookService.GetBillingBusiness(GetUserContext()));
        }

        /// <summary>
        /// 获取提现列表和收单列表状态
        /// </summary>
        /// <returns></returns>
        [Route("list-status")]
        [HttpGet, HttpOptions]
        public ResponseModel GetWithdrawingStatusAndBillingStatus()
        {
            return Success(_accountbookService.GetWithdrawingStatusAndBillingStatus());
        }

        /// <summary>
        /// 发送验证码（提现时候获取验证码）
        /// </summary>
        /// <returns></returns>
        [Route("verify-code/withdrawing")]
        [HttpPost, HttpOptions]
        public ResponseModel SendVerificationCode()
        {
            var model = _accountbookService.SendWithdrawingAuthCode(GetUserContext());
            return model.Code == (int)ErrorCodeEnum.Success ? Success(model) : Fail((ErrorCodeEnum)model.Code);
        }

        /// <summary>
        /// 发送验证码（绑定银行卡）
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [Route("verify-code/bind-withdrawingaccount")]
        [HttpPost, HttpOptions]
        public ResponseModel SendVerificationCode(VerficationCodeRequestParams parameters)
        {
            if (parameters == null)
            {
                return Fail(ErrorCodeEnum.NullArguments);
            }

            if (string.IsNullOrWhiteSpace(parameters.Phone))
            {
                return Fail(ErrorCodeEnum.PhoneCanNotBeNull);
            }

            var model = _accountbookService.SendBindWithdrawingAccountAuthCode(GetUserContext(), parameters.Phone);
            return model.Code == (int) ErrorCodeEnum.Success ? Success(model) : Fail((ErrorCodeEnum) model.Code);
        }


        /// <summary>
        /// 获取当前操作用户的权限
        /// </summary>
        /// <returns></returns>
        [Route("power")]
        [HttpGet, HttpOptions]
        public ResponseModel GetCurrentOperatorPower(int type)
        {
            return _accountbookService.GetCurrentOperatorPower(GetUserContext(), type);
        }
    }
}