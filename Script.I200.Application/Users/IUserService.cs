using System;
using System.Collections.Generic;
using System.Data;
using Script.I200.Entity.Api.Users;
using Script.I200.Entity.API;
using Script.I200.Entity.Enum;
using Script.I200.Entity.Model.User;
using UserIntegral = Script.I200.Entity.Api.Users.UserIntegral;

namespace Script.I200.Application.Users
{
    /// <summary>
    ///     会员相关接口（C端用户）
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        ///     获取即将过生日的会员
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        ResponseModel GetBirthdayUsers(UserContext userContext,DateTime endDate);

        /// <summary>
        /// 搜索会员
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        ResponseModel SearchUser(UserContext userContext, string searchValue);
        /// <summary>
        /// 新增会员
        /// </summary>
        /// <returns></returns>
        ResponseModel AddUser(UserHandle request);
        /// <summary>
        /// 修改会员
        /// </summary>
        /// <returns></returns>
        ResponseModel UpdateUser(UserHandle request);
        /// <summary>
        /// 删除会员
        /// </summary>
        /// <returns></returns>
        ResponseModel DeleteUser();

        /// <summary>
        /// 校验-相同的卡号
        /// </summary>
        /// <param name="accId"></param>
        /// <param name="userNo"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool ExistSameUserNo(int accId, string userNo, int userId = 0);

        /// <summary>
        /// 校验-相同的手机号
        /// </summary>
        /// <param name="accId"></param>
        /// <param name="userPhone"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool ExistSameUserPhone(int accId, string userPhone, int userId = 0);
        /// <summary>
        /// 校验-相同的会员分组
        /// </summary>
        /// <param name="accId"></param>
        /// <param name="groupName"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        bool ExistSameUserGroupName(int accId, string groupName, int groupId=0);

        /// <summary>
        /// 设置消费密码
        /// </summary>
        /// <returns></returns>
        bool SetUserPassword(UserContext userContext, int userId, string password);

        /// <summary>
        /// 修改消费密码
        /// </summary>
        /// <returns></returns>
        int ChangeUserPassword(UserContext userContext, int userId, string oldPassword, string newPassword);

        /// <summary>
        /// 检查是否有设置消费密码
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool CheckUserPassword(UserContext userContext, int userId);

        /// <summary>
        /// 历史消息记录
        /// </summary>
        /// <returns></returns>
        List<UserMessage> GetUserMessages(UserContext userContext, int userId);

        /// <summary>
        /// 获取最新会员编号
        /// </summary>
        /// <param name="accId"></param>
        /// <returns></returns>
        ResponseModel GetUserNewNo(int accId);

        ResponseModel GetUserGroups(int accId);
        ResponseModel UpdateUserGroup(int[] userIds, int groupId, int accId);
        ResponseModel InsertUserGroup(UserGroup entity,int[] userIds);
        
        /// <summary>
        /// 获取会员详情
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserDetail GetUserDetail(UserContext userContext, int userId);

        /// <summary>
        /// 获取会员消费统计
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserStatistics GetUserStatistics(UserContext userContext, int userId);

        /// <summary>
        /// 更换会员头像
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="userId"></param>
        /// <param name="avatar"></param>
        /// <returns></returns>
        bool ChangeUserAvatar(UserContext userContext, int userId, string avatar);
        /// <summary>
        /// 获取标签列表
        /// </summary>
        /// <param name="accId"></param>
        /// <returns></returns>
        ResponseModel GetTagList(int accId);
        /// <summary>
        /// 给会员打标签
        /// </summary>
        /// <param name="accId"></param>
        /// <param name="operatorId"></param>
        /// <param name="userIds"></param>
        /// <param name="tagId"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        ResponseModel UpdateUserTag(int accId,int operatorId, int[] userIds,int tagId, IDbTransaction transaction = null);

        /// <summary>
        /// 新增标签并给会员打标签
        /// </summary>
        /// <param name="request"></param>
        /// <param name="accId"></param>
        /// <param name="userIds"></param>
        /// <returns></returns>
        ResponseModel InsertUserTag(UserTagInfo request,int accId, int[] userIds);

        bool ExistSameUserTagName(int accId, string tagName);
        ResponseModel DeleteUserTag(int accId, int userId, int tagId);
        ResponseModel GetUserNickList(int accId);
        ResponseModel AddUserNick(UserNickName entity);
        bool ExistSameUserNick(int accId, string nickName);

        /// <summary>
        /// 获取会员积分列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="userId"></param>
        /// <param name="searchType"></param>
        /// <returns></returns>
        UserIntegral GetUserIntegral(UserContext userContext, int userId, UserIntegralSearchTypeEnum searchType);

        /// <summary>
        /// 设置会员积分
        /// </summary>
        /// <param name="setType"></param>
        /// <param name="userContext"></param>
        /// <param name="userId"></param>
        /// <param name="integral"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        bool SetUserIntegral(UserIntegralSetTypeEnum setType, UserContext userContext, int userId, int integral,string remark);

        /// <summary>
        /// 会员消费记录
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="userId"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
        UserSale GetUserSales(UserContext userContext, int userId, string keywords);

        ResponseModel GetUserList(UserListSearch serarchParam, int accId);
        ResponseModel CheckPhoneNo(string phoneno, int accId);
        ResponseModel GetAccountAllCouponInfo(int accId);
        ResponseModel GetUserCouponAll(int accId, int userid, int masterId);
        ResponseModel SendUserCoupon(UserCouponBind request, UserContext getUserContext);
    }
}