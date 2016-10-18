using AutoMapper;
using CommonLib.UserSearch;
using Script.I200.Entity.Api.Coupon;
using Script.I200.Entity.Api.Users;
using Script.I200.Entity.Dto.OnlineMall;
using Script.I200.Entity.Model.Coupon;
using Script.I200.Entity.Model.Onlinemall;
using Script.I200.Entity.Model.User;

namespace Script.I200.WebAPI
{
    public class MapperInit
    {
        public static void InitMapping()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<T_MaterialGoods2, MaterialGoodsInfo>();
                cfg.CreateMap<T_MaterialGoods2, MaterialGoodsListRedisItem>()
                    .ForMember(x => x.GoodsId, y => y.MapFrom(z => z.GoodsId))
                    .ForMember(x => x.Status, y => y.MapFrom(z => z.Status))
                    .ForAllOtherMembers(x => x.Ignore());
                cfg.CreateMap<UserHandle, UserBasic>()
                    .ForMember(x => x.user_id, y => y.MapFrom(z => z.Id))
                    .ForMember(x => x.user_name, y => y.MapFrom(z => z.UserName))
                    .ForMember(x => x.user_cardno, y => y.MapFrom(z => z.UserNo))
                    .ForMember(x => x.user_phone, y => y.MapFrom(z => z.UserPhone))
                    .ForMember(x => x.user_initials, y => y.MapFrom(z => z.PY))
                    .ForMember(x => x.user_pinyin, y => y.MapFrom(z => z.PinYin))
                    .ForMember(x => x.account_id, y => y.MapFrom(z => z.AccId))
                    .ForMember(x => x.master_id, y => y.MapFrom(z => z.MasterId))
                    .ForAllOtherMembers(x => x.Ignore());
                cfg.CreateMap<UserInfoDetail, UserListDetails>()
                    .ForMember(x => x.AccId, y => y.MapFrom(z => z.AccId))
                    .ForMember(x => x.UserId, y => y.MapFrom(z => z.Uid))
                    .ForMember(x => x.UserNo, y => y.MapFrom(z => z.UNumber))
                    .ForMember(x => x.UserName, y => y.MapFrom(z => z.UName))
                    .ForMember(x => x.UserPhone, y => y.MapFrom(z => z.UPhone))
                    .ForMember(x => x.UserGradeId, y => y.MapFrom(z => z.URank))
                    .ForMember(x => x.UserGroupId, y => y.MapFrom(z => z.UGroup))
                    .ForMember(x => x.UserIntegral, y => y.MapFrom(z => z.UIntegral))
                    .ForMember(x => x.UserStoreMoney, y => y.MapFrom(z => z.UStoreMoney))
                    .ForMember(x => x.UserLastButDate, y => y.MapFrom(z => z.ULastBuyDate))
                    .ForMember(x => x.UserGradeName, y => y.MapFrom(z => z.UserGradeName))
                    .ForMember(x => x.UserGroupName, y => y.MapFrom(z => z.UserGroupName))
                    .ForMember(x => x.UserAvatar, y => y.MapFrom(z => z.UPortrait))
                    .ForAllOtherMembers(x => x.Ignore());
                cfg.CreateMap<CouponInfo, CouponInfoItem>()
                    .ForMember(x => x.Id, y => y.MapFrom(z => z.Id))
                    .ForMember(x => x.AccId, y => y.MapFrom(z => z.AccId))
                    .ForMember(x => x.CouponClass, y => y.MapFrom(z => z.CouponClass))
                    .ForMember(x => x.CouponType, y => y.MapFrom(z => z.CouponType))
                    .ForMember(x => x.CouponRuleId, y => y.MapFrom(z => z.CouponRule))
                    .ForMember(x => x.CouponRuleRemark, y => y.MapFrom(z => z.CouponRuleRemark))
                    .ForMember(x => x.CouponRuleVal, y => y.MapFrom(z => z.CouponRuleVal))
                    .ForMember(x => x.CouponValue, y => y.MapFrom(z => z.CouponValue))
                    .ForMember(x => x.CouponStatus, y => y.MapFrom(z => z.CouponStatus))
                    .ForMember(x => x.CouponDesc, y => y.MapFrom(z => z.CouponDesc))
                    .ForMember(x => x.MaxLimitNum, y => y.MapFrom(z => z.MaxLimitNum))
                    .ForMember(x => x.CreateDate, y => y.MapFrom(z => z.CreateDate))
                    .ForMember(x => x.EndDate, y => y.MapFrom(z => z.EndDate))
                    .ForMember(x => x.OperatorId, y => y.MapFrom(z => z.OperatorId))
                    .ForMember(x => x.LongUrl, y => y.MapFrom(z => z.LongUrl))
                    .ForMember(x => x.ShortUrl, y => y.MapFrom(z => z.ShortUrl))
                    .ForMember(x => x.CrossShop, y => y.MapFrom(z => z.CrossShop))
                    .ForAllOtherMembers(x => x.Ignore());


            });
        }
    }
}