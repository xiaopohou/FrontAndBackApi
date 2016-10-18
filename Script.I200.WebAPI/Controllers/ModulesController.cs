using System.Web.Http;
using Script.I200.Application.Modules;
using Script.I200.Entity.API;
using Script.I200.Entity.Enum;

namespace Script.I200.WebAPI.Controllers
{
    /// <summary>
    /// 管理模块相关功能
    /// </summary>
    [RoutePrefix("v0/modules")]
    public class ModulesController : BaseApiController
    {
        private readonly IModuleService _moduleService;

        /// <summary>
        ///     初始化
        /// </summary>
        public ModulesController()
        {
            _moduleService = new ModuleService();
        }

        /// <summary>
        /// 获取管理模块列表
        /// </summary>
        /// <returns></returns>
         [Route("list")]
        public ResponseModel GetModules()
        {
            var moduleData = _moduleService.GetModules(GetUserContext());
            return Success(moduleData);
        }

        /// <summary>
        /// 获取当前用户店铺管理模块列表
        /// </summary>
        /// <returns></returns>
        [Route("account")]
        public ResponseModel GetModulesByAccount()
        {
            var moduleData = _moduleService.GetModulesByAccount(GetUserContext());
            return Success(moduleData);
        }

        /// <summary>
        /// 呈现某个功能模块
        /// </summary>
        /// <param name="modulesId"></param>
        /// <returns></returns>
        [Route("account/{modulesId}/show")]
        [HttpPut, HttpOptions]
        public ResponseModel ShowAccountModule(int modulesId)
        {
            var moduleData = _moduleService.ShowAccountModule(GetUserContext(), modulesId);

            if (moduleData == -1)
                return Fail(ErrorCodeEnum.ModuleLessThanEightTimes);

            if (moduleData == -2)
                return Fail(ErrorCodeEnum.ModuleNeedPaid);

            if (moduleData == 0)
                return Fail(ErrorCodeEnum.ModuleFail);

            return Success(moduleData);
        }

        /// <summary>
        /// 呈现某个功能模块
        /// </summary>
        /// <param name="modulesId"></param>
        /// <returns></returns>
        [Route("account/{modulesId}/hide")]
        [HttpPut, HttpOptions]
        public ResponseModel HideAccountModule(int modulesId)
        {
            var moduleData = _moduleService.HideAccountModule(GetUserContext(), modulesId);

            if (moduleData == -1)
                return Fail(ErrorCodeEnum.ModuleMoreThanFourTimes);

            if (moduleData == 0)
                return Fail(ErrorCodeEnum.ModuleFail);

            return Success(moduleData);
        }
    }
}