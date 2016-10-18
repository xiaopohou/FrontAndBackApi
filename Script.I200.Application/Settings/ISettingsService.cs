using Script.I200.Entity.API;

namespace Script.I200.Application.Settings
{
    /// <summary>
    ///     设置相关接口
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        ///     获取店铺短信模板列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        ResponseModel GetAccountSmsTeampltes(UserContext userContext, int categoryId);

        /// <summary>
        ///     获取系统短信模板列表
        /// </summary>
        /// <returns></returns>
        ResponseModel GetSystemSmsTeampltes(UserContext userContext, int categoryId);
    }
}