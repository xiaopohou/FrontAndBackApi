
using System.Web.Http;
using Script.I200.Application.Advert;
using Script.I200.Entity.API;
using Script.I200.Entity.Enum;

namespace Script.I200.WebAPI.Controllers
{
    /// <summary>
    /// 站内广告数据Api接口
    /// </summary>
    [RoutePrefix("v0/wumao")]
    public class AdvertController :BaseApiController
    {
        private readonly IAdvertService _advertService = new AdvertService();

        [AllowAnonymous]
        [Route("{positionCode}")]
        [HttpGet, HttpOptions]
        public ResponseModel GetAdvertDetail(string positionCode)
        {
            //UserContext userContext = GetUserContext();
            var advertApi = _advertService.GetAdvertDetail(positionCode,-1, null);

            if (advertApi == null)
            {
                Fail(ErrorCodeEnum.NotFound);
            }

            return Success(advertApi);
        }

        [AllowAnonymous]
        [Route("{accountId}/{positionCode}")]
        [HttpGet, HttpOptions]
        public ResponseModel GetAdvertDetail(int accountId, string positionCode)
        {
            //UserContext userContext = GetUserContext();
            var advertApi = _advertService.GetAdvertDetail(positionCode, accountId, null);

            if (advertApi == null)
            {
                Fail(ErrorCodeEnum.NotFound);
            }

            return Success(advertApi);
        }
    }
}
