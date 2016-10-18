using System;
using Script.I200.Entity.API;
using Script.I200.Entity.Model.User;

namespace Script.I200.Application.BasicData
{
    /// <summary>
    ///     获取城市列表（包括省列表、城市列表）
    /// </summary>
    public interface IBasicDataService
    {
        /// <summary>
        ///     获取省市数据
        /// </summary>
        /// <returns></returns>
        ResponseModel GetProvinceCityList();

        /// <summary>
        ///     根据城市Id获取城市名称
        /// </summary>
        /// <param name="provinceId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        Tuple<string, string> GetProvinceAndCityNameById(int provinceId, int cityId);

        /// <summary>
        ///     积分增加
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="userInfo"></param>
        /// <param name="uIntegral"></param>
        /// <returns></returns>
        bool IntegrationAdd(UserContext userContext, UserInfoDetail userInfo, int uIntegral);

        /// <summary>
        ///     获取店铺积分配置
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        string GetStoreIntegrationConfig(UserContext userContext);

        /// <summary>
        ///     是否是本店会员
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        bool IsExistInCurrentStore(UserContext userContext, UserInfoDetail userInfo);

        /// <summary>
        ///     获取店铺会员等级配置
        /// </summary>
        /// <param name="accId"></param>
        /// <returns></returns>
        ResponseModel GetStoreRankConfig(int accId);


    }
}