using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CommonLib;
using Script.I200.Core.Caching;
using Script.I200.Core.Config;
using Script.I200.Data;
using Script.I200.Entity;
using Script.I200.Entity.Enum;
using Script.I200.Entity.Model.Order;
using ResponseModel = Script.I200.Entity.API.ResponseModel;

namespace Script.I200.Application.OnlineMall
{
    public class OrderCouponService : IOrderCouponService
    {
        private readonly DapperRepository<T_Order_CouponList> _orderCouponList;
        private static readonly object LockOrderCouonObject = new object();
        public OrderCouponService()
        {
            var dapperDbContext = new DapperDbContext(new SqlConnection(WebConfigSetting.Instance.I200DbConnectionString));
            _orderCouponList = new DapperRepository<T_Order_CouponList>(dapperDbContext);
        }

        #region 优惠券相关

        public bool IsHasNewMaterialCoupon(int accId, int groupId)
        {
            var couponCount = _orderCouponList.FindAll(x => x.groupId == groupId && x.toAccId == accId).Count();
            return couponCount > 0;
        }

        public bool IsHasDailyMaterialCoupon(int accId, int groupId)
        {
            string key = RedisConsts.DailyMaterialCoupon + groupId.ToString() + accId.ToString();
            var redisCacheService = new RedisCacheService();
            var redisvalue = redisCacheService.Get(key);
            return redisvalue != "True";
        }

        public bool HadReceivedDailyMaterialCoupon(int accId, int groupId,int totalSeconds)
        {
            string key = RedisConsts.DailyMaterialCoupon + groupId.ToString() + accId.ToString();
            var redisCacheService = new RedisCacheService();
            return redisCacheService.Set(key, true.ToString(), totalSeconds);
        }

        public ResponseModel CreateCoupon(T_Order_CouponList couponModel, string preName)
        {
            string strCouponCode = CreateCouponCode(couponModel, preName);
            couponModel.couponId = strCouponCode;
            bool isFail = string.IsNullOrEmpty(strCouponCode);
            return new ResponseModel
            {
                Code = isFail ?  (int)ErrorCodeEnum.CreateCouponFail : (int)ErrorCodeEnum.Success,
                Data = couponModel
            };
        }

        #endregion

        #region 生成优惠券相关redis操作方法

        /// <summary>
        /// 生成优惠券编码
        /// </summary>
        /// <param name="couponModel"></param>
        /// <param name="preName"></param>
        /// <returns>优惠券编码</returns>
        private string CreateCouponCode(T_Order_CouponList couponModel, string preName)
        {
            //生成编码 判断是否重复
            string strCouponCode = "";
            for (int i = 0; i < 10; i++)
            {
                strCouponCode = CreateCode(preName);
                if (CreateCouponToRedisSet(strCouponCode, i == 0))
                    break;
                strCouponCode = "";
            }
            if (!string.IsNullOrEmpty(strCouponCode))
            {
                couponModel.couponId = strCouponCode;
                if (_orderCouponList.Insert(couponModel))
                {
                    return strCouponCode;
                }
                else
                {
                    RemoveCouponCode(strCouponCode);
                }
            }
            return null;

        }

        /// <summary>
        /// 生成优惠券到redis集合set,成功返回true,失败返回false
        /// </summary>
        /// <param name="couponCode"></param>
        /// <param name="isFirst"></param>
        /// <returns></returns>
        private bool CreateCouponToRedisSet(string couponCode, bool isFirst)
        {
            //首次添加查询是否存在key
            if (isFirst)
            {
                var redisCacheService = new RedisCacheService();
                if (!redisCacheService.HasKey(RedisConsts.OrderCouponIdSet))
                {
                    AddCouponArrayToRedis();
                }
            }
            return AddCouponToRedis(couponCode);
        }


        /// <summary>
        /// 生成优惠券编码
        /// </summary>
        /// <param name="preName"></param>
        /// <returns></returns>
        private string CreateCode(string preName)
        {
            return preName + Helper.CreateUUID();
        }

        /// <summary>
        /// 去除couponCode
        /// </summary>
        /// <param name="couponCode"></param>
        private void RemoveCouponCode(string couponCode)
        {
            Task.Run(() =>
            {
                var redis = new RedisCacheService();
                redis.SetRemove(RedisConsts.OrderCouponIdSet, couponCode);
            });
        }
        /// <summary>
        /// 添加优惠券code到redis set集合中
        /// </summary>
        /// <param name="couponCode"></param>
        /// <returns></returns>
        private bool AddCouponToRedis(string couponCode)
        {
            var redis = new RedisCacheService();
            return redis.SetAdd(RedisConsts.OrderCouponIdSet, couponCode);
        }

        /// <summary>
        /// 添加优惠券数组到redis中
        /// </summary>
        /// <returns></returns>
        private void AddCouponArrayToRedis()
        {
            lock (LockOrderCouonObject)
            {
                string[] couponArray = GetAllCouponArray();
                var redis = new RedisCacheService();
                redis.SetAdd(RedisConsts.OrderCouponIdSet, couponArray);
            }
        }

        /// <summary>
        /// 获取所有couponId数组
        /// </summary>
        /// <returns></returns>
        private string[] GetAllCouponArray()
        {
            return _orderCouponList
                .FindAll(x => x.couponStatus == 0 || x.couponStatus == 2)
                .Select(x => x.couponId)
                .GroupBy(x => x)
                .Select(x => x.First()).ToArray();
        }
        #endregion
    }
}
