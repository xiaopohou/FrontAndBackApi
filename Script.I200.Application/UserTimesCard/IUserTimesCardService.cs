using Script.I200.Entity.Api.UserTimesCard;
using Script.I200.Entity.API;
using Script.I200.Entity.Model.TimesCard;
using Script.I200.Entity.Model.User;

namespace Script.I200.Application.UserTimesCard
{
    public interface IUserTimesCardService
    {
        /// <summary>
        /// 新增店铺计次卡
        /// </summary>
        /// <returns></returns>
        ResponseModel AddAccountTimesCard(AccountTimesCard request, UserContext userContext);

        /// <summary>
        /// 是否已经存在无限制计次卡类
        /// </summary>
        /// <param name="accId"></param>
        /// <returns></returns>
        bool ExistUnlimitedTimesCard(int accId);

        /// <summary>
        /// 是否存在该记录,一个服务只能对应一个计次卡类
        /// </summary>
        /// <param name="accId"></param>
        /// <param name="bindGoodsId"></param>
        /// <returns></returns>
        bool ExistServiceOnly(int accId, int bindGoodsId);

        /// <summary>
        /// 是否存在该记录,卡名不能重复
        /// </summary>
        /// <param name="accId"></param>
        /// <param name="cardName"></param>
        /// <returns></returns>
        bool ExistSameNameTimesCard(int accId, string cardName);

        /// <summary>
        /// 修改店铺计次卡
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userContext"></param>
        /// <returns></returns>
        ResponseModel UpdateAccountTimesCard(AccountTimesCard request, UserContext userContext);

        /// <summary>
        /// 删除店铺计次卡
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="userContext"></param>
        /// <returns></returns>
        ResponseModel DeleteAccountTimesCard(int cardId, UserContext userContext);

        /// <summary>
        /// 获取店铺计次卡列表
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="userContext"></param>
        /// <returns></returns>
        ResponseModel GetAccountTimesCardsList(AccountTimeCardsSearchParam searchParam, UserContext userContext);

        /// <summary>
        /// 获取店铺计次卡信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="accId"></param>
        /// <returns></returns>
        AccountTimesCard GetAccountTimesCard(int id, int accId);

        /// <summary>
        ///  创建用户计次卡
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        ResponseModel AddUserTimesCard(UserContext userContext, UserStoreTimes request);

        /// <summary>
        /// 修改用户计次卡
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        ResponseModel EditUserTimesCard(UserContext userContext, UserStoreTimes request);

        /// <summary>
        /// 删除用户计次卡
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        ResponseModel DeleteUserTimesCard(UserContext userContext, int cardId);

        /// <summary>
        /// 用户计次卡充次
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        ResponseModel UserTimesCardIncharge(UserContext userContext, UserTimesCardAdd request);

        /// <summary>
        /// 获取用户计次卡列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="searchParam"></param>
        /// <returns></returns>
        ResponseModel GetUserTimesCardList(UserContext userContext, UserTimesCardSearchParam searchParam);

        /// <summary>
        /// 获取用户计次卡交易记录
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="searchParam"></param>
        /// <returns></returns>
        ResponseModel GetUserTimesCardChargeList(UserContext userContext,
            UserTimesCardTransactionRecordSearchParam searchParam);

        /// <summary>
        ///  获取当前店铺的所有计次卡项提供前台页面下拉筛选项
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        ResponseModel GetCurrentStoreTimesCardNameByAccId(UserContext userContext);

        /// <summary>
        /// 获取当前的店铺的服务类项目
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        ResponseModel GetCurrentStoreServiceItemExceptBinging(UserContext userContext);

        /// <summary>
        /// 获取用户计次卡信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userContext"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        UserStoreTimes GetUserTimesCardInfo(int userId,UserContext userContext,int cardId);

        /// <summary>
        /// 根据用户计次卡Id获取用户计次卡信息
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        ResponseModel GetUserTimesCardInfoByCardId(UserContext userContext, int cardId);

        /// <summary>
        /// 计次卡充次时，根据会员查询结果，如果是会员：（1）该会员有计次卡，下拉框计次卡列表显示会员已有计次卡，该会员无计次卡，下拉显示
        /// 当前店铺所有计次卡 ； 如果是非会员，下拉显示当前店铺所有计次卡
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        ResponseModel GetUserTimesCardOrAccTimesCard(UserContext userContext,UserHandle userInfo);
    }
}