using System.Collections.Generic;
using Script.I200.Entity.API;
using Script.I200.Entity.Model.Goods;

namespace Script.I200.Application.Goods
{
    public interface IGoodsService
    {
        ResponseModel GetGoodsInfoByGid(int gid, UserContext userContext);

        /// <summary>
        /// 获取服务类项目
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        List<GoodsInfoSummary> GetServiceGoodsInfo(UserContext userContext);
    }
}
