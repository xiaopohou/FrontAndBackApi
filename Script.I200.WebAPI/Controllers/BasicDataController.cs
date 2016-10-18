using System.Web.Http;
using Script.I200.Application.BasicData;
using Script.I200.Entity.API;
using Script.I200.Entity.Enum;
using Script.I200.Entity.Model.User;

namespace Script.I200.WebAPI.Controllers
{
    /// <summary>
    ///     基础数据Api接口
    /// </summary>
    [RoutePrefix("v0/basic-data")]
    public class BasicDataController : BaseApiController
    {
        private readonly IBasicDataService _basicDataService = new BasicDataService();

        /// <summary>
        ///     省市数据
        /// </summary>
        /// <returns></returns>
        [Route("province")]
        [HttpGet, HttpOptions]
        public ResponseModel GetProvinceCityList()
        {
            return _basicDataService.GetProvinceCityList();
        }

        /// <summary>
        ///     新增积分
        /// </summary>
        /// <returns></returns>
        [Route("integrations")]
        [HttpPost, HttpOptions]
        public ResponseModel IntegrationAdd(UserInfoDetail userInfo, int uIntegral)
        {
            var result = _basicDataService.IntegrationAdd(GetUserContext(), userInfo, uIntegral);
            return new ResponseModel
            {
                Code = result ? (int) ErrorCodeEnum.Success : (int) ErrorCodeEnum.Failed,
                Data = result
            };
        }

        /// <summary>
        ///     获取店铺积分配置
        /// </summary>
        /// <returns></returns>
        [Route("integrations-config")]
        [HttpGet, HttpOptions]
        public ResponseModel GetStoreIntegrationConfig()
        {
            var result = _basicDataService.GetStoreIntegrationConfig(GetUserContext());
            return new ResponseModel
            {
                Code = (int)ErrorCodeEnum.Success,
                Data = result
            };
        }

        /// <summary>
        ///     获取店铺会员等级配置
        /// </summary>
        /// <returns></returns>
        [Route("rank-config")]
        [HttpGet, HttpOptions]
        public ResponseModel GetStoreRankConfig(UserInfoDetail userInfo)
        {
            return _basicDataService.GetStoreRankConfig(GetUserContext().AccId);
        }
    }
}