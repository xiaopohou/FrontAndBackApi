using System.Data.SqlClient;
using CommonLib;
using Script.I200.Core;
using Script.I200.Core.Config;
using Script.I200.Data;
using Script.I200.Entity.API;
using Script.I200.Entity.Enum;
using Script.I200.Entity.Model.Tips;
using ResponseModel = Script.I200.Entity.API.ResponseModel;

namespace Script.I200.Application.Tips
{
    /// <summary>
    ///     提示
    /// </summary>
    public class TipsService : ITipsService
    {
        private readonly DapperRepository<UserBehaviorModel> _userBehaviorDapperRepository;

        /// <summary>
        ///     初始化
        /// </summary>
        public TipsService()
        {
            var dapperContext = new DapperDbContext(new SqlConnection(WebConfigSetting.Instance.I200DbConnectionString));
            _userBehaviorDapperRepository = new DapperRepository<UserBehaviorModel>(dapperContext);
        }

        /// <summary>
        ///     判断是否显示消息提示层
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="tipsType"></param>
        /// <returns></returns>
        public ResponseModel IsHideElement(UserContext userContext, int tipsType)
        {
            var userBehaviorModel = GetUserBehaviorResult(userContext, tipsType);
            return new ResponseModel
            {
                Code = (int) ErrorCodeEnum.Success,
                Data = userBehaviorModel!=null
            };
        }

        /// <summary>
        ///     更新消息提示层
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="tipsType"></param>
        /// <returns></returns>
        public ResponseModel AddOrUpdateUserBehavior(UserContext userContext, int tipsType)
        {
            var modelConfig = ((UserBehaviorNameEnum) tipsType).GetConfig();
            if (modelConfig==null)
            {
                throw  new YuanbeiException("获取配置信息错误");
            }
            var searchResult = GetUserBehaviorResult(userContext, tipsType);
            if (searchResult!=null)
            {
                searchResult.TotalCount = searchResult.TotalCount + 1;
                var updateResult = UpdateUserBehavior(searchResult);
                return new ResponseModel
                {
                    Code = updateResult?(int)ErrorCodeEnum.Success:(int)ErrorCodeEnum.Failed,
                    Data = searchResult
                };
            }

            var model = new UserBehaviorModel
            {
                AccId = userContext.AccId,
                AccUserId = userContext.UserId,
                TotalCount = 1,
                ElementId = tipsType,
                ElementName = (tipsType).ToEnumDescriptionString((typeof (UserBehaviorNameEnum))),
                ConfigCount = modelConfig.ConfigCount,
                EventType = (int) modelConfig.EventType
            };

            var result = _userBehaviorDapperRepository.Insert(model);
            return new ResponseModel
            {
                Code = result ? (int) ErrorCodeEnum.Success : (int) ErrorCodeEnum.Failed,
                Data = model
            };
        }

        /// <summary>
        /// 　获取用户消息处理行为是否存在（是否点击、是否查看）
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="tipsType"></param>
        /// <returns></returns>
        private UserBehaviorModel GetUserBehaviorResult(UserContext userContext, int tipsType)
        {
            var modelConfig = ((UserBehaviorNameEnum) tipsType).GetConfig();
            if (modelConfig == null) return null;
            var eventType = (int) modelConfig.EventType;
            var userBehaviorResult =
                _userBehaviorDapperRepository.Find(
                    x =>
                        x.AccId == userContext.AccId && x.AccUserId == userContext.UserId && x.ElementId == tipsType &&
                        x.EventType == eventType);
            return userBehaviorResult;
        }

        /// <summary>
        ///     更新点击消息的次数
        /// </summary>
        /// <param name="userBehaviorModel"></param>
        /// <returns></returns>
        public bool UpdateUserBehavior(UserBehaviorModel userBehaviorModel)
        {
            return _userBehaviorDapperRepository.Update(userBehaviorModel);
        }
    }
}