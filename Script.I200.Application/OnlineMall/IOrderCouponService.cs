using Script.I200.Entity.API;
using Script.I200.Entity.Model.Order;

namespace Script.I200.Application.OnlineMall
{
    public interface IOrderCouponService
    {
        bool IsHasNewMaterialCoupon(int accId, int groupId);
        bool IsHasDailyMaterialCoupon(int accId, int groupId);
        bool HadReceivedDailyMaterialCoupon(int accId, int groupId, int totalSeconds);
        ResponseModel CreateCoupon(T_Order_CouponList couponModel,string preName);
    }
}
