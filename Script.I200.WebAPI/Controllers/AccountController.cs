using System.Web.Http;
using Script.I200.Application.Account;
using Script.I200.Entity.API;

namespace Script.I200.WebAPI.Controllers
{
    /// <summary>
    ///     店铺账户相关接口（B端用户）
    /// </summary>
    [RoutePrefix("v0/account")]
    public class AccountController : BaseApiController
    {
        private readonly IAccountService _accountService;

        /// <summary>
        ///     初始化
        /// </summary>
        public AccountController()
        {
            _accountService = new AccountService();
        }

        /// <summary>
        ///     根据店铺人员的Id获取详细信息
        /// </summary>
        /// <returns></returns>
        [Route("account-users/{id}")]
        [HttpGet, HttpOptions]
        public ResponseModel GetAccountUserInfoById(int id)
        {
            var userInfo = _accountService.GetAccountUserInfoById(GetUserContext(),id);
            return Success(userInfo);
        }

        /// <summary>
        /// 获取店铺基本信息
        /// </summary>
        /// <returns></returns>
        [Route("baseinfo")]
        [HttpGet,HttpOptions]
        public ResponseModel GetAccountBaseInfo()
        {
            var accountInfo = _accountService.GetAccountBaseInfo(GetUserContext());
            return Success(accountInfo);
        }

        /// <summary>
        /// 获取店铺今日运营概况
        /// </summary>
        /// <returns></returns>
        [Route("overview/today")]
        [HttpGet, HttpOptions]
        public ResponseModel GetAccountOverView()
        {
            var accountOverview = _accountService.GetAccountOverViewToday(GetUserContext());
            return Success(accountOverview);
        }

        /// <summary>
        /// 获取店铺运营概况明细
        /// </summary>
        /// <returns></returns>
        [Route("overview/{type}/detail")]
        [HttpGet, HttpOptions]
        public ResponseModel GetAccountOverViewDetail(int type)
        {
            var accountOverviewDetail = _accountService.GetAccountOverViewDetail(GetUserContext(), type);
            return Success(accountOverviewDetail);
        }
    }
}