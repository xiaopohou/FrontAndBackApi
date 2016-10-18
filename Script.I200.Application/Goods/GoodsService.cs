using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Script.I200.Core.Config;
using Script.I200.Data;
using Script.I200.Entity.API;
using Script.I200.Entity.Enum;
using Script.I200.Entity.Model.Goods;

namespace Script.I200.Application.Goods
{
    public class GoodsService : IGoodsService
    {
        private readonly DapperRepository<GoodsInfoSummary> _goodsSummaryRepository;

        public GoodsService()
        {
            var dapperDbContext =
                new DapperDbContext(new SqlConnection(WebConfigSetting.Instance.I200DbConnectionString));
            _goodsSummaryRepository = new DapperRepository<GoodsInfoSummary>(dapperDbContext);
        }

        public ResponseModel GetGoodsInfoByGid(int gid, UserContext userContext)
        {
            var result =
                _goodsSummaryRepository.Find(x => x.Id == gid && x.AccId == userContext.AccId, null,
                    item => new {item.Id, item.GName, item.IsService, item.AccId});
            if (result == null)
            {
                return new ResponseModel
                {
                    Code = (int) ErrorCodeEnum.NotFound,
                    Message = "未找到对应的资源",
                    Data = null
                };
            }
            return new ResponseModel
            {
                Code = (int) ErrorCodeEnum.Success,
                Message = "获取数据成功",
                Data = result
            };
        }

        /// <summary>
        /// 获取服务类项目
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public List<GoodsInfoSummary> GetServiceGoodsInfo(UserContext userContext)
        {
            return _goodsSummaryRepository.FindAll(x => x.AccId == userContext.AccId && x.IsService == 1).ToList();
        }
    }
}