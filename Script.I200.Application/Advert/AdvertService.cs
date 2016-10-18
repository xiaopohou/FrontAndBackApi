using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Script.I200.Application.Logging;
using Script.I200.Core.Caching;
using Script.I200.Core.Config;
using Script.I200.Core.Data;
using Script.I200.Data;
using Script.I200.Entity;
using Script.I200.Entity.API;
using Script.I200.Entity.Model.Advert;

namespace Script.I200.Application.Advert
{
    public class AdvertService : IAdvertService
    {
        private readonly IRepository<Entity.Model.Advert.Advert> _advertDataRepository;
        private readonly IRepository<AdvertResources> _advertResourcesDataRepository;
        private readonly IRepository<AdvertLog> _advertLogDataRepository;
        private ILogger _logger;

        public AdvertService()
        {
            _logger = new NLogger();

            var dapperDbContext =
                new DapperDbContext(new SqlConnection(WebConfigSetting.Instance.I200StationDbConnectionString));

            _advertDataRepository =
                new DapperRepository<Entity.Model.Advert.Advert>(dapperDbContext);
            _advertResourcesDataRepository =
                new DapperRepository<AdvertResources>(dapperDbContext);
            _advertLogDataRepository = new DapperRepository<AdvertLog>(dapperDbContext);
        }

        /// <summary>
        /// 获取广告详情
        /// </summary>
        /// <returns></returns>
        public ResponseModel GetAdvertDetail(string positionCode, int accountId = -1, UserContext userContext=null)
        {
            var adverApi = new Entity.Api.Advert.Advert();

            string key = RedisConsts.StationAdvertKey + positionCode.ToLower();

            var redisCacheService = new RedisCacheService();
            if (redisCacheService.HasKey(key))
            {
                adverApi = redisCacheService.Get<Entity.Api.Advert.Advert>(key);
            }
            else
            {
                var advertModel =
                    _advertDataRepository.FindAll(ad => ad.AdPositionCode == positionCode
                                                        && ad.Enable == 1
                                                        && ad.StartDate <= DateTime.Now
                                                        && ad.EndDate >= DateTime.Now
                        ).OrderByDescending(o => o.Id).FirstOrDefault();

                if (advertModel == null)
                {
                    //throw new YuanbeiException("获取站内广告信息失败，未找到对应信息");
                    return new ResponseModel
                    {
                        Code = 0,
                        Message = "无数据",
                        Data = null
                    };
                }

                var adverResourcesList = _advertResourcesDataRepository.FindAll(ad => ad.AdId == advertModel.Id
                                                                                      && ad.State == 1)
                    .OrderByDescending(o => o.Id);

                //TO DTO
                adverApi.title = advertModel.AdTitle;
                adverApi.display = advertModel.DisplayMode;
                adverApi.width = advertModel.AdWidth;
                adverApi.height = advertModel.AdHieght;
                adverApi.items = new List<Entity.Api.Advert.AdvertResources>();
                foreach (var advertResourcese in adverResourcesList)
                {
                    var advertResourceApi = new Entity.Api.Advert.AdvertResources();
                    //advertResourceApi.link = advertResourcese.AdLink;
                    advertResourceApi.link = WebConfigSetting.Instance.AdvertTransferUrl +
                                             string.Format("?adresid={0}&turl={1}", advertResourcese.Id,
                                                 HttpUtility.UrlEncode(advertResourcese.AdLink));
                    advertResourceApi.image = WebConfigSetting.Instance.ImageServer + advertResourcese.AdImageUrl;

                    adverApi.items.Add(advertResourceApi);
                }

                //Insert Cache 7days
                redisCacheService.Set(key, adverApi, 60*60*24*7);
            }

            //Insert View Log
                Task.Factory.StartNew(() =>
                {
                    var advertModel =
                         _advertDataRepository.FindAll(ad => ad.AdPositionCode == positionCode
                                                             && ad.Enable == 1
                                                             && ad.StartDate <= DateTime.Now
                                                             && ad.EndDate >= DateTime.Now
                             ).OrderByDescending(o => o.Id).FirstOrDefault();

                    AdvertLog advertLog = new AdvertLog();
                    advertLog.AdId = advertModel.Id;
                    advertLog.AccountId = accountId;
                    if (userContext!=null)
                        advertLog.Ip = userContext.IpAddress;
                    advertLog.Type = 1;
                    advertLog.CreateTime = DateTime.Now;
                    _advertLogDataRepository.Insert(advertLog);
                });

            //TODO 店铺权限判断
            if (accountId != -1)
            {

            }

            //2.返回查询结果
            return new ResponseModel
            {
                Code = 0,
                Message = "获取数据成功",
                Data = adverApi
            };
        }
    }
}
