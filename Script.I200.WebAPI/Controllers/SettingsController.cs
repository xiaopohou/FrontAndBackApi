using System.Web.Http;
using Script.I200.Application.Settings;
using Script.I200.Core.Config;
using Script.I200.Entity.API;

namespace Script.I200.WebAPI.Controllers
{
    /// <summary>
    ///     设置相关接口
    /// </summary>
    [RoutePrefix("v0/settings")]
    public class SettingsController : BaseApiController
    {
        private readonly IRemoteSetting _remoteSetting;
        private readonly ISettingsService _settingsService;
        // 初始化
        /// <summary>
        /// </summary>
        public SettingsController()
        {
            _settingsService = new SettingsService();
            _remoteSetting = new RedisRemoteConfig();
        }

        /// <summary>
        ///     获取店铺短信模板列表
        /// </summary>
        /// <returns></returns>
        [Route("account-sms-teampltes/{categoryId}")]
        [HttpGet, HttpOptions]
        public ResponseModel GetAccountSmsTeampltes(int categoryId)
        {
            return _settingsService.GetAccountSmsTeampltes(GetUserContext(), categoryId);
        }

        /// <summary>
        ///     获取系统短信模板列表
        /// </summary>
        /// <returns></returns>
        [Route("sys-sms-teampltes/{categoryId}")]
        [HttpGet, HttpOptions]
        public ResponseModel GetSystemSmsTeampltes(int categoryId)
        {
            return _settingsService.GetSystemSmsTeampltes(GetUserContext(), categoryId);
        }

        /// <summary>
        ///     更新配置本地缓存
        /// </summary>
        /// <param name="key"></param>
        [Route("updateLocalConfig")]
        [HttpPost, HttpOptions]
        public void UpdateLocalConfig(string key)
        {
            _remoteSetting.UpdateLocalConfig(key);
        }

        /// <summary>
        ///     获取缓存信息
        /// </summary>
        /// <param name="key"></param>
        [Route("getConfig")]
        [HttpGet, HttpOptions]
        public void GetConfig(string key)
        {
            _remoteSetting.GetConfig(key);
        }
    }
}