using Script.I200.Entity.Api.Account;
using Script.I200.Entity.API;
using Script.I200.Entity.Model.User;

namespace Script.I200.Application.Users
{
    public interface IUserQueryService
    {
        UserPhoneEmail GetShopAdministratorPhoneEmail(int accId, bool isEncrypt = false);

        /// <summary>
        ///  获取用户的相关信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="appKey"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserContext GetUserContext(string token, string appKey,string userId);
        /// <summary>
        /// 获取店铺版本信息
        /// </summary>
        /// <param name="accId"></param>
        /// <returns></returns>
        AccountVersion GetAccountVersion(int accId);
        /// <summary>
        /// 是否有权限 2的N次方权限
        /// </summary>
        /// <param name="sysPower"></param>
        /// <param name="userPower"></param>
        /// <param name="userRole"></param>
        /// <returns></returns>
        bool IsPower(int sysPower, int userPower, int userRole);

    }
}
