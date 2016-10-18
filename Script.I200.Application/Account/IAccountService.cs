using System.Collections.Generic;
using Script.I200.Entity.Api.Account;
using Script.I200.Entity.API;
using Script.I200.Entity.Model.User;

namespace Script.I200.Application.Account
{
    /// <summary>
    ///     店铺账户相关接口（B端用户）
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        ///     根据店铺人员的Id获取详细信息
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        AccountUser GetAccountUserInfoById(UserContext userContext, int id);

        /// <summary>
        /// 获取店铺基本信息
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        AccountBaseInfo GetAccountBaseInfo(UserContext userContext);

        /// <summary>
        /// 获取店铺今日运营概况
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        AccountOverview GetAccountOverViewToday(UserContext userContext);

        /// <summary>
        /// 获取店铺运营概况明细
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        List<AccountOverviewDetail> GetAccountOverViewDetail(UserContext userContext, int type);
    }
}