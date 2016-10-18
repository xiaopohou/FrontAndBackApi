namespace Script.I200.Entity
{
    public class RedisConsts
    {
        public const string AddressBaseData = "rd_addressBaseData";
        public const string ConstructedProvinceCityList = "rd_constructedProvinceCityList";

        /// <summary>
        /// 电脑端PC Key
        /// </summary>
        public const string WebUserTokenKey = "I200_PC_UserToken:";

        /// <summary>
        /// 移动端用户Token Key
        /// </summary>
        public const string AppUserTokenKey = "I200_App_UserToken:";

        /// <summary>
        /// 支出分类列表缓存Key
        /// </summary>
        public const string PayClassList = "rd_payClassList:";

        /// <summary>
        /// 站内广告Key
        /// </summary>
        public const string StationAdvertKey = "SYSTEM:Station:Advert:";

        /// <summary>
        /// 硬件列表id,status list
        /// </summary>
        public const string MaterialGoodsList = "SYSTEM:materialgoods:list";
        /// <summary>
        /// 硬件详情
        /// </summary>
        public const string MaterialGoodsInfo = "SYSTEM:materialgoodsinfo:";

        /// <summary>
        /// 硬件每日优惠券
        /// </summary>
        public const string DailyMaterialCoupon = "I200_DailyMaterialCoupon:";

        /// <summary>
        /// 优惠券集合
        /// </summary>
        public const string OrderCouponIdSet = "I200_OrderCouponIdSet:All";
        /// <summary>
        /// 版本权限
        /// </summary>
        public const string AccountVersion = "I200_AccountVersion:";
    }
}
