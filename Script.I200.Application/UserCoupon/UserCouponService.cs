using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CommonLib;
using CommonLib.UserSearch;
using Dapper;
using Script.I200.Core.Config;
using Script.I200.Data;
using Script.I200.Data.MicroOrm.SqlGenerator;
using Script.I200.Entity.Api.Coupon;
using Script.I200.Entity.Enum;
using Script.I200.Entity.Model.Coupon;

namespace Script.I200.Application.UserCoupon
{
    public class UserCouponService:IUserCouponService
    {
        private readonly DapperRepository<CouponInfo> _couponInfoDapperRepository;
        private readonly DapperRepository<CouponList> _couponListDapperRepository;
        public UserCouponService()
        {
            var dapperContext =
               new DapperDbContext(new SqlConnection(WebConfigSetting.Instance.I200DbConnectionString));
            _couponInfoDapperRepository=new DapperRepository<CouponInfo>(dapperContext);
            _couponListDapperRepository = new DapperRepository<CouponList>(dapperContext);
        }


        public CouponInfoItem GetCouponInfo(int accId, int groupId)
        {
            var result=_couponInfoDapperRepository.Find(x => x.Id == groupId);
            var count = _couponListDapperRepository.FindAll(x => x.AccId == accId && x.GroupId == groupId).Count();
            var couponInfoItem=Mapper.Map<CouponInfo, CouponInfoItem>(result);
            couponInfoItem.CouponType=Enum.GetName(typeof(CouponEnum.CouponType), couponInfoItem.CouponType);
            return couponInfoItem;
        }
    }
}
