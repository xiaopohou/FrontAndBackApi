using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Script.I200.Entity.Api.Coupon;

namespace Script.I200.Application.UserCoupon
{
    public interface IUserCouponService
    {
        CouponInfoItem GetCouponInfo(int accId, int groupId);
    }
}
