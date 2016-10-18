using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;
using Dapper;
using Script.I200.Application.Shared;
using Script.I200.Application.Users;
using Script.I200.Core.Config;
using Script.I200.Data;
using Script.I200.Data.MicroOrm.SqlGenerator;
using Script.I200.Entity.Api.UserStoreMoneyCard;
using Script.I200.Entity.API;
using Script.I200.Entity.Enum;
using Script.I200.Entity.Model.User;
using ResponseModel = Script.I200.Entity.API.ResponseModel;

namespace Script.I200.Application.UserStoreMoneyCard
{
    public class UserStoreMoneyCardService : IUserStoreMoneyCardService
    {
        private readonly ISharedService _shardService = new SharedService();
        private readonly DapperRepository<UserHandle> _useraddDapperRepository;
        private readonly DapperRepository<UserInfoDetail> _userInfoRepository;
        private readonly DapperRepository<UserLogInfo> _userLogRepository;
        private readonly IUserService _userService = new UserService();
        private readonly DapperRepository<UserStoreMoneySearchResultItem> _userStoreMoneyCardDapperRepository;

        private readonly DapperRepository<UserStoreTransactionRecordResultItem>
            _userStoreMoneyTransactionDapperRepository;

        private readonly DapperRepository<UserStoreTransactionRecordResult> _userStoreTransactionRecordSumRepository;

        public UserStoreMoneyCardService()
        {
            var dapperDbContext =
                new DapperDbContext(new SqlConnection(WebConfigSetting.Instance.I200DbConnectionString));
            _userInfoRepository = new DapperRepository<UserInfoDetail>(dapperDbContext);
            _userLogRepository = new DapperRepository<UserLogInfo>(dapperDbContext);
            _userStoreMoneyTransactionDapperRepository =
                new DapperRepository<UserStoreTransactionRecordResultItem>(dapperDbContext);
            _userStoreTransactionRecordSumRepository =
                new DapperRepository<UserStoreTransactionRecordResult>(dapperDbContext);
            _userStoreMoneyCardDapperRepository = new DapperRepository<UserStoreMoneySearchResultItem>(dapperDbContext);
            _useraddDapperRepository = new DapperRepository<UserHandle>(dapperDbContext);
        }

        /// <summary>
        ///     会员充值
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="userStoreParam"></param>
        /// <returns></returns>
        public ResponseModel UserStoreMoney(UserContext userContext, UserStoreMoneyAdd userStoreParam)
        {
            var tranFlag = false;
            var userInfo = new UserInfoDetail();
            if (userStoreParam.UserId > 0)
            {
                userInfo = _userInfoRepository.Find(x => x.Uid == userStoreParam.UserId && x.AccId == userContext.AccId);
            }

            var conn = _useraddDapperRepository.Connection;
            conn.Open();
            var trans = conn.BeginTransaction();
            try
            {
                //1.先获取充值会员的个人信息（储值余额、计次卡、上次购买时间等）
                if (userStoreParam.UserId == 0)
                {
                    //1.1 新会员储值，先增加会员，然后充值
                    if (userStoreParam.UserId == 0)
                    {
                        var userNo = _userService.GetUserNewNo(userContext.AccId).Data.ToString();

                        //1.2 新增会员，储值
                        var userHandel = new UserHandle
                        {
                            Id = userStoreParam.UserId,
                            UserNo = userNo,
                            UserPhone = userStoreParam.Phone,
                            UserName = userStoreParam.UserName,
                            UserStoreMoney = userStoreParam.RechargeMoney,
                            PinYin = Helper.GetPinyin(userStoreParam.UserName),
                            PY = Helper.GetInitials(userStoreParam.UserName),
                            OperatorId = userContext.UserId,
                            AccId = userContext.AccId
                        };
                        var userInsertResult = _useraddDapperRepository.Insert(userHandel, trans);
                        if (userInsertResult)
                        {
                            //1.3 记录日志
                            var storeLogInfo = new UserLogInfo
                            {
                                AccId = userContext.AccId,
                                OriginalAccId = userContext.AccId,
                                UId = userHandel.Id,
                                LogType = (int) UserLogTypeEnum.StoreChange,
                                ItemType = (int) UserLogItemTypeEnum.Shopping,
                                OriginalVal = 0,
                                EditMoney = userStoreParam.RealMoney,
                                EditVal = userStoreParam.RechargeMoney,
                                FinalVal = userStoreParam.RechargeMoney,
                                LogTime = DateTime.Now,
                                OperatorTime = DateTime.Now,
                                OperatorId = userContext.UserId,
                                OperatorIp = userContext.IpAddress,
                                Remark = userStoreParam.Remark,
                                Flag = string.Empty,
                                FlagStatus = 0,
                                FlagStatusTime = DateTime.Now,
                                EditMoneyType = userStoreParam.PayType,
                                AddedLgUserId = userStoreParam.Salesman,
                                BindCardId = 0
                            };
                            if (_userLogRepository.Insert(storeLogInfo, trans))
                            {
                                tranFlag = true;
                            }
                        }
                    }
                }
                else
                {
                    //2.储值并记录日志
                    tranFlag = LoggingStoreMoneyLog(userContext, userStoreParam, userInfo, trans);
                }
            }
            catch (Exception)
            {
                trans.Rollback();
                tranFlag = false;
                throw;
            }
            finally
            {
                if (tranFlag)
                {
                    trans.Commit();
                    //3.发送提醒短信
                    if (userStoreParam.SendMsg)
                    {
                        Task.Run(() => { _shardService.SendSms(userContext); });
                    }
                }
                else
                {
                    trans.Rollback();
                }
                trans.Dispose();
                conn.Close();
            }
            //4.返回数据
            return new ResponseModel
            {
                Code = tranFlag ? (int) ErrorCodeEnum.Success : (int) ErrorCodeEnum.StoreMoneyFailed,
                Data = userStoreParam
            };
        }

