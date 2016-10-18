using Script.I200.Entity.Api.UserStoreMoneyCard;
using Script.I200.Entity.API;

namespace Script.I200.Application.UserStoreMoneyCard
{
    public interface IUserStoreMoneyCardService
    {
        /// <summary>
        /// 会员充值
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="userStoreParam"></param>
        /// <returns></returns>
        ResponseModel UserStoreMoney(UserContext userContext, UserStoreMoneyAdd userStoreParam);

        /// <summary>
        /// 获取会员储值卡列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="searchParam"></param>
        /// <returns></returns>
        ResponseModel GetUserStoreCardsList(UserContext userContext, UserStoreCardSearchParam searchParam);

        /// <summary>
        /// 获取储值历史记录
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="searchParam"></param>
        /// <returns></returns>
        ResponseModel GetStoreMoneyHistoryList(UserContext userContext, UserStoreMoneySearchParam searchParam);

        /// <summary>
        /// 获取储值业务状态列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        ResponseModel GetBussinessStatus(UserContext userContext);
    }
}