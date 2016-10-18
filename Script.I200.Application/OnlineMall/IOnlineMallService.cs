using System.Collections.Generic;
using Script.I200.Entity.Api.OnlineMall;
using Script.I200.Entity.API;
using Script.I200.Entity.Enum;

namespace Script.I200.Application.OnlineMall
{
    public interface IOnlineMallService
    {
        /// <summary>
        /// 获取硬件商品简要信息列表
        /// </summary>
        /// <param name="gidInts"></param>
        /// <returns></returns>
        ResponseModel GetIndexMaterialGoodsList(IEnumerable<int> gidInts);

        /// <summary>
        /// 获取硬件商品详情
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        ResponseModel GetMaterialGoodsInfo(int goodsId);

        /// <summary>
        /// 获取硬件商品评论
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        ResponseModel GetMaterialEvaluation(int goodsId);

        /// <summary>
        /// 获取硬件商品ID
        /// </summary>
        /// <param name="typEnum"></param>
        /// <returns></returns>
        IEnumerable<int> GetMaterialGoodsId(GetMaterialIdEnum typEnum);

        /// <summary>
        /// 获取最后一次收货地址
        /// </summary>
        /// <returns></returns>
        ResponseModel GetLastAddressModel(int accId);
        /// <summary>
        /// 添加地址信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        ResponseModel AddAddressModel(ReceiveingAddressAdd entity);
    }
}