        /// <summary>
        ///     获取会员储值卡列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="searchParam"></param>
        /// <returns></returns>
        public ResponseModel GetUserStoreCardsList(UserContext userContext, UserStoreCardSearchParam searchParam)
        {
            if (searchParam.CurrentPage == null || searchParam.CurrentPage < 1)
                searchParam.CurrentPage = 1;
            if (searchParam.PageSize == null || searchParam.PageSize < 1)
                searchParam.PageSize = 25;

            //1.初始化查询条件
            var userStoreMoneySearchResult = new UserStoreMoneySearchResult();
            var strSql = new StringBuilder();
            var sqlWhereBuilder = new StringBuilder();
            var logStrSql = new StringBuilder();
            var tRowBuilder = new StringBuilder();
            sqlWhereBuilder.Append(" WHERE T_UserInfo.AccId= @AccId ");
            logStrSql.Append("where T_User_LogInfo.accId=@accId ");
            if (searchParam.UserId > 0)
            {
                sqlWhereBuilder.Append(" and  T_UserInfo.uid =@UserId ");
                logStrSql.Append("and T_User_LogInfo.uid=@UserId ");
            }

            //排序
            var rankName = searchParam.Rank;
            var sort = searchParam.Sort;
            if (rankName != "userName")
            {
                rankName = "uid";
            }

            if (string.IsNullOrWhiteSpace(sort) || (sort.ToLower() != "desc" && sort.ToLower() != "asc"))
            {
                sort = "Desc";
            }

            //2.筛选项
            tRowBuilder.Append("   SELECT ROW_NUMBER() OVER ( ");
            tRowBuilder.Append(" ORDER BY " + rankName + "   " + sort + " ");
            tRowBuilder.Append("       ) AS rownumber ");
            tRowBuilder.Append(
                ",uid as UserId ,uName as UserName,uPhone as Phone,accId,uStoreMoney as Balance ");
            tRowBuilder.Append("   FROM T_UserInfo  ");
            tRowBuilder.Append(sqlWhereBuilder);


            //3.分页查询
            strSql.Append(" SELECT *");
            strSql.Append(" FROM (");
            strSql.Append(tRowBuilder);
            strSql.Append("   ) AS T");
            strSql.Append(" WHERE RowNumber BETWEEN (@PageIndex-1)*@PageSize+1  ");
            strSql.Append("     AND @PageSize*@PageIndex ;");


            //4.统计卡内余额、总共会员
            strSql.Append(" SELECT @TotalBalance= ISNULL(SUM(uStoreMoney),0)" +
                          ",@TotalNum=Count(1)" +
                          ",@TotalUsers=COUNT(DISTINCT uid) from  T_UserInfo  ");
            strSql.Append(sqlWhereBuilder);

            //5.统计总充值金额
            strSql.Append(" select @TotalStoreMoney=sum(EditVal) from  T_User_LogInfo ");
            strSql.Append(logStrSql);


            var sqlParams = new
            {
                userContext.AccId,
                searchParam.UserId,
                PageSize = searchParam.PageSize ?? 25,
                PageIndex = searchParam.CurrentPage ?? 1
            };
            var dapperParam = new DynamicParameters(sqlParams);
            dapperParam.Add("TotalUsers", dbType: DbType.Int32, direction: ParameterDirection.Output);
            dapperParam.Add("TotalStoreMoney", dbType: DbType.Decimal, direction: ParameterDirection.Output);
            dapperParam.Add("TotalBalance", dbType: DbType.Decimal, direction: ParameterDirection.Output);
            dapperParam.Add("TotalNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var sqlQuery = new SqlQuery(strSql.ToString(), dapperParam);

            var itemResult = _userStoreMoneyCardDapperRepository.FindAll(sqlQuery);
            var storeMoneySearchResultItems = itemResult as UserStoreMoneySearchResultItem[] ?? itemResult.ToArray();
            if (itemResult != null && storeMoneySearchResultItems.Any())
            {
                foreach (var oItem in storeMoneySearchResultItems)
                {
                    var item = oItem;
                    var sum =
                        _userLogRepository.FindAll(x => x.AccId == userContext.AccId && x.UId == item.UserId)
                            .Sum(x => x.EditVal);
                    if (sum != null)
                        oItem.TotalStoreMoney = (decimal) sum;
                }
            }

            //6.赋值查询结果
            userStoreMoneySearchResult.Items = storeMoneySearchResultItems;
            userStoreMoneySearchResult.TotalUsers = dapperParam.Get<int?>("TotalUsers") ?? 0;
            userStoreMoneySearchResult.TotalBalance = dapperParam.Get<decimal?>("TotalBalance") ?? 0;
            userStoreMoneySearchResult.TotalStoreMoney = dapperParam.Get<decimal?>("TotalStoreMoney") ?? 0;
            userStoreMoneySearchResult.CurrentPage = searchParam.CurrentPage ?? 1;
            userStoreMoneySearchResult.PageSize = searchParam.PageSize ?? 25;
            userStoreMoneySearchResult.TotalSize = dapperParam.Get<int?>("TotalNum") ?? 0;
            userStoreMoneySearchResult.TotalPage =
                Convert.ToInt32(
                    Math.Ceiling(Convert.ToDecimal(userStoreMoneySearchResult.TotalSize)/
                                 Convert.ToDecimal(userStoreMoneySearchResult.PageSize)));
            //7.返回查询数据
            return new ResponseModel
            {
                Code = (int) ErrorCodeEnum.Success,
                Data = userStoreMoneySearchResult
            };
        }

        /// <summary>
        ///     获取储值历史记录
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="searchParam"></param>
        /// <returns></returns>
        public ResponseModel GetStoreMoneyHistoryList(UserContext userContext, UserStoreMoneySearchParam searchParam)
        {
            if (searchParam.CurrentPage == null || searchParam.CurrentPage < 1)
                searchParam.CurrentPage = 1;
            if (searchParam.PageSize == null || searchParam.PageSize < 1)
                searchParam.PageSize = 25;

            //1.初始化查询条件
            var userStoreTransactionRecordResult = new UserStoreTransactionRecordResult();
            var strSql = new StringBuilder();
            var sqlWhereBuilder = new StringBuilder();
            var tRowBuilder = new StringBuilder();

            sqlWhereBuilder.Append(" WHERE T_User_LogInfo.AccId= @AccId  ");
            //日期过滤
            if (searchParam.StartDate != null && searchParam.EndDate != null &&
                searchParam.StartDate <= searchParam.EndDate)
            {
                var newEndDate = (DateTime) searchParam.EndDate;
                searchParam.EndDate = newEndDate.AddDays(1).AddSeconds(-1);
                sqlWhereBuilder.Append(
                    " and  T_User_LogInfo.LogTime >=@StartDate  and T_User_LogInfo.LogTime <=@EndDate ");
            }

            if (searchParam.UserId != 0)
            {
                sqlWhereBuilder.Append(" and  T_User_LogInfo.uid =@UserId ");
            }

            //记录类型过滤（1: 储值消费 2：充值）
            sqlWhereBuilder.Append(
                searchParam.OperateType != null
                    ? " and  T_User_LogInfo.logType =@LogType  and T_User_LogInfo.itemType =@ItemType "
                    : "   and  (( T_User_LogInfo.logType =1  and T_User_LogInfo.itemType =1) or (T_User_LogInfo.logType =1  and T_User_LogInfo.itemType =2 ))  ");

            //2.筛选项
            tRowBuilder.Append("   SELECT ROW_NUMBER() OVER ( ");
            tRowBuilder.Append("       ORDER BY T_User_LogInfo.userLogID DESC ");
            tRowBuilder.Append("       ) AS rownumber ");
            tRowBuilder.Append(
                ",LogTime as createdAt,T_UserInfo.uid AS userId,T_UserInfo.uName AS userName, logType,itemType,operatorID, dbo.T_Account_User.name AS Salesman,remark,editVal as editMoney,FinalVal as StoreMoney ");
            tRowBuilder.Append("   FROM T_User_LogInfo  ");
            tRowBuilder.Append("  LEFT JOIN T_UserInfo ON T_User_LogInfo.uid = T_UserInfo.uid ");
            tRowBuilder.Append("  LEFT JOIN  dbo.T_Account_User ON  operatorID= dbo.T_Account_User.id  ");
            tRowBuilder.Append(sqlWhereBuilder);


            //3.分页查询
            strSql.Append(
                " SELECT *,(case when logType=1 and itemType=1  then 1  when logType=1 and itemType=2 then 2 else null end ) as OperateType ");
            strSql.Append(" FROM (");
            strSql.Append(tRowBuilder);
            strSql.Append("   ) AS T");
            strSql.Append(" WHERE RowNumber BETWEEN (@PageIndex-1)*@PageSize+1  ");
            strSql.Append("     AND @PageSize*@PageIndex ;");

            strSql.Append(" SELECT @TotalNum=COUNT(1) from ( ");
            strSql.Append(tRowBuilder);
            strSql.Append("   ) AS T");


            var sqlParams = new
            {
                userContext.AccId,
                searchParam.UserId,
                LogType = (int) UserLogTypeEnum.StoreChange,
                ItemType =
                    searchParam.OperateType == 1
                        ? (int) UserLogItemTypeEnum.Shopping
                        : (int) UserLogItemTypeEnum.ExchangeIntegral,
                searchParam.StartDate,
                searchParam.EndDate,
                PageSize = searchParam.PageSize ?? 25,
                PageIndex = searchParam.CurrentPage ?? 1
            };
            var dapperParam = new DynamicParameters(sqlParams);
            dapperParam.Add("TotalNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var sqlQuery = new SqlQuery(strSql.ToString(), dapperParam);

            //5.赋值查询结果
            var itemResult = _userStoreMoneyTransactionDapperRepository.FindAll(sqlQuery).ToList();
            var summaryResult = GetUserStoreMoneyTotalData(userContext, searchParam);
            foreach (var item in itemResult)
            {
                item.OperateTypeText =
                    (item.OperateType == 1 ? 1 : 2).ToEnumDescriptionString(typeof (StoreMoneyOperateEnum));
            }
            userStoreTransactionRecordResult.Items = itemResult;
            userStoreTransactionRecordResult.CurrentPage = searchParam.CurrentPage ?? 1;
            userStoreTransactionRecordResult.PageSize = searchParam.PageSize ?? 25;
            userStoreTransactionRecordResult.TotalSize = dapperParam.Get<int?>("TotalNum") ?? 0;
            userStoreTransactionRecordResult.TotalPage =
                Convert.ToInt32(
                    Math.Ceiling(Convert.ToDecimal(userStoreTransactionRecordResult.TotalSize)/
                                 Convert.ToDecimal(userStoreTransactionRecordResult.PageSize)));
            if (summaryResult != null)
            {
                userStoreTransactionRecordResult.TotalBalance = summaryResult.TotalBalance;
                userStoreTransactionRecordResult.TotalShopping = summaryResult.TotalShopping;
                userStoreTransactionRecordResult.TotalStoreMoney = summaryResult.TotalStoreMoney;
                userStoreTransactionRecordResult.TotalUserCount = summaryResult.TotalUserCount;
            }

            //6.返回查询数据
            return new ResponseModel
            {
                Code = (int) ErrorCodeEnum.Success,
                Data = userStoreTransactionRecordResult
            };
        }

        /// <summary>
        ///     获取储值状态列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public ResponseModel GetBussinessStatus(UserContext userContext)
        {
            var responseModel = new ResponseModel();
            var listStatus = new Dictionary<string, Dictionary<string, string>>();
            //储值操作类型（1.充值 2.消费）
            var storeMoneyOperateStatus = new Dictionary<string, string>
            {
                {"1", "充值"},
                {"2", "消费"}
            };
            //计次卡状态
            var timesCardStatus = new Dictionary<string, string>
            {
                {"1", "正常"},
                {"2", "已过期"}
            };
            //储值计次支付方式
            var storeMoneyPayType = new Dictionary<string, string>
            {
                {"1", "现金"},
                {"2", "银行卡支付"},
                {"3", "微信支付"},
                {"4", "支付宝支付"}
            };
            //充次操作类型
            var timesCardOperateStatus = new Dictionary<string, string>
            {
                {"1", "充次"},
                {"2", "消费"}
            };

            listStatus.Add("storeMoneyOperateStatus", storeMoneyOperateStatus);
            listStatus.Add("timesCardStatus", timesCardStatus);
            listStatus.Add("storeMoneyPayType", storeMoneyPayType);
            listStatus.Add("timesCardOperateStatus", timesCardOperateStatus);
            responseModel.Data = listStatus;
            return responseModel;
        }

        /// <summary>
        ///     更新会员表储值金额并记录储值日志
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="userStoreParam"></param>
        /// <param name="userInfo"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        private bool LoggingStoreMoneyLog(UserContext userContext, UserStoreMoneyAdd userStoreParam,
            UserInfoDetail userInfo, IDbTransaction transaction)
        {
            var logInfoResult = true;
            var updateRequest = new UserInfoDetail
            {
                Uid = userStoreParam.UserId,
                UStoreMoney = userInfo.UStoreMoney + userStoreParam.RechargeMoney
            };
            var storeResult = _userInfoRepository.Update<UserInfoDetail>(updateRequest, item => new {item.UStoreMoney},
                transaction);
            if (storeResult)
            {
                var storeLogInfo = new UserLogInfo
                {
                    AccId = userContext.AccId,
                    OriginalAccId = userContext.AccId,
                    UId = userStoreParam.UserId,
                    LogType = (int) UserLogTypeEnum.StoreChange,
                    ItemType = (int) UserLogItemTypeEnum.Shopping,
                    OriginalVal = userInfo.UStoreMoney,
                    EditMoney = userStoreParam.RealMoney,
                    EditVal = userStoreParam.RechargeMoney,
                    FinalVal = userInfo.UStoreMoney + userStoreParam.RechargeMoney,
                    LogTime = DateTime.Now,
                    OperatorTime = DateTime.Now,
                    OperatorId = userContext.UserId,
                    OperatorIp = userContext.IpAddress,
                    Remark = userStoreParam.Remark,
                    Flag = string.Empty,
                    FlagStatus = 0,
                    FlagStatusTime = DateTime.Now,
                    EditMoneyType = userStoreParam.PayType,
                    AddedLgUserId = userStoreParam.Salesman,
                    BindCardId = 0
                };

                //记录日志
                logInfoResult = _userLogRepository.Insert(storeLogInfo, transaction);
            }
            return logInfoResult;
        }

        /// <summary>
        ///     获取会员储值总计数据（总共会员数、总储值、总消费、总余额）
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="searchParam"></param>
        /// <returns></returns>
        public UserStoreTransactionRecordResult GetUserStoreMoneyTotalData(UserContext userContext,
            UserStoreMoneySearchParam searchParam)
        {
            var totalStoreMoneyStrSql = new StringBuilder();
            var totalShoppingStrSql = new StringBuilder();
            var totalBalanceStrSql = new StringBuilder();
            var totalUsersStrSql = new StringBuilder();
            var strSql = new StringBuilder();

            //统计总金额
            totalStoreMoneyStrSql.Append(
                " SELECT @TotalStoreMoney= ISNULL(SUM(T_User_LogInfo.EditVal),0) from T_User_LogInfo where accId=@AccId ");
            totalShoppingStrSql.Append(
                " SELECT @TotalShopping= ISNULL(SUM(T_User_LogInfo.EditVal),0) from T_User_LogInfo where accId=@AccId  ");

            totalBalanceStrSql.Append(
                " SELECT @TotalBalance= ISNULL(SUM(T_User_LogInfo.FinalVal),0)  from T_User_LogInfo where accId=@AccId ");
            totalUsersStrSql.Append(
                " SELECT @TotalUserCount= count(DISTINCT uid) from T_User_LogInfo where accId=@AccId ");
            var userIdStr = "  and uid=@UserId   ";
            if (searchParam.UserId != 0)
            {
                totalStoreMoneyStrSql.Append(userIdStr);
                totalShoppingStrSql.Append(userIdStr);
                totalBalanceStrSql.Append(userIdStr);
                totalUsersStrSql.Append(userIdStr);
            }

            var searchDateStr = "  and  T_User_LogInfo.LogTime >=@StartDate  and T_User_LogInfo.LogTime <=@EndDate ";
            if (searchParam.StartDate != null && searchParam.EndDate != null &&
                searchParam.StartDate <= searchParam.EndDate)
            {
                var newEndDate = (DateTime) searchParam.EndDate;
                searchParam.EndDate = newEndDate.AddDays(1).AddSeconds(-1);
                totalStoreMoneyStrSql.Append(searchDateStr);
                totalShoppingStrSql.Append(searchDateStr);
                totalBalanceStrSql.Append(searchDateStr);
                totalUsersStrSql.Append(searchDateStr);
            }

            //记录类型过滤（1: 储值消费 2：充值）
            var operateTypeStr = " and  T_User_LogInfo.logType =@LogType  and T_User_LogInfo.itemType =@ItemType  ";
            if (searchParam.OperateType != null)
            {
                totalStoreMoneyStrSql.Append(operateTypeStr);
                totalShoppingStrSql.Append(operateTypeStr);
                totalBalanceStrSql.Append(operateTypeStr);
                totalUsersStrSql.Append(operateTypeStr);
            }
            else
            {
                var operateRechargeType =
                    "   and  (T_User_LogInfo.logType =1  and T_User_LogInfo.itemType =1 )  ;   ";

                var operateShoppingType = "  and  (T_User_LogInfo.logType =1  and T_User_LogInfo.itemType =2) ;  ";

                var operateTypeStrElse =
                    "  and  ((T_User_LogInfo.logType =1  and T_User_LogInfo.itemType =1 ) or (T_User_LogInfo.logType =1  and T_User_LogInfo.itemType =2))  ;";

                totalStoreMoneyStrSql.Append(operateRechargeType);
                totalShoppingStrSql.Append(operateShoppingType);
                totalBalanceStrSql.Append(operateTypeStrElse);
                totalUsersStrSql.Append(operateTypeStrElse);
            }

            var sqlParams = new
            {
                userContext.AccId,
                searchParam.UserId,
                LogType = (int) UserLogTypeEnum.StoreChange,
                ItemType =
                    searchParam.OperateType == 1
                        ? (int) UserLogItemTypeEnum.Shopping
                        : (int) UserLogItemTypeEnum.ExchangeIntegral,
                searchParam.StartDate,
                searchParam.EndDate,
                searchParam.PageSize,
                PageIndex = searchParam.CurrentPage
            };
            var dapperParam = new DynamicParameters(sqlParams);
            dapperParam.Add("TotalStoreMoney", dbType: DbType.Decimal, direction: ParameterDirection.Output);
            dapperParam.Add("TotalShopping", dbType: DbType.Decimal, direction: ParameterDirection.Output);
            dapperParam.Add("TotalBalance", dbType: DbType.Decimal, direction: ParameterDirection.Output);
            dapperParam.Add("TotalUserCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            strSql.Append(totalStoreMoneyStrSql)
                .Append(totalBalanceStrSql)
                .Append(totalShoppingStrSql)
                .Append(totalUsersStrSql);

            var sqlQuery = new SqlQuery(strSql.ToString(), dapperParam);
            var result = _userStoreTransactionRecordSumRepository.FindAll(sqlQuery);

            //汇总结果集
            decimal totalStoreMoney = 0;
            decimal totalShopping = 0;
            var totalUserCount = dapperParam.Get<int?>("TotalUserCount") ?? 0;
            var totalBalance = dapperParam.Get<decimal?>("TotalBalance") ?? 0;
            switch (searchParam.OperateType)
            {
                case 1:
                    totalStoreMoney = dapperParam.Get<decimal?>("TotalStoreMoney") ?? 0;
                    totalShopping = 0;
                    break;
                case 2:
                    totalStoreMoney = 0;
                    totalShopping = dapperParam.Get<decimal?>("TotalShopping") ?? 0;
                    break;
                default:
                    totalStoreMoney = dapperParam.Get<decimal?>("TotalStoreMoney") ?? 0;
                    totalShopping = dapperParam.Get<decimal?>("TotalShopping") ?? 0;
                    break;
            }
            return new UserStoreTransactionRecordResult
            {
                TotalUserCount = totalUserCount,
                TotalBalance = totalBalance,
                TotalShopping = totalShopping,
                TotalStoreMoney = totalStoreMoney
            };
        }
    }
}