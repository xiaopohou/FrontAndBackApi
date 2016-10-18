using System;
using System.Web.Http;
using Script.I200.Application.OnlineMall;
using Script.I200.Entity.Api.OnlineMall;
using Script.I200.Entity.API;
using Script.I200.Entity.Enum;

namespace Script.I200.WebAPI.Controllers
{
    /// <summary>
    /// 商城相关接口
    /// </summary>
    [RoutePrefix("v0/onlinemall")]
    public class OnlineMallController : BaseApiController
    {
        private readonly IOnlineMallService _onlineMallService;

        public OnlineMallController()
        {
            _onlineMallService = new OnlineMallService();
        }

        /// <summary>
        /// 获取移动端商城硬件首页显示内容
        /// </summary>
        /// <returns></returns>
        [Route("mobile/material/index")]
        [HttpGet, HttpOptions]
        public ResponseModel GetMobileIndexMaterialGoodsList()
        {
            var gInts = _onlineMallService.GetMaterialGoodsId(GetMaterialIdEnum.MobileShow);
            if (gInts==null)
            {
               return Fail(ErrorCodeEnum.NoMaterialGoods);
            }
            return _onlineMallService.GetIndexMaterialGoodsList(gInts);
        }
        /// <summary>
        /// 获取移动端商城显示内容
        /// </summary>
        /// <returns></returns>
        [Route("mobile/material/info/{id}")]
        [HttpGet, HttpOptions]
        public ResponseModel GetMobileMaterialGoodsInfo(int id)
        {
            return _onlineMallService.GetMaterialGoodsInfo(id);
        }
        /// <summary>
        /// 获取移动端商城硬件评论
        /// </summary>
        /// <returns></returns>
        [Route("mobile/evaluation/{id}")]
        [HttpGet, HttpOptions]
        public ResponseModel GetMobileEvaluationList(int id)
        {
            return _onlineMallService.GetMaterialEvaluation(id);
        }

        /// <summary>
        /// 获取最近一次地址
        /// </summary>
        /// <returns></returns>
        [Route("mobile/address")]
        [HttpGet, HttpOptions]
        public ResponseModel GetMobileAddressModel()
        {
            return _onlineMallService.GetLastAddressModel(GetUserContext().AccId);
        }
        /// <summary>
        /// 获取最近一次地址
        /// </summary>
        /// <returns></returns>
        [Route("mobile/address")]
        [HttpPost, HttpOptions]
        public ResponseModel AddMobileAddressModel(ReceiveingAddressAdd entity)
        {
            entity.CreateTime = DateTime.Now;
            entity.AccId = GetUserContext().AccId;
            return _onlineMallService.AddAddressModel(entity);
        }

    }
}
