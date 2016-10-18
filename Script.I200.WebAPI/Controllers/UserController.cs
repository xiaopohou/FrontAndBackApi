using System;
using System.Web.Http;
using CommonLib;
using Script.I200.Application.BasicData;
using Script.I200.Application.Users;
using Script.I200.Entity.Api.Users;
using Script.I200.Entity.Enum;
using Script.I200.Entity.Model.User;
using ResponseModel = Script.I200.Entity.API.ResponseModel;
using UserIntegral = Script.I200.Entity.Dto.User.UserIntegral;

namespace Script.I200.WebAPI.Controllers
{
    /// <summary>
    ///     会员相关（C端用户）
    /// </summary>
    [RoutePrefix("v0/users")]
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IBasicDataService _basicDataService;

        /// <summary>
        ///     初始化
        /// </summary>
        public UserController()
        {
            _userService = new UserService();
            _basicDataService = new BasicDataService();
        }

        /// <summary>
        ///     获取即将过生日的会员
        /// </summary>
        /// <returns></returns>
        [Route("birthdayUsers/{endDate}")]
        [HttpGet]
        public ResponseModel GetBirthdayUsers(DateTime endDate)
        {
            return _userService.GetBirthdayUsers(GetUserContext(), endDate);
        }

        /// <summary>
        ///     搜索会员
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpGet, HttpOptions]
        public ResponseModel SearchUser([FromUri] string keyword)
        {
            return FunctionReturn(_userService.SearchUser(GetUserContext(), keyword));
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpPost, HttpOptions]
        public ResponseModel AddUser(UserHandle request)
        {
            //1.入参校验
            //1.1 参数为空
            ResponseModel checkResult;
            var userContext = GetUserContext();

            var result = CheckRequestParamIsNull(request, out checkResult);
            if (!result)
                return checkResult;

            //1.2 参数校验-参数不合法
            if (CheckModelParams(out checkResult))
                return checkResult;

            //1.3 参数校验-相同的卡号
            if (_userService.ExistSameUserNo(userContext.AccId, request.UserNo))
                return Fail(ErrorCodeEnum.SameUserNo);

            //1.4 参数校验-相同的手机号
            if (_userService.ExistSameUserPhone(userContext.AccId, request.UserPhone))
                return Fail(ErrorCodeEnum.SameUserPhone);

            request.PinYin = Helper.GetPinyin(request.UserName);
            request.PY = Helper.GetInitials(request.UserName);
            request.Sex = request.NickId > 2 ? 3 : request.NickId;
            request.OperatorId = userContext.UserId;
            request.UserLockRank = request.UserGrade != 1 ? 1 : 0;
            request.AccId = userContext.AccId;
            request.MasterId = userContext.MasterId;
            return FunctionReturn(_userService.AddUser(request));
        }

        /// <summary>
        /// 修改会员
        /// </summary>
        /// <returns></returns>
        [Route("{userId}")]
        [HttpPut, HttpOptions]
        public ResponseModel UpdateUser(int userId, UserHandle request)
        {
            //1.入参校验
            //1.1 参数为空
            ResponseModel checkResult;
            var userContext = GetUserContext();

            var result = CheckRequestParamIsNull(request, out checkResult);
            if (!result)
                return checkResult;

            request.Id = userId;

            //1.2 参数校验-参数不合法
            if (CheckModelParams(out checkResult))
                return checkResult;

            //1.3 参数校验-相同的卡号
            if (_userService.ExistSameUserNo(userContext.AccId, request.UserNo, request.Id))
                return Fail(ErrorCodeEnum.SameUserNo);

            //1.4 参数校验-相同的手机号
            if (_userService.ExistSameUserPhone(userContext.AccId, request.UserPhone, request.Id))
                return Fail(ErrorCodeEnum.SameUserPhone);

            request.PinYin = Helper.GetPinyin(request.UserName);
            request.PY = Helper.GetInitials(request.UserName);
            request.Sex = request.NickId > 2 ? 3 : request.NickId;
            request.OperatorId = userContext.UserId;
            request.UserLockRank = request.UserGrade != 1 ? 1 : 0;
            request.AccId = userContext.AccId;
            return FunctionReturn(_userService.UpdateUser(request));
        }

        /// <summary>
        /// 删除会员
        /// </summary>
        /// <returns></returns>
        [HttpDelete, HttpOptions]
        public ResponseModel DeleteUser()
        {
            return FunctionReturn(_userService.DeleteUser());
        }

        /// <summary>
        /// 获取最新会员编号
        /// </summary>
        /// <returns></returns>
        [Route("userno")]
        [HttpGet, HttpOptions]
        public ResponseModel GetUserNewNo()
        {
            return FunctionReturn(_userService.GetUserNewNo(GetUserContext().AccId));
        }

        /// <summary>
        /// 会员分组获取分组列表
        /// </summary>
        /// <returns></returns>
        [Route("data/group")]
        [HttpGet, HttpOptions]
        public ResponseModel GetUserGroups()
        {
            return FunctionReturn(_userService.GetUserGroups(GetUserContext().AccId));
        }

        /// <summary>
        /// 会员分组更新
        /// </summary>
        /// <returns></returns>
        [Route("data/group/{groupId}")]
        [HttpPut, HttpOptions]
        public ResponseModel UpdateUserGroup(int groupId, int[] userIds)
        {
            return FunctionReturn(_userService.UpdateUserGroup(userIds, groupId, GetUserContext().AccId));
        }

        /// <summary>
        /// 会员分组新增
        /// </summary>
        /// <returns></returns>
        [Route("data/group/{groupName}")]
        [HttpPost, HttpOptions]
        public ResponseModel InsertUserGroup(string groupName, int[] userIds)
        {
            ResponseModel checkResult;
            var userContext = GetUserContext();

            UserGroup request = new UserGroup();
            request.AccId = userContext.AccId;
            request.GroupAddTime = DateTime.Now;
            request.GroupName = groupName;

            //1.入参校验
            //1.1 参数为空
            var result = CheckRequestParamIsNull(request, out checkResult);
            if (!result)
                return checkResult;
            //1.2 参数校验-参数不合法
            if (CheckModelParams(out checkResult))
                return checkResult;

            //1.3 参数校验-相同的分组名称
            if (_userService.ExistSameUserGroupName(userContext.AccId, request.GroupName, request.GroupId))
                return Fail(ErrorCodeEnum.SameUserGroupName);

            return FunctionReturn(_userService.InsertUserGroup(request, userIds));
        }

        /// <summary>
        /// 获取会员详情
        /// </summary>
        /// <returns></returns>
        [Route("{userId}")]
        [HttpGet, HttpOptions]
        public ResponseModel GetUserDetail(int userId)
        {
            var data = _userService.GetUserDetail(GetUserContext(), userId);

            if (data == null)
                Fail(ErrorCodeEnum.UserFail);

            return Success(data);
        }

        /// <summary>
        /// 获取会员消费统计
        /// </summary>
        /// <returns></returns>
        [Route("{userId}/statistics")]
        [HttpGet, HttpOptions]
        public ResponseModel GetUserStatistics(int userId)
        {
            var data = _userService.GetUserStatistics(GetUserContext(), userId);

            if (data == null)
                Fail(ErrorCodeEnum.UserFail);

            return Success(data);
        }

        /// <summary>
        /// 更换会员头像
        /// </summary>
        /// <returns></returns>
        [Route("{userId}/avatar/{avatar}")]
        [HttpGet, HttpOptions]
        public ResponseModel ChangeUserAvatar(int userId, string avatar)
        {
            var data = _userService.ChangeUserAvatar(GetUserContext(), userId, avatar);

            if (!data)
                Fail(ErrorCodeEnum.UserFail);

            return Success(data);
        }

        /// <summary>
        /// 获取标签列表
        /// </summary>
        /// <returns></returns>
        [Route("data/tag")]
        [HttpGet, HttpOptions]
        public ResponseModel GetUserTags()
        {
            return FunctionReturn(_userService.GetTagList(GetUserContext().AccId));
        }

        /// <summary>
        /// 给会员打标签
        /// </summary>
        /// <returns></returns>
        [Route("data/tag")]
        [HttpPut, HttpOptions]
        public ResponseModel UpdateUserTag(UserTagBind request)
        {
            var userContext = GetUserContext();
            return
                FunctionReturn(_userService.UpdateUserTag(userContext.AccId, userContext.UserId, request.UserIds,
                    request.TagId));
        }

        /// <summary>
        /// 删除会员标签
        /// </summary>
        /// <returns></returns>
        [Route("data/tag/{userId}/{tagId}")]
        [HttpDelete, HttpOptions]
        public ResponseModel DeleteUserTag(int userId, int tagId)
        {
            var userContext = GetUserContext();
            return FunctionReturn(_userService.DeleteUserTag(userContext.AccId, userId, tagId));
        }

        /// <summary>
        /// 新增会员标签并给会员打标签
        /// </summary>
        /// <returns></returns>
        [Route("data/tag")]
        [HttpPost, HttpOptions]
        public ResponseModel InsertUserTag(UserTagAdd request)
        {
            ResponseModel checkResult;
            var userContext = GetUserContext();
            UserTagInfo entity = new UserTagInfo();
            entity.AccId = userContext.AccId;
            entity.TagColor = request.TagColor;
            entity.TagName = request.TagName;
            entity.InsertTime = DateTime.Now;
            entity.OperatorId = userContext.UserId;
            entity.TagPinYin = Helper.GetPinyin(entity.TagName);
            entity.TagPy = Helper.GetInitials(entity.TagName);
            entity.TagSerarch = entity.TagName + " | " + entity.TagPy + " | " + entity.TagPinYin;
            entity.TagType = 1;

            //1.1入参校验
            if (CheckModelParams(out checkResult))
                return checkResult;

            //1.2 参数校验-相同的分组名称
            if (_userService.ExistSameUserTagName(userContext.AccId, entity.TagName))
                return Fail(ErrorCodeEnum.SameUserNo);

            return FunctionReturn(_userService.InsertUserTag(entity, userContext.AccId, request.UserIds));
        }

        /// <summary>
        /// 获取店铺会员等级配置
        /// </summary>
        /// <returns></returns>
        [Route("data/grade")]
        [HttpGet, HttpOptions]
        public ResponseModel GetStoreRankConfig(UserInfoDetail userInfo)
        {
            return FunctionReturn(_basicDataService.GetStoreRankConfig(GetUserContext().AccId));
        }

        /// <summary>
        /// 获取店铺称谓
        /// </summary>
        /// <returns></returns>
        [Route("data/nick")]
        [HttpGet, HttpOptions]
        public ResponseModel GetUserNickList()
        {
            return FunctionReturn(_userService.GetUserNickList(GetUserContext().AccId));
        }

        /// <summary>
        /// 新增会员店铺称谓
        /// </summary>
        /// <returns></returns>
        [Route("data/nick/{nickname}")]
        [HttpPost, HttpOptions]
        public ResponseModel AddUserNick(string nickname)
        {
            UserNickName entity = new UserNickName();
            entity.accID = GetUserContext().AccId;
            entity.insertTime = DateTime.Now;
            entity.nickName = nickname;
            //参数校验-相同的分组名称
            if (_userService.ExistSameUserNick(entity.accID, entity.nickName))
                return Fail(ErrorCodeEnum.SameUserNo);
            return FunctionReturn(_userService.AddUserNick(entity));
        }

        /// <summary>
        /// 获取会员积分列表
        /// </summary>
        /// <returns></returns>
        [Route("{userId}/integral/{searchType}")]
        [HttpGet, HttpOptions]
        public ResponseModel GetUserIntegral(int userId, int searchType)
        {
            var data = _userService.GetUserIntegral(GetUserContext(), userId, (UserIntegralSearchTypeEnum) searchType);

            return Success(data);
        }

        /// <summary>
        /// 修改会员积分
        /// </summary>
        /// <returns></returns>
        [Route("{userId}/integral/change")]
        [HttpPut, HttpOptions]
        public ResponseModel ChangeUserIntegral(int userId,
            [FromBody] UserIntegral userIntegralDto)
        {
            var data = _userService.SetUserIntegral(UserIntegralSetTypeEnum.Change, GetUserContext(), userId,
                userIntegralDto.integral, userIntegralDto.remark);

            if (!data)
                Fail(ErrorCodeEnum.UserFail);

            return Success(data);
        }

        /// <summary>
        /// 会员积分兑换
        /// </summary>
        /// <returns></returns>
        [Route("{userId}/integral/exchange")]
        [HttpPut, HttpOptions]
        public ResponseModel ExChangeUserIntegral(int userId,
            [FromBody] UserIntegral userIntegralDto)
        {
            var data = _userService.SetUserIntegral(UserIntegralSetTypeEnum.Exchange, GetUserContext(), userId,
                userIntegralDto.integral, userIntegralDto.remark);

            if (!data)
                Fail(ErrorCodeEnum.UserFail);

            return Success(data);
        }

        /// <summary>
        /// 会员消费记录
        /// </summary>
        /// <returns></returns>
        [Route("{userId}/sales")]
        [HttpGet, HttpOptions]
        public ResponseModel GetUserSales(int userId, string keywords)
        {
            if (string.IsNullOrEmpty(keywords))
                keywords = "";

            var data = _userService.GetUserSales(GetUserContext(), userId, keywords);

            return Success(data);
        }

        /// <summary>
        /// 获取店员列表
        /// </summary>
        /// <param name="serarchParam"></param>
        /// <returns></returns>
        [Route("data/userlist")]
        [HttpGet, HttpOptions]
        public ResponseModel GetUserList([FromUri] UserListSearch serarchParam)
        {
            return FunctionReturn(_userService.GetUserList(serarchParam,GetUserContext().AccId));
        }

        /// <summary>
        /// 校验手机号是否被使用
        /// </summary>
        /// <param name="phoneno"></param>
        /// <returns></returns>
        [Route("data/checkphone/{phoneno}")]
        [HttpGet, HttpOptions]
        public ResponseModel CheckPhoneNo(string phoneno)
        {
            return FunctionReturn(_userService.CheckPhoneNo(phoneno, GetUserContext().AccId));
        }

        /// <summary>
        /// 获取店铺所有优惠券
        /// </summary>
        /// <returns></returns>
        [Route("data/coupon/accountall")]
        [HttpGet, HttpOptions]
        public ResponseModel GetAccountAllCouponInfo()
        {
            return FunctionReturn(_userService.GetAccountAllCouponInfo(GetUserContext().AccId));
        }

        /// <summary>
        /// 获取会员所有优惠券
        /// </summary>
        /// <returns></returns>
        [Route("data/coupon/usercoupon/{userid}")]
        [HttpGet, HttpOptions]
        public ResponseModel GetUserCouponAll(int userid)
        {
            var userContext = GetUserContext();
            return FunctionReturn(_userService.GetUserCouponAll(userContext.AccId,userid, userContext.MasterId));
        }

        /// <summary>
        /// 新增会员标签并给会员打标签
        /// </summary>
        /// <returns></returns>
        [Route("data/coupon")]
        [HttpPost, HttpOptions]
        public ResponseModel SendUserCoupon(UserCouponBind request)
        {
            return FunctionReturn(_userService.SendUserCoupon(request, GetUserContext()));
        }
    }
}