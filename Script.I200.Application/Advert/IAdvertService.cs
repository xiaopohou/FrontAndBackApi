using Script.I200.Entity.API;

namespace Script.I200.Application.Advert
{
    public interface IAdvertService
    {
        /// <summary>
        /// 获取广告详情
        /// </summary>
        /// <returns></returns>
        ResponseModel GetAdvertDetail(string positionCode, int accountId = -1, UserContext userContext=null);
    }
}
