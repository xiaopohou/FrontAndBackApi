using System.Web.Http;
using Script.I200.Application.Tips;
using Script.I200.Entity.API;

namespace Script.I200.WebAPI.Controllers
{
    /// <summary>
    ///     提示相关接口
    /// </summary>
    [RoutePrefix("v0")]
    public class TipsController : BaseApiController
    {
        private readonly ITipsService _tipsService;

        /// <summary>
        ///     初始化
        /// </summary>
        public TipsController()
        {
            _tipsService = new TipsService();
        }

        /// <summary>
        ///     是否隐藏提示消息
        /// </summary>
        /// <param name="tipsType"></param>
        /// <returns></returns>
        [Route("tips-Status/{tipsType}")]
        [HttpGet, HttpOptions]
        public ResponseModel IsHideElement(int tipsType)
        {
            return FunctionReturn(_tipsService.IsHideElement(GetUserContext(), tipsType));
        }

        /// <summary>
        ///   更新消息提示层
        /// </summary>
        /// <returns></returns>
        [Route("tips/{tipsType}")]
        [HttpPut, HttpOptions]
        public ResponseModel AddOrUpdateUserBehavior(int tipsType)
        {
            return FunctionReturn(_tipsService.AddOrUpdateUserBehavior(GetUserContext(), tipsType));
        }
    }
}