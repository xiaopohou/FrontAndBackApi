using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Script.I200.Application.OnlineMall;
using Script.I200.Entity.API;
using Script.I200.Entity.Dto.OrderCoupon;
using Script.I200.Entity.Enum;
using Script.I200.Entity.Model.Order;

namespace Script.I200.WebAPI.Controllers
{
    /// <summary>
    /// 优惠券相关接口
    /// </summary>
    [RoutePrefix("v0")]
    public class OrderCouponController : BaseApiController
    {
        private readonly IOrderCouponService _orderCouponService;

        public OrderCouponController()
        {
            _orderCouponService = new OrderCouponService();
        }

        /// <summary>
        /// 获取移动端商城硬件首页优惠券信息
        /// </summary>
        /// <returns></returns>
        [Route("ordercoupon/getmcouponinfo")]
        [HttpGet, HttpOptions]
        public ResponseModel GetMobileIndexMaterialGoodsList()
        {
            var userContext = GetUserContext();
            var gInts = new List<int> { 3382, 3381, 3380, 3379, 3378 };
            Dictionary<int, bool> dic = gInts.ToDictionary(item => item, item => GetMaterialCouponInfo(item, userContext.AccId).IsCanReceive);
            return new ResponseModel
            {
                Code = (int)ErrorCodeEnum.Success,
                Data = dic
            };
        }


        [Route("ordercoupon/materialreceive/{id}")]
        [HttpPost, HttpOptions]
        public ResponseModel GetMobileIndexMaterialGoodsList(int id)
        {
            var userContext = GetUserContext();
            MaterialCouponInfo coupinInfo = GetMaterialCouponInfo(id, userContext.AccId);
            if (coupinInfo.IsCanReceive)
            {
                T_Order_CouponList couponModel = new T_Order_CouponList
                {
                    groupId = coupinInfo.GroupId,
                    couponId = "",
                    couponValue = coupinInfo.CouponMoney,
                    couponStatus = (int)OrderCouponEnum.CouponStatus.Bound,
                    createDate = DateTime.Now,
                    endDate = coupinInfo.EndTime,
                    receiveDate = DateTime.Now,
                    toAccId = userContext.AccId,
                    remark = "硬件商城用户自助领取",
                    bindWay = (int)OrderCouponEnum.CouponBindWay.UserBind,
                    flag = "硬件商城用户自助领取"
                };
                string preName = "LJ";
                var model = _orderCouponService.CreateCoupon(couponModel, preName);
                if (model.Code == (int)ErrorCodeEnum.Success)
                {
                    _orderCouponService.HadReceivedDailyMaterialCoupon(userContext.AccId, coupinInfo.GroupId,
                        coupinInfo.EndTimeSeconds);
                }
                return model;
            }
            else
            {
                return Fail(ErrorCodeEnum.RecievedCoupon);
            }
            //return _orderCouponService.GetMobileIndexMaterialGoodsList();
        }
        /// <summary>
        /// 优惠券组ID 优惠金额 过期时间 优惠券结束时间 是否可领取
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="accId"></param>
        /// <returns></returns>
        private MaterialCouponInfo GetMaterialCouponInfo(int groupId, int accId)
        {
            DateTime dt = DateTime.Now;
            DateTime enDateTime = Convert.ToDateTime(dt.ToShortDateString()).AddDays(1);
            DateTime enYearDateTime = new DateTime(dt.Year + 1, 1, 1).AddSeconds(-1);
            TimeSpan ts = enDateTime - dt;
            MaterialCouponInfo couponInfo = new MaterialCouponInfo();
            switch (groupId)
            {
                case 3382:
                    couponInfo = new MaterialCouponInfo
                    {
                        GroupId = groupId,
                        CouponMoney = 18,
                        EndTimeSeconds = (int)ts.TotalSeconds,
                        EndTime = dt.AddDays(1),
                        IsCanReceive = _orderCouponService.IsHasDailyMaterialCoupon(accId, groupId)
                    };
                    break;
                case 3381:
                    couponInfo = new MaterialCouponInfo
                    {
                        GroupId = groupId,
                        CouponMoney = 10,
                        EndTimeSeconds = (int)ts.TotalSeconds,
                        EndTime = dt.AddDays(1),
                        IsCanReceive = _orderCouponService.IsHasDailyMaterialCoupon(accId, groupId)
                    };
                    break;
                case 3380:
                    couponInfo = new MaterialCouponInfo
                    {
                        GroupId = groupId,
                        CouponMoney = 5,
                        EndTimeSeconds = (int)ts.TotalSeconds,
                        EndTime = dt.AddDays(1),
                        IsCanReceive = _orderCouponService.IsHasDailyMaterialCoupon(accId, groupId)
                    };
                    break;
                case 3379:
                    couponInfo = new MaterialCouponInfo
                    {
                        GroupId = groupId,
                        CouponMoney = 3,
                        EndTimeSeconds = (int)ts.TotalSeconds,
                        EndTime = dt.AddDays(1),
                        IsCanReceive = _orderCouponService.IsHasDailyMaterialCoupon(accId, groupId)
                    };
                    break;
                case 3378:
                    couponInfo = new MaterialCouponInfo
                    {
                        GroupId = groupId,
                        CouponMoney = 20,
                        EndTimeSeconds = 0,
                        EndTime = enYearDateTime,
                        IsCanReceive = _orderCouponService.IsHasNewMaterialCoupon(accId, groupId)
                    };
                    break;
            }
            return couponInfo;
        }
    }
}
