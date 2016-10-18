using System.Collections.Generic;
using Script.I200.Entity.Api.Modules;
using Script.I200.Entity.API;

namespace Script.I200.Application.Modules
{
    public interface IModuleService
    {
        /// <summary>
        /// 获取管理模块列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        ModuleList GetModules(UserContext userContext);

        /// <summary>
        /// 获取当前用户店铺管理模块列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        IEnumerable<AccountUserModule> GetModulesByAccount(UserContext userContext);

        /// <summary>
        /// 呈现某个功能模块
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="modulesId"></param>
        /// <returns></returns>
        int ShowAccountModule(UserContext userContext, int modulesId);

        /// <summary>
        /// 隐藏某个功能模块
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="modulesId"></param>
        /// <returns></returns>
        int HideAccountModule(UserContext userContext, int modulesId);
    }
}