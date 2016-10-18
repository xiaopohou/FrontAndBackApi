using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Script.I200.Core.Caching;
using Script.I200.Core.Config;
using Script.I200.Data;
using Script.I200.Entity;
using Script.I200.Entity.Api.OnlineMall;
using Script.I200.Entity.API;
using Script.I200.Entity.Dto.OnlineMall;
using Script.I200.Entity.Enum;
using Script.I200.Entity.Model.Onlinemall;

namespace Script.I200.Application.OnlineMall
{
    public class OnlineMallService : IOnlineMallService
    {
        private readonly DapperRepository<T_MaterialGoods2> _materialGoods;
        private readonly DapperRepository<T_MaterialGoodsPic> _materalgoodspic;
        private readonly DapperRepository<MobileEvaluation> _mobileEvaluation;
        private readonly DapperRepository<T_Receiving_Address> _receiveAddress;
        private readonly DapperRepository<ReceiveingAddressAdd> _receiveAddressAdd;

        public OnlineMallService()
        {
            var dapperDbContext = new DapperDbContext(new SqlConnection(WebConfigSetting.Instance.I200DbConnectionString));
            _materialGoods = new DapperRepository<T_MaterialGoods2>(dapperDbContext);
            _materalgoodspic = new DapperRepository<T_MaterialGoodsPic>(dapperDbContext);
            _mobileEvaluation = new DapperRepository<MobileEvaluation>(dapperDbContext);
            _receiveAddress = new DapperRepository<T_Receiving_Address>(dapperDbContext);
            _receiveAddressAdd = new DapperRepository<ReceiveingAddressAdd>(dapperDbContext);
        }

        /// <summary>
        /// 获取硬件商品简要信息列表
        /// </summary>
        /// <returns></returns>
        public ResponseModel GetIndexMaterialGoodsList(IEnumerable<int> gidInts)
        {
            var rModel = GetIndexMaterialGoodses2(gidInts);
            return new ResponseModel
            {
                Code = rModel == null ? (int)ErrorCodeEnum.NotFound : (int)ErrorCodeEnum.Success,
                Data = rModel
            };
        }

        /// <summary>
        /// 获取硬件商品详情
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public ResponseModel GetMaterialGoodsInfo(int goodsId)
        {
            return new ResponseModel { Code = (int)ErrorCodeEnum.Success, Message = null, Data = GetMaterialGoodsDetails(goodsId) };
        }

        /// <summary>
        /// 获取硬件评论
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public ResponseModel GetMaterialEvaluation(int goodsId)
        {
            var rModel = GetMaterialEvaluationListByGoodsId(goodsId);
            return new ResponseModel
            {
                Code = rModel == null ? (int)ErrorCodeEnum.NotFound : (int)ErrorCodeEnum.Success,
                Data = rModel
            };
        }

        /// <summary>
        /// 获取筛选后的硬件商品ID
        /// </summary>
        /// <param name="typEnum"></param>
        /// <returns></returns>
        public IEnumerable<int> GetMaterialGoodsId(GetMaterialIdEnum typEnum)
        {
            IEnumerable<int> rEnumerable;
            switch (typEnum)
            {
                case GetMaterialIdEnum.Pcshow:
                    rEnumerable = GetMaterialGoodsIdList()
                        .Where(x => (x.Status == (int)MaterialGoodsStatusEnum.Open || x.Status == (int)MaterialGoodsStatusEnum.PcOnly))
                        .Select(x => x.GoodsId);
                    break;
                case GetMaterialIdEnum.MobileShow:
                    rEnumerable = GetMaterialGoodsIdList()
                        .Where(x => (x.Status == (int)MaterialGoodsStatusEnum.Open || x.Status == (int)MaterialGoodsStatusEnum.MobileOnly))
                        .Select(x => x.GoodsId);
                    break;
                default:
                    rEnumerable = null;
                    break;
            }
            return rEnumerable;
        }

        public ResponseModel GetLastAddressModel(int accId)
        {
            var entity = _receiveAddress.FindAll(
                x => x.AccId == accId).OrderByDescending(x => x.CreateTime).FirstOrDefault();
            return new ResponseModel
            {
                Code = entity == null ? (int)ErrorCodeEnum.NotFound : (int)ErrorCodeEnum.Success,
                Data = entity
            };
        }

        public ResponseModel AddAddressModel(ReceiveingAddressAdd entity)
        {
            //1.添加支出数据
            var result = _receiveAddressAdd.Insert(entity);

            //2.返回新增的支出实体
            return new ResponseModel
            {
                Code = result ? (int)ErrorCodeEnum.Success : (int)ErrorCodeEnum.ExpenseFailed,
                Data = entity
            };
        }

        #region 硬件相关

        /// <summary>
        /// 根据ID获取硬件商品相应数据
        /// 新方法更快
        /// </summary>
        /// <param name="gidInts"></param>
        /// <returns></returns>
        private List<IndexMaterialGoods> GetIndexMaterialGoodses2(IEnumerable<int> gidInts)
        {
            var rList = new List<IndexMaterialGoods>();
            var mList = new List<MaterialGoodsInfo>();
            var goodsList = GetMaterialGoodsIdList();
            if (goodsList.Any() && gidInts.Any())
            {
                var redisCacheService = new RedisCacheService();
                mList =
                    redisCacheService
                        .Get<MaterialGoodsInfo>(
                            gidInts.Select(x => RedisConsts.MaterialGoodsInfo + x.ToString()).ToArray())
                        .ToList();
                if (gidInts.Count() != mList.Count)
                {
                    mList.AddRange(
                        gidInts
                            .Except(mList.Select(x => x.GoodsId))
                            .Select(GetMaterialGoodsDetails)
                        );
                }
            }
            if (mList.Any())
            {
                rList.AddRange(mList.Select(goods => new IndexMaterialGoods
                {
                    Id = goods.Id,
                    GoodsId = goods.GoodsId,
                    AliasName = goods.AliasName,
                    Price = goods.Price,
                    Status = goods.Status,
                    RankNo = goods.RankNo,
                    ClassId = goods.ClassId,
                    CoverImage = goods.CoverImage
                }));
                return rList.OrderBy(x=>x.RankNo).ToList();
            }
            return null;
        }


        /// <summary>
        /// redis获取商品列表
        /// </summary>
        /// <returns></returns>
        private IEnumerable<MaterialGoodsListRedisItem> GetMaterialGoodsIdList()
        {
            IEnumerable<MaterialGoodsListRedisItem> rModel;
            string key = RedisConsts.MaterialGoodsList;
            var redisCacheService = new RedisCacheService();
            if (redisCacheService.HasKey(key))
            {
                rModel = redisCacheService.Get<List<MaterialGoodsListRedisItem>>(key);
            }
            else
            {
                rModel = Mapper.Map<IEnumerable<T_MaterialGoods2>, IEnumerable<MaterialGoodsListRedisItem>>(_materialGoods.FindAll());

                //Insert Cache 7days
                Task.Factory.StartNew(() =>
                {
                    redisCacheService.Set(key, rModel, 60 * 60 * 24 * 7);
                });
            }
            return rModel;
        }

        /// <summary>
        /// redis获取商品详情
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        private MaterialGoodsInfo GetMaterialGoodsDetails(int goodsId)
        {
            MaterialGoodsInfo entity;
            string key = RedisConsts.MaterialGoodsInfo + goodsId;
            var redisCacheService = new RedisCacheService();
            if (redisCacheService.HasKey(key))
            {
                entity = redisCacheService.Get<MaterialGoodsInfo>(key);
            }
            else
            {
                entity = GetMaterialGoodsDb(goodsId);
                //Insert Cache 7days
                Task.Factory.StartNew(() =>
                {
                    redisCacheService.Set(key, entity, 60 * 60 * 24 * 7);
                });
            }
            return entity;
        }

        /// <summary>
        /// 获取商品详情从DB（针对移动端）
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        private MaterialGoodsInfo GetMaterialGoodsDb(int goodsId)
        {
            MaterialGoodsInfo entity = Mapper.Map<T_MaterialGoods2, MaterialGoodsInfo>(_materialGoods.Find(x => x.GoodsId == goodsId));
            var listPic = _materalgoodspic.FindAll(x => x.GoodsId == goodsId);
            var materialGoodsPics = listPic as IList<T_MaterialGoodsPic> ?? listPic.ToList();
            if (listPic != null && materialGoodsPics.Any())
            {
                entity.CoverImage =
                    new List<T_MaterialGoodsPic>(
                        materialGoodsPics
                        .Where(c => c.Type == (int)MaterialGoodsPicTypeEnum.CoverImage && c.Status == 0)
                        .OrderBy(c => c.RankNo));
                //TODO:移动端特殊处理
                entity.Description =
                    new List<T_MaterialGoodsPic>(
                        materialGoodsPics
                        .Where(c => c.Type == (int)MaterialGoodsPicTypeEnum.MobileDescription && c.Status == 0)
                        .OrderBy(c => c.RankNo));
                entity.HeadThumb =
                    new List<T_MaterialGoodsPic>(
                        materialGoodsPics
                        .Where(c => c.Type == (int)MaterialGoodsPicTypeEnum.HeadThumb && c.Status == 0)
                        .OrderBy(c => c.RankNo));
            }
            return entity;
        }
        #endregion

        #region 评论相关

        private List<MobileEvaluationBase> GetMaterialEvaluationListByGoodsId(int goodsId)
        {
            var entity = _mobileEvaluation.FindAll(
                x => x.productType == 2
                && x.isDelete == 1
                && x.isDisplay == 0
                && x.accId == 0
                && x.productID == goodsId);
            List<MobileEvaluationBase> iList = new List<MobileEvaluationBase>();
            if (entity != null && entity.Any())
            {
                iList.AddRange(entity.Select(mobileEvaluation => new MobileEvaluationBase
                {
                    DummyName = mobileEvaluation.DummyName,
                    content = mobileEvaluation.content,
                    createTime = mobileEvaluation.createTime
                }));
                return iList;
            }
            return null;
        }

        #endregion


    }
}
