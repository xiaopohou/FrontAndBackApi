using System;
using System.Web.Http;
using Script.I200.Application.Goods;
using Script.I200.Application.UserTimesCard;
using Script.I200.Entity.Api.UserTimesCard;
using Script.I200.Entity.API;
using Script.I200.Entity.Enum;
using Script.I200.Entity.Model.Goods;
using Script.I200.Entity.Model.TimesCard;
using Script.I200.Entity.Model.User;

namespace Script.I200.WebAPI.Controllers
{
    /// <summary>
    ///     用户计次卡接口
    /// </summary>
    [RoutePrefix("v0")]
    public class UserTimesCardController : BaseApiController
    {
        private readonly IGoodsService _goodsService;
        private readonly IUserTimesCardService _userTimesCardService;

        public UserTimesCardController()
        {
            _userTimesCardService = new UserTimesCardService();
            _goodsService = new GoodsService();
        }

        /// <summary>
        ///     添加店铺计次卡
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("users/timesCards/card")]
        [HttpPost, HttpOptions]
        public ResponseModel AddAccountTimesCard(AccountTimesCard request)
        {
            ResponseModel rModel;
            var userContext = GetUserContext();
            //1.入参校验：

            //1.1 参数为空
            if (request == null)
                return Fail(ErrorCodeEnum.TimeCardIsNullArguments);

            request.AddTime = DateTime.Now;
            request.EditTime = DateTime.Now;

            //1.2 参数校验-参数不合法
            if (CheckModelParams(out rModel)) return rModel;

            //1.3 参数校验-是否存在该记录,卡名不能重复
            if (_userTimesCardService.ExistSameNameTimesCard(userContext.AccId,
                request.CardName))
                return Fail(ErrorCodeEnum.TimeCardExistSameNameTimesCard);
            if (request.BindGoodsId == 0)
            {
                //1.4 参数校验-是否已经存在无限制计次卡类
                if (_userTimesCardService.ExistUnlimitedTimesCard(userContext.AccId))
                    return Fail(ErrorCodeEnum.TimeCardExistUnlimitedTimesCard);
            }

            //1.5 参数校验-是否存在该记录,一个服务只能对应一个计次卡类
            if (_userTimesCardService.ExistServiceOnly(userContext.AccId, request.BindGoodsId))
                return Fail(ErrorCodeEnum.TimeCardExistServiceOnly);

            //1.6 校验绑定的服务类商品是否存在
            if (request.BindGoodsId != 0)
            {
                var goodsinfo = _goodsService.GetGoodsInfoByGid(request.BindGoodsId, userContext);
                if (goodsinfo.Code != (int) ErrorCodeEnum.Success)
                {
                    return Fail(ErrorCodeEnum.NotFoundServiceItem);
                }

                var gModel = (GoodsInfoSummary) goodsinfo.Data;
                //1.7 参数校验-必须为服务类商品
                if (gModel.IsService != 1)
                    return Fail(ErrorCodeEnum.TimeCardIsService);
            }

            //TODO: 添加版本校验，是否添加多张计次卡


            //1.8 新增店铺计次卡
            var result = _userTimesCardService.AddAccountTimesCard(request, userContext);
            rModel = Success(result);
            return rModel;
        }

        /// <summary>
        ///     修改店铺计次卡
        /// </summary>
        /// <param name="timesCard"></param>
        /// <returns></returns>
        [Route("users/timesCards/card")]
        [HttpPut, HttpOptions]
        public ResponseModel UpdateAccountTimesCard(AccountTimesCard timesCard)
        {
            var userContext = GetUserContext();
            //1.入参校验：

            //1.1 参数为空
            if (timesCard == null)
            {
                return Fail(ErrorCodeEnum.NullArguments);
            }
            ResponseModel checkResult;
            //1.2 参数校验-参数不合法
            if (CheckModelParams(out checkResult)) return checkResult;

            //1.3 参数校验-获取不到此计次卡信息
            var accountTimeCard = _userTimesCardService.GetAccountTimesCard(timesCard.Id, userContext.AccId);
            if (accountTimeCard == null)
                return Fail(ErrorCodeEnum.TimeCardGetById);

            accountTimeCard.CardName = timesCard.CardName;
            //1.4 参数校验-是否存在该记录,卡名不能重复
            if (_userTimesCardService.ExistSameNameTimesCard(userContext.AccId,
                accountTimeCard.CardName))
                return Fail(ErrorCodeEnum.TimeCardExistSameNameTimesCard);

            var result = _userTimesCardService.UpdateAccountTimesCard(accountTimeCard, userContext);
            return Success(result);
        }

        /// <summary>
        ///     删除店铺计次卡
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        [Route("users/timesCards/{cardId}")]
        [HttpDelete, HttpOptions]
        public ResponseModel DeleteAccountTimesCard(int cardId)
        {
            var userContext = GetUserContext();
            //1.入参校验：
            //1.1 参数校验-获取不到此计次卡信息
            var accountTimeCard = _userTimesCardService.GetAccountTimesCard(cardId, userContext.AccId);
            if (accountTimeCard == null)
                return Fail(ErrorCodeEnum.TimeCardGetById);

            var result = _userTimesCardService.DeleteAccountTimesCard(cardId, userContext);
            return Success(result);
        }

        /// <summary>
        ///     获取店铺计次卡列表
        /// </summary>
        /// <returns></returns>
        [Route("users/timesCards/list")]
        [HttpGet, HttpOptions]
        public ResponseModel GetAccountTimesCardsList([FromUri] AccountTimeCardsSearchParam searchParam)
        {
            if (searchParam == null)
            {
                searchParam = new AccountTimeCardsSearchParam();
            }

            ResponseModel functionReturn;
            if (CheckIsMoreThanPageSize(searchParam.PageSize, out functionReturn)) return functionReturn;
            var result = _userTimesCardService.GetAccountTimesCardsList(searchParam, GetUserContext());
            return Success(result);
        }

        /// <summary>
        ///     创建用户计次卡
        /// </summary>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("users/userTimesCards/bindCard")]
        public ResponseModel AddUserTimesCard(UserStoreTimes request)
        {
            //1.参数为空校验
            if (request == null)
            {
                return Fail(ErrorCodeEnum.TimeCardIsNullArguments);
            }

            var userContext = GetUserContext();
            if (request.UserId > 0)
            {
                //2.校验用户计次卡是否存在，存在则不能创建
                var checkResult = _userTimesCardService.GetUserTimesCardInfo(request.UserId, userContext,
                    request.AccTimesCardId);
                if (checkResult != null)
                {
                    return Fail(ErrorCodeEnum.ExistSameUserTimesCard);
                }    
            }

            request.AccId = userContext.AccId;
            request.EditTime = DateTime.Now;
            return _userTimesCardService.AddUserTimesCard(userContext, request);
        }

        /// <summary>
        ///     修改用户计次卡
        /// </summary>
        /// <returns></returns>
        [HttpPut, HttpOptions]
        [Route("users/userTimesCards/card")]
        public ResponseModel EditUserTimesCard(UserStoreTimes request)
        {
            //参数为空校验
            if (request == null) return Fail(ErrorCodeEnum.NullArguments);
            var userContext = GetUserContext();
            return _userTimesCardService.EditUserTimesCard(userContext, request);
        }

        /// <summary>
        ///     删除用户计次卡
        /// </summary>
        /// <returns></returns>
        [HttpDelete, HttpOptions]
        [Route("users/userTimesCards/{cardId}")]
        public ResponseModel DeleteUserTimesCard(int cardId)
        {
            return _userTimesCardService.DeleteUserTimesCard(GetUserContext(), cardId);
        }

        /// <summary>
        ///     获取用户计次卡列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("users/userTimesCards/list")]
        public ResponseModel GetUserTimesCardList([FromUri]UserTimesCardSearchParam searchParam)
        {
            //1.判断查询参数是否为空，为空则初始化当前页面大小为PageSize=25，CurrentPage=1
            if (searchParam == null)
            {
                searchParam = new UserTimesCardSearchParam();
            }
            
            //2.校验单页最大数据量不超过100条,防止通过接口单页请求大批量数据
            ResponseModel functionReturn;
            if (CheckIsMoreThanPageSize(searchParam.PageSize, out functionReturn)) return functionReturn;
            return _userTimesCardService.GetUserTimesCardList(GetUserContext(), searchParam);
        }

        /// <summary>
        ///     获取用户计次卡交易记录列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("users/userTimesCards/records")]
        public ResponseModel GetUserTimesCardChargeList(UserTimesCardTransactionRecordSearchParam searchParam)
        {
            //1.判断查询参数是否为空，为空则初始化当前页面大小为PageSize=25，CurrentPage=1
            if (searchParam == null)
            {
                searchParam = new UserTimesCardTransactionRecordSearchParam();
            }

            ResponseModel functionReturn;
            if (CheckIsMoreThanPageSize(searchParam.PageSize, out functionReturn)) return functionReturn;
            return _userTimesCardService.GetUserTimesCardChargeList(GetUserContext(), searchParam);
        }

        /// <summary>
        ///     用户计次卡充次
        /// </summary>
        /// <returns></returns>
        [HttpPut, HttpOptions]
        [Route("users/userTimesCards/{userTimesCardId}/incharge")]
        public ResponseModel UserTimesCardIncharge(UserTimesCardAdd request)
        {
            //参数为空校验
            if (request == null)
            {
                Fail(ErrorCodeEnum.NullArguments);
            }
            //1.校验用户的电话号码和用户姓名不为空
            if (request!=null && request.UserId==0)
            {
                if (string.IsNullOrWhiteSpace(request.Phone)|| string.IsNullOrWhiteSpace(request.UserName))
                {
                    Fail(ErrorCodeEnum.PhoneAndUserNameIsNull);
                }
            }

            return FunctionReturn(_userTimesCardService.UserTimesCardIncharge(GetUserContext(), request));
        }

        /// <summary>
        ///     获取当前的店铺的所有计次卡项目，提供前台页面下拉框筛选(用于绑定用户计次卡)
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("users/timesCards")]
        public ResponseModel GetCurrentStoreTimesCardNameByAccId()
        {
            return _userTimesCardService.GetCurrentStoreTimesCardNameByAccId(GetUserContext());
        }

        /// <summary>
        ///     获取当前的店铺的服务类项目
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("users/userTimesCards/getStoreTimesCardBindingItem")]
        public ResponseModel GetCurrentStoreServiceItemExceptBinging()
        {
            return _userTimesCardService.GetCurrentStoreServiceItemExceptBinging(GetUserContext());
        }

        /// <summary>
        ///    根据用户计次卡Id获取用户计次卡信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("users/userTimesCards/{cardId}")]
        public ResponseModel GetUserTimesCardInfoByCardId(int cardId)
        {
            return FunctionReturn(_userTimesCardService.GetUserTimesCardInfoByCardId(GetUserContext(),cardId));
        }

        /// <summary>
        /// 计次卡充次时，根据会员查询结果，如果是会员：（1）该会员有计次卡，下拉框计次卡列表显示会员已有计次卡，该会员无计次卡，下拉显示
        /// 当前店铺所有计次卡 ； 如果是非会员，下拉显示当前店铺所有计次卡
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("users/userTimesCards/cardList")]
        public ResponseModel GetUserTimesCardOrAccTimesCard([FromUri]UserHandle userInfo)
        {
            return FunctionReturn(_userTimesCardService.GetUserTimesCardOrAccTimesCard(GetUserContext(), userInfo));
        }
    }
}