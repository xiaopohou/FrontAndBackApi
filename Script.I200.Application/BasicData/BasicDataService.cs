using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Script.I200.Core;
using Script.I200.Core.Caching;
using Script.I200.Core.Config;
using Script.I200.Data;
using Script.I200.Entity;
using Script.I200.Entity.Api.BasicData;
using Script.I200.Entity.API;
using Script.I200.Entity.Enum;
using Script.I200.Entity.Model.BaseData;
using Script.I200.Entity.Model.User;

namespace Script.I200.Application.BasicData
{
    public class BasicDataService : IBasicDataService
    {
        private readonly DapperRepository<Entity.Model.Account.Account> _accountRepository;
        private readonly DapperRepository<AddressBase> _addressBaseRepository;
        private readonly DapperRepository<UserInfoDetail> _userInfoDetailRepository;
        private readonly DapperRepository<UserRank> _userRankRepository;

        public BasicDataService()
        {
            var dapperDbContext =
                new DapperDbContext(new SqlConnection(WebConfigSetting.Instance.I200DbConnectionString));
            _addressBaseRepository = new DapperRepository<AddressBase>(dapperDbContext);
            _accountRepository = new DapperRepository<Entity.Model.Account.Account>(dapperDbContext);
            _userRankRepository = new DapperRepository<UserRank>(dapperDbContext);
            _userInfoDetailRepository = new DapperRepository<UserInfoDetail>(dapperDbContext);
        }

        /// <summary>
        ///     获取城市列表（包括省列表、城市列表）
        /// </summary>
        /// <returns></returns>
        public ResponseModel GetProvinceCityList()
        {
            var catchTime = 1440*60;
            var redisCacheService = new RedisCacheService();
                return new ResponseModel
                {
                    Code = (int) ErrorCodeEnum.Success,
                    Data = redisCacheService.Get(RedisConsts.ConstructedProvinceCityList, () =>
                    {
                        {
                            {
                                // ReSharper disable once AccessToDisposedClosure
                                var basedata = redisCacheService.Get(RedisConsts.AddressBaseData, catchTime,
                                    GetAllProvinceFromDb);
                                // 获取省列表
                                var provinceList = basedata.Where(a => a.Level == 1);
                                var result = new Address.RetAddressData();

                                // 遍历省列表追加市列表
                                foreach (var province in provinceList)
                                {
                                    var provinceSelfId = province.SelfId;
                                    var provinceModel = new Address.Province {Id = province.Id, Name = province.Name};
                                    result.ProvinceList.Add(provinceModel);

                                    var cities = basedata.Where(a => a.Level == 2 && a.ParentId == provinceSelfId);
                                    result.CityList.Add(new Address.City
                                    {
                                        ProvinceId = province.Id,
                                        CityValues =
                                            cities.Select(c => new Address.CityValues {CityId = c.Id, CityName = c.Name})
                                                .ToList()
                                    });
                                }

                                return result;
                            }
                        }
                    })
                };
            
        }

        /// <summary>
        ///     根据省Id和城市id获取对应的名称
        /// </summary>
        /// <returns></returns>
        public Tuple<string, string> GetProvinceAndCityNameById(int provinceId, int cityId)
        {
            var catchTime = 1440*60;
            if (provinceId == 1)
            {
                return new Tuple<string, string>("北京", "北京");
            }
            if (provinceId == 153)
            {
                return new Tuple<string, string>("上海", "上海");
            }
            if (provinceId == 342)
            {
                return new Tuple<string, string>("重庆", "重庆");
            }
            if (provinceId == 294)
            {
                return new Tuple<string, string>("天津", "天津");
            }

            var redisCacheService = new RedisCacheService();
            var allProvince = redisCacheService.Get(RedisConsts.AddressBaseData, catchTime, GetAllProvinceFromDb);
            var province = allProvince.SingleOrDefault(a => a.Id == provinceId && a.Level == 1);
            if (province == null)
                throw new YuanbeiException("未找到对应的省:" + provinceId);

            var city = allProvince.SingleOrDefault(a => a.Id == cityId && a.Level == 2);
            if (city == null)
                throw new YuanbeiException("未找到对应的市:" + cityId);

            return new Tuple<string, string>(province.Name, city.Name);

        }

        /// <summary>
        ///     积分增加
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="userInfo"></param>
        /// <param name="uIntegral"></param>
        /// <returns></returns>
        public bool IntegrationAdd(UserContext userContext, UserInfoDetail userInfo, int uIntegral)
        {
            var userExistInfo = _userInfoDetailRepository.Find(x => x.Uid == userInfo.Uid);
            if (userExistInfo == null)
            {
                return false;
            }
            var userInfoDetail = new UserInfoDetail
            {
                Uid = userInfo.Uid,
                UIntegral = userExistInfo.UIntegral + uIntegral
            };
            return _userInfoDetailRepository.Update<UserInfoDetail>(userInfoDetail, item => new {item.UIntegral});
        }

        /// <summary>
        ///     是否是本店会员
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public bool IsExistInCurrentStore(UserContext userContext, UserInfoDetail userInfo)
        {
            return _userInfoDetailRepository.Find(x => x.AccId == userContext.AccId && x.Uid == userInfo.Uid) != null;
        }

        /// <summary>
        /// 获取店铺会员等级配置
        /// </summary>
        /// <param name="accId"></param>
        /// <returns></returns>
        public ResponseModel GetStoreRankConfig(int accId)
        {
            var result = _userRankRepository.FindAll(x => x.AccId == accId);
            return new ResponseModel
            {
                Code = 0,
                Message = "获取数据成功",
                Data = result
            };
        }

        /// <summary>
        ///     获取店铺积分配置
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public string GetStoreIntegrationConfig(UserContext userContext)
        {
            var strResult = "1/1"; //默认
            var result = _accountRepository.FindAll(x => x.Id == userContext.AccId).Select(x => x.Proportion).FirstOrDefault();
            return result ?? strResult;
        }

        /// <summary>
        ///     从Db中获取省市数据
        /// </summary>
        /// <returns></returns>
        private List<AddressBase> GetAllProvinceFromDb()
        {
            return _addressBaseRepository.FindAll().ToList();
        }
    }
}