using System.Web.Http;
using Script.I200.Application.UserStoreMoneyCard;
using Script.I200.Entity.Api.UserStoreMoneyCard;
using Script.I200.Entity.API;
using Script.I200.Entity.Enum;

namespace Script.I200.WebAPI.Controllers
{
    /// <summary>
    ///     用户储值接口
    /// </summary>
    [RoutePrefix("v0")]
    public class UserStoreMoneyController : BaseApiController
    {
        private readonly IUserStoreMoneyCardService _userStoreMoneyCardService;

        /// <summary>
        ///     初始化
        /// </summary>
        public UserStoreMoneyController()
        {
            _userStoreMoneyCardService = new UserStoreMoneyCardService();
        }

        /// <summary>
        ///     会员充值
        /// </summary>
        /// <returns></returns>
        [Route("users/storeMoney/recharge")]
        [HttpPost, HttpOptions]
        public ResponseModel UserStoreMoney(UserStoreMoneyAdd userStoreParam)
        {
            //1.入参校验
            ResponseModel checkResult;
            if (userStoreParam == null)
            {
                return Fail(ErrorCodeEnum.StoreMoneyParamsIsNull);
            }

            //2.校验Model
            if (CheckModelParams(out checkResult)) return checkResult;

            //3. 校验储值金额和实收金额，说明：【实收金额不能大于储值金额】
            if (userStoreParam.RealMoney > userStoreParam.RechargeMoney)
            {
                return Fail(ErrorCodeEnum.StoreMoneyIsMoreThanRechargeMoney);
            }

            //4.储值时，若不是会员，则要求会员手机号和姓名必填,不能为空
            if (userStoreParam.UserId==0)
            {
                if (string.IsNullOrWhiteSpace(userStoreParam.Phone)||string.IsNullOrWhiteSpace(userStoreParam.UserName))
                {
                    return Fail(ErrorCodeEnum.PhoneAndUserNameIsNull);
                }
            }

            return _userStoreMoneyCardService.UserStoreMoney(GetUserContext(), userStoreParam);
        }

        /// <summary>
        ///     获取会员储值卡列表
        /// </summary>
        /// <returns></returns>
        [Route("users/storeMoney/list")]
        [HttpGet, HttpOptions]
        public ResponseModel GetUserStoreCardsList([FromUri] UserStoreCardSearchParam searchParam)
        {
            //1.判断查询参数是否为空，为空则初始化当前页面大小为PageSize=25，CurrentPage=1
            if (searchParam == null)
            {
                searchParam = new UserStoreCardSearchParam();
            }

            //2.校验单页最大数据量不超过100条,防止通过接口单页请求大批量数据
            if (searchParam.PageSize != null && searchParam.PageSize > 100)
            {
                return FunctionReturn(new ResponseModel
                {
                    Code = (int) ErrorCodeEnum.MoreThanMaxSize
                });
            }

            return _userStoreMoneyCardService.GetUserStoreCardsList(GetUserContext(), searchParam);
        }

        /// <summary>
        ///     获取储值历史记录
        /// </summary>
        /// <param name="searchParam"></param>
        /// <returns></returns>
        [Route("users/storeMoney/recordes")]
        [HttpGet, HttpOptions]
        public ResponseModel GetStoreMoneyHistoryList([FromUri] UserStoreMoneySearchParam searchParam)
        {
            //1.校验查询参数是否为空，空则新建对象
            if (searchParam == null)
            {
                searchParam = new UserStoreMoneySearchParam();
            }

            //2.校验单页最大数据量不超过100条,防止通过接口单页请求大批量数据
            if (searchParam.PageSize != null && searchParam.PageSize > 100)
            {
                return FunctionReturn(new ResponseModel
                {
                    Code = (int) ErrorCodeEnum.MoreThanMaxSize
                });
            }

            //3.校验查询的日期参数是否合法（开始日期小于等于结束日期）
            if (searchParam.StartDate != null && searchParam.EndDate != null &&
                searchParam.StartDate > searchParam.EndDate)
            {
                return FunctionReturn(new ResponseModel
                {
                    Code = (int) ErrorCodeEnum.SearchDateInvalid
                });
            }

            return _userStoreMoneyCardService.GetStoreMoneyHistoryList(GetUserContext(), searchParam);
        }

        /// <summary>
        ///     获取储值业务状态列表
        /// </summary>
        /// <returns></returns>
        [Route("list-status")]
        [HttpGet, HttpOptions]
        public ResponseModel GetBussinessStatus()
        {
            return _userStoreMoneyCardService.GetBussinessStatus(GetUserContext());
        }
    }
}