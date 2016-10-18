using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Script.I200.Core.Config;
using Script.I200.Data;
using Script.I200.Data.MicroOrm.SqlGenerator;
using Script.I200.Entity.Api.Feedback;
using Script.I200.Entity.API;
using Script.I200.Entity.Enum;
using Script.I200.Entity.Model.Feedback;

namespace Script.I200.Application.Feedback
{
    /// <summary>
    ///     用户反馈（B端用户）
    /// </summary>
    /// ·
    public class FeedbackService : IFeedbackService
    {
        private readonly DapperRepository<string> _checkSysTaskDailyDapperRepository;
        private readonly DapperRepository<SysAccount> _sysAccountDapperRepository;
        private readonly DapperRepository<SysTaskDaily> _sysTaskDailyDapperRepository;

        public FeedbackService()
        {
            var dapperContext =
                new DapperDbContext(new SqlConnection(WebConfigSetting.Instance.I200SysDbConnectionString));
            _checkSysTaskDailyDapperRepository = new DapperRepository<string>(dapperContext);
            _sysTaskDailyDapperRepository = new DapperRepository<SysTaskDaily>(dapperContext);
            _sysAccountDapperRepository = new DapperRepository<SysAccount>(dapperContext);
        }

        /// <summary>
        ///     提交反馈
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="feedbackParams"></param>
        /// <returns></returns>
        public ResponseModel CommmitFeedback(UserContext userContext, FeedbackRequestParams feedbackParams)
        {
            //1.判断反馈是否低于10分钟，低于10分钟则不允许提交
            var checkLessThanTenMinutesResult = CheckCommitFeedbackIsLessThanTenMinutes(userContext);
            if (checkLessThanTenMinutesResult)
            {
                return new ResponseModel
                {
                    Code = (int) ErrorCodeEnum.FeedbackLessThanTenMinutes
                };
            }

            //2.插入反馈记录
            SysTaskDaily insertFeedbackModel;
            var iResult = InsertFeedbacks(userContext, feedbackParams, out insertFeedbackModel);

            //3.后台系统统计信息
            if (!iResult)
            {
                return new ResponseModel
                {
                    Code = (int) ErrorCodeEnum.FeedbackFail
                };
            }

            //3.1异常处理后台统计消息
            if (!string.IsNullOrEmpty(feedbackParams.FeedbackTelPhone))
            {
                Task.Run(() => { UpdateSysAccountInfo(userContext, feedbackParams); });
            }

            return new ResponseModel
            {
                Code = (int) ErrorCodeEnum.Success,
                Data = insertFeedbackModel
            };
        }

        /// <summary>
        ///     校验提交反馈是否低于分钟，低于十分钟则不允许重复提交
        /// </summary>
        /// <returns></returns>
        public bool CheckCommitFeedbackIsLessThanTenMinutes(UserContext userContext)
        {
            //当前店铺的Id
            var strSql = new StringBuilder();
            strSql.Append("if exists(select Id from Sys_TaskDaily where accountid=@accountId)  ");
            strSql.Append("begin   ");
            strSql.Append("SELECT TOP 1 DATEDIFF(MINUTE,inertTime,GETDATE()) as minutes FROM [Sys_TaskDaily]  ");
            strSql.Append("where accountid=@accountId order by inertTime desc    ");
            strSql.Append("end  ");
            strSql.Append("ELSE   ");
            strSql.Append("begin    ");
            strSql.Append("SELECT 10  ");
            strSql.Append("end ");
            var sqlParams = new
            {
                accountId = userContext.AccId
            };
            var dapperParam = new DynamicParameters(sqlParams);
            var sqlQuery = new SqlQuery(strSql.ToString(), dapperParam);
            var result = _checkSysTaskDailyDapperRepository.FindAll(sqlQuery);
            if (result != null) return Convert.ToInt32(result.FirstOrDefault()) < 10;
            return true;
        }

        /// <summary>
        ///     插入反馈记录
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="feedbackRequestParams"></param>
        /// <param name="sysTaskDaily"></param>
        /// <returns></returns>
        public bool InsertFeedbacks(UserContext userContext, FeedbackRequestParams feedbackRequestParams,
            out SysTaskDaily sysTaskDaily)
        {
            //1.获取反馈备注
            var remark = GetFeedbackRemark((FeedbackTypeEnum) feedbackRequestParams.FeedbackType);
            sysTaskDaily = new SysTaskDaily
            {
                Accountid = userContext.AccId,
                Content = feedbackRequestParams.Suggestion,
                OperateTime = DateTime.Now.AddHours(2),
                Level = 9,
                Status = 0,
                InsertTime = DateTime.Now,
                InsertName = userContext.OperaterName,
                LoginUid = userContext.UserId,
                Remark = remark,
                Source = remark
            };

            //2.插入反馈记录
            return _sysTaskDailyDapperRepository.Insert(sysTaskDaily);
        }

        /// <summary>
        ///     更新后台系统统计信息
        /// </summary>
        /// <returns></returns>
        public bool UpdateSysAccountInfo(UserContext userContext, FeedbackRequestParams feedbackRequestParams)
        {
            var columnName = string.Empty;
            switch ((FeedbackTypeEnum) feedbackRequestParams.FeedbackType)
            {
                case FeedbackTypeEnum.TelephoneFeedback:
                    columnName = "feedbackTel";
                    break;
                case FeedbackTypeEnum.QqFeedback:
                    columnName = "feedbackQQ";
                    break;
            }

            var strSql = new StringBuilder();
            strSql.Append(" if(EXISTS(select * from Sys_Account where accid=@accountId)) ");
            strSql.Append(" begin ");
            strSql.Append(" update Sys_Account ");
            strSql.Append(" set " + columnName + "= @updateValue ");
            strSql.Append("  where accid=@accountId;");
            strSql.Append("  end ");
            strSql.Append("  else ");
            strSql.Append(" begin ");
            strSql.Append("  insert into Sys_Account (accid," + columnName + ") ");
            strSql.Append("  values(@accountId,@updateValue); ");
            strSql.Append(" end ");

            var sqlParams = new
            {
                accountId = userContext.AccId,
                updateValue =  feedbackRequestParams.FeedbackTelPhone
            };
            var dapperParam = new DynamicParameters(sqlParams);
            var sqlQuery = new SqlQuery(strSql.ToString(), dapperParam);
            var insertResult = _sysAccountDapperRepository.FindAll(sqlQuery);
            return insertResult != null && insertResult.Any();
        }

        /// <summary>
        ///     获取反馈的备注
        /// </summary>
        /// <param name="feedbackType"></param>
        /// <returns></returns>
        public string GetFeedbackRemark(FeedbackTypeEnum feedbackType)
        {
            string remark;
            switch (feedbackType)
            {
                case FeedbackTypeEnum.ShoppingMallFeedback:
                    remark = "商城反馈";
                    break;
                case FeedbackTypeEnum.CashWithdrawalFeedback:
                    remark = "提现申请";
                    break;
                case FeedbackTypeEnum.NewGoodsServiceFeedback:
                    remark = "新增商品服务评论";
                    break;
                case FeedbackTypeEnum.PackageOf599SassServiceFeedback:
                    remark = "新增599套餐购买";
                    break;
                case FeedbackTypeEnum.AndroidShoppingFeedback:
                    remark = "新增安卓端购买";
                    break;
                default:
                    remark = "前台反馈";
                    break;
            }
            return remark;
        }
    }
}