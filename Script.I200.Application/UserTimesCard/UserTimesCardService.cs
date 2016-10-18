using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;
using Dapper;
using Script.I200.Application.BasicData;
using Script.I200.Application.Goods;
using Script.I200.Application.Shared;
using Script.I200.Application.Users;
using Script.I200.Core.Config;
using Script.I200.Data;
using Script.I200.Data.MicroOrm.SqlGenerator;
using Script.I200.Entity.Api.UserTimesCard;
using Script.I200.Entity.API;
using Script.I200.Entity.Dto.User;
using Script.I200.Entity.Enum;
using Script.I200.Entity.Model.Goods;
using Script.I200.Entity.Model.TimesCard;
using Script.I200.Entity.Model.User;
using ResponseModel = Script.I200.Entity.API.ResponseModel;

namespace Script.I200.Application.UserTimesCard
{
    public class UserTimesCardService : IUserTimesCardService
    {
        private readonly DapperRepository<AccountTimesCardSearchParamResultItem> _accountTimesCardDapperRepository;
        private readonly DapperRepository<AccountTimesCard> _accountTimesCardRepository;
        private readonly IBasicDataService _basicDataService = new BasicDataService();
        private readonly IGoodsService _goodsService = new GoodsService();
        private readonly ISharedService _shardService = new SharedService();
        private readonly DapperRepository<UserHandle> _useraddDapperRepository;
        private readonly DapperRepository<UserInfoDetail> _userInfoRepository;
        private readonly DapperRepository<UserLogInfo> _userLogRepository;
        private readonly IUserService _userService = new UserService();
        private readonly DapperRepository<UserStoreTimes> _userTimesCardRepository;
        private readonly DapperRepository<UserTimesCardSearchParamResultItem> _userTimesCardResultItemRepository;

        private readonly DapperRepository<UserTimesCardTransactionRecordResultItem>
            _userTimesCardTransactionRecordResultItemRepository;

        public UserTimesCardService()
        {
            var dapperDbContext =
                new DapperDbContext(new SqlConnection(WebConfigSetting.Instance.I200DbConnectionString));
            _accountTimesCardRepository = new DapperRepository<AccountTimesCard>(dapperDbContext);
            _userTimesCardRepository = new DapperRepository<UserStoreTimes>(dapperDbContext);
            _userInfoRepository = new DapperRepository<UserInfoDetail>(dapperDbContext);
            _userLogRepository = new DapperRepository<UserLogInfo>(dapperDbContext);
            _accountTimesCardDapperRepository =
                new DapperRepository<AccountTimesCardSearchParamResultItem>(dapperDbContext);
            _userTimesCardResultItemRepository =
                new DapperRepository<UserTimesCardSearchParamResultItem>(dapperDbContext);
            _userTimesCardTransactionRecordResultItemRepository =
                new DapperRepository<UserTimesCardTransactionRecordResultItem>(dapperDbContext);
            _useraddDapperRepository = new DapperRepository<UserHandle>(dapperDbContext);
        }

        /// <summary>
        ///     新增店铺计次卡
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public ResponseModel AddAccountTimesCard(AccountTimesCard request, UserContext userContext)
        {
            request.AccId = userContext.AccId;
            var result = _accountTimesCardRepository.Insert(request);

            //2.返回新增的支出实体
            return new ResponseModel
            {
                Code = result ? (int) ErrorCodeEnum.Success : (int) ErrorCodeEnum.OperateTimesCardFailed,
                Data = request
            };
        }

        /// <summary>
        ///     校验无限制的计次卡
        /// </summary>
        /// <param name="accId"></param>
        /// <returns></returns>
        public bool ExistUnlimitedTimesCard(int accId)
        {
            var result = _accountTimesCardRepository.Find(x => x.AccId == accId && x.BindGoodsId == 0, null,
                item => new {item.Id, item.CardName});
            return result != null;
        }

        /// <summary>
        ///     校验是否是服务类项目
        /// </summary>
        /// <param name="accId"></param>
        /// <param name="bindGoodsId"></param>
        /// <returns></returns>
        public bool ExistServiceOnly(int accId, int bindGoodsId)
        {
            var result =
                _accountTimesCardRepository.Find(
                    x => x.AccId == accId && x.BindGoodsId == bindGoodsId, null, item => new {item.Id, item.CardName});
            return result != null;
        }

        /// <summary>
        ///     校验是否存在同名的计次卡
        /// </summary>
        /// <param name="accId"></param>
        /// <param name="cardName"></param>
        /// <returns></returns>
        public bool ExistSameNameTimesCard(int accId, string cardName)
        {
            var result =
                _accountTimesCardRepository.Find(x => x.AccId == accId && x.CardName == cardName, null,
                    item => new {item.Id, item.CardName});
            return result != null;
        }

        /// <summary>
        ///     修改店铺计次卡
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public ResponseModel UpdateAccountTimesCard(AccountTimesCard request, UserContext userContext)
        {
            //1.修改店铺计次卡
            var result = _accountTimesCardRepository.Update<AccountTimesCard>(request, item => new
            {
                item.CardName
            });

            //2.返回修改店铺计次卡后的数据实体
            return new ResponseModel
            {
                Code = result ? (int) ErrorCodeEnum.Success : (int) ErrorCodeEnum.OperateTimesCardFailed,
                Data = request
            };
        }

        /// <summary>
        ///     删除店铺计次卡
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public ResponseModel DeleteAccountTimesCard(int cardId, UserContext userContext)
        {
            var request = new AccountTimesCard
            {
                Id = cardId
            };

            //1.删除店铺计次卡
            var result = _accountTimesCardRepository.Delete(request);

            //2.返回
            return new ResponseModel
            {
                Code = result ? (int) ErrorCodeEnum.Success : (int) ErrorCodeEnum.OperateTimesCardFailed,
                Data = cardId
            };
        }

        /// <summary>
        ///     获取店铺计次卡列表
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public ResponseModel GetAccountTimesCardsList(AccountTimeCardsSearchParam searchParam, UserContext userContext)
        {
            if (searchParam.CurrentPage == null || searchParam.CurrentPage < 1)
                searchParam.CurrentPage = 1;
            if (searchParam.PageSize == null || searchParam.PageSize < 1)
                searchParam.PageSize = 25;

            //排序
            var rankName = searchParam.Rank;
            var sort = searchParam.Sort;
            if (rankName != "cardName" && rankName != "cardId" && rankName != "totalUserCards" && rankName != "totalAvailableCards")
            {
                rankName = "A.cardId";
            }

            if (string.IsNullOrWhiteSpace(sort)|| (sort.ToLower() !="desc" && sort.ToLower() !="asc"))
            {
                sort = "Desc";
            }

             var orderStrSql= new StringBuilder();
             orderStrSql.Append("SELECT ROW_NUMBER() OVER( ");
             orderStrSql.Append(" ORDER BY " + rankName + "   " + sort + " )");
             var strSql = orderStrSql + @"  AS rownumber,A.cardId 
	                        ,A.NAME as  cardName
	                        ,A.bindGoodsId as bindGoodsId
	                        ,D.gName as BindGoodsName
	                        ,Isnull(B.totalUserCards, 0) totalUserCards
	                        ,Isnull(C.totalAvailableCards, 0) totalAvailableCards
                        FROM (
	                        SELECT id AS cardId
		                        ,cardName AS NAME
		                        ,bindGoodsId
	                        FROM T_Act_TimesCard
	                        WHERE accId = @AccId
	                        ) A
                        LEFT JOIN (
	                        SELECT ISNULL(accTimesCardId, 0) cardId
		                        ,COUNT(uid) totalUserCards
	                        FROM T_User_StoreTimes
	                        WHERE accID = @AccId
	                        GROUP BY ISNULL(accTimesCardId, 0)
	                        ) B ON A.cardId = B.cardId
                        LEFT JOIN (
	                        SELECT ISNULL(accTimesCardId, 0) cardId
		                        ,COUNT(uid) totalAvailableCards
	                        FROM T_User_StoreTimes
	                        WHERE accID = @AccId
		                        AND closeDate >= GETDATE()
	                        GROUP BY ISNULL(accTimesCardId, 0)
	                        ) C ON A.cardId = C.cardId
                        LEFT JOIN T_GoodsInfo D ON A.bindGoodsId = D.gid
	                        AND D.accID = @AccId
                        ";
            if (!string.IsNullOrWhiteSpace(searchParam.CardName))
            {
                strSql += " where A.NAME=@CardName ";
            }
            var searchSql = new StringBuilder();
            //分页查询
            searchSql.Append(" SELECT *");
            searchSql.Append(" FROM (");
            searchSql.Append(strSql);
            searchSql.Append("   ) AS T");
            searchSql.Append(" WHERE RowNumber BETWEEN (@PageIndex-1)*@PageSize+1  ");
            searchSql.Append("     AND @PageSize*@PageIndex ;");

            // 汇总数据
            searchSql.Append(" SELECT @TotalNum=COUNT(1) from T_Act_TimesCard where accId=@AccId ");
            if (!string.IsNullOrWhiteSpace(searchParam.CardName))
            {
                searchSql.Append(" and cardName = @CardName ");
            }

            var dapperParam = new DynamicParameters(new
            {
                userContext.AccId,
                PageSize = searchParam.PageSize ?? 25,
                PageIndex = searchParam.CurrentPage ?? 1,
                searchParam.CardName
            });

            dapperParam.Add("TotalNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var sqlQuery = new SqlQuery(searchSql.ToString(), dapperParam);
            var result = new AccountTimesCardSearchParamResult
            {
                Items = _accountTimesCardDapperRepository.FindAll(sqlQuery),
                CurrentPage = searchParam.CurrentPage ?? 1,
                PageSize = searchParam.PageSize ?? 25
            };
            var itemCount = dapperParam.Get<int?>("TotalNum") ?? 0;
            result.TotalTimesCardTypes = itemCount;
            result.TotalSize = itemCount;
            result.TotalPage =
                Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(result.TotalSize)/Convert.ToDecimal(result.PageSize)));
            //2.返回查询结果
            return new ResponseModel
            {
                Code = (int) ErrorCodeEnum.Success,
                Data = result
            };
        }

        /// <summary>
        ///     获取店铺计次卡信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="accId"></param>
        /// <returns></returns>
        public AccountTimesCard GetAccountTimesCard(int id, int accId)
        {
            var result = _accountTimesCardRepository.Find(x => x.Id == id && x.AccId == accId);
            return result;
        }

        /// <summary>
        ///     创建用户计次卡
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseModel AddUserTimesCard(UserContext userContext, UserStoreTimes request)
        {
            //1.如果不是会员，先新增会员进行充次
            var tranFlag = false;
            var nowIntegral = GetStoreTimesCardCanAddIntegral(userContext, request.ReceiveMoney);
            var conn = _useraddDapperRepository.Connection;
            conn.Open();
            var trans = conn.BeginTransaction();
            try
            {
                if (request.UserId == 0)
                {
                    //1.1 如果会员不存在，则先添加会员
                    var userHandel = new UserHandle
                    {
                        Id = request.UserId,
                        UserNo = _userService.GetUserNewNo(userContext.AccId).Data.ToString(),
                        UserPhone = request.Phone,
                        UserName = request.UserName,
                        StoreTimes = request.StoreTimes,
                        Integral = nowIntegral,
                        PinYin = Helper.GetPinyin(request.UserName),
                        PY = Helper.GetInitials(request.UserName),
                        OperatorId = userContext.UserId,
                        AccId = userContext.AccId
                    };

                    var userAddResult = _useraddDapperRepository.Insert(userHandel, trans);
                    if (userAddResult)
                    {
                        //1.2 绑定会员计次卡,并充次
                        var userTimesCard = new UserStoreTimes
                        {
                            StId = request.StId,
                            AccId = userContext.AccId,
                            UserId = userHandel.Id,
                            StoreTimes = request.StoreTimes,
                            ReceiveMoney = request.ReceiveMoney,
                            EditTime = DateTime.Now,
                            CardName = request.CardName,
                            BindGoodsId = request.BindGoodsId,
                            ExpireDate = request.ExpireDate,
                            AccTimesCardId = request.AccTimesCardId
                        };

                        var bindResult = _userTimesCardRepository.Insert(userTimesCard, trans);
                        if (bindResult)
                        {
                            //1.3 记录充次日志
                            var userLog = new UserLog
                            {
                                AccId = userContext.AccId,
                                UserId = userHandel.Id,
                                LogType = (int) UserLogTypeEnum.TimesCardChange,
                                ItemType = (int) UserLogItemTypeEnum.Shopping,
                                OriginValue = 0,
                                EditValue = request.StoreTimes,
                                FinalValue = request.StoreTimes,
                                OperatorId = userContext.UserId,
                                OperatorIp = userContext.IpAddress,
                                OperatorTime = DateTime.Now,
                                Flag = string.Empty,
                                FlagStatus = 0,
                                FlagStatusTime = DateTime.Now,
                                EditMoney = request.ReceiveMoney,
                                EditMoneyType = request.PayType,
                                LogTime = DateTime.Now,
                                OriginAccId = userContext.AccId,
                                BindCardId = request.BindGoodsId,
                                Remark = request.Remark,
                                AddedLgUserId = userContext.UserId
                            };
                            var logResult = AddUserLog(userLog, userContext, trans);
                            if (logResult)
                            {
                                tranFlag = true;
                            }
                        }
                    }
                }
                else
                {
                    //2.绑定计次卡并充次
                    var result = _userTimesCardRepository.Insert(request, trans);
                    if (result)
                    {
                        var userInfo = _userService.GetUserDetail(userContext, request.UserId);
                        var updateRequest = new UserHandle
                        {
                            Id = request.UserId,
                            StoreTimes = userInfo.UserTimesCardCount + request.StoreTimes,
                            Integral = userInfo.UserIntegral + nowIntegral
                        };
                        var updateResult = _userService.UpdateUser(updateRequest);
                        if (updateResult.Code == (int) ErrorCodeEnum.Success)
                        {
                            var userLog = new UserLog
                            {
                                AccId = userContext.AccId,
                                UserId = request.UserId,
                                LogType = (int) UserLogTypeEnum.TimesCardChange,
                                ItemType = (int) UserLogItemTypeEnum.Shopping,
                                OriginValue = userInfo.UserTimesCardCount,
                                EditValue = request.StoreTimes,
                                FinalValue = request.StoreTimes + userInfo.UserTimesCardCount,
                                OperatorId = userContext.UserId,
                                OperatorIp = userContext.IpAddress,
                                OperatorTime = DateTime.Now,
                                Flag = string.Empty,
                                FlagStatus = 0,
                                FlagStatusTime = DateTime.Now,
                                EditMoney = request.ReceiveMoney,
                                EditMoneyType = request.PayType,
                                LogTime = DateTime.Now,
                                OriginAccId = userContext.AccId,
                                BindCardId = request.StId,
                                Remark = request.Remark,
                                AddedLgUserId = userContext.UserId
                            };
                            var logResult = AddUserLog(userLog, userContext, trans);
                            if (logResult)
                            {
                                tranFlag = true;
                            }
                        }
                    }
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
                }
                else
                {
                    trans.Rollback();
                }
                trans.Dispose();
                conn.Close();
            }

            //3.返回结果
            return new ResponseModel
            {
                Code = tranFlag ? (int) ErrorCodeEnum.Success : (int) ErrorCodeEnum.OperateTimesCardFailed,
                Data = request
            };
        }

        /// <summary>
        ///     更新用户计次卡
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseModel EditUserTimesCard(UserContext userContext, UserStoreTimes request)
        {
            var userStoreTimes = new UserStoreTimes
            {
                StId = request.StId,
                ExpireDate = request.ExpireDate,
                EditTime = DateTime.Now
            };

            var result = _userTimesCardRepository.Update<UserStoreTimes>(userStoreTimes, x => new
            {
                x.ExpireDate,
                x.EditTime
            });
            return new ResponseModel
            {
                Code = result ? (int) ErrorCodeEnum.Success : (int) ErrorCodeEnum.OperateTimesCardFailed,
                Data = request
            };
        }

        /// <summary>
        ///     删除用户计次卡
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public ResponseModel DeleteUserTimesCard(UserContext userContext, int cardId)
        {
            var request = new UserStoreTimes
            {
                StId = cardId
            };

            //1.删除店铺计次卡
            var result = _userTimesCardRepository.Delete(request);

            //2.返回
            return new ResponseModel
            {
                Code = result ? (int) ErrorCodeEnum.Success : (int) ErrorCodeEnum.OperateTimesCardFailed,
                Data = cardId
            };
        }

        /// <summary>
        ///     用户计次卡充次
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseModel UserTimesCardIncharge(UserContext userContext, UserTimesCardAdd request)
        {
            //1.先判断当前会员是否存在
            var tranFlag = false;
            var nowIntegral = GetStoreTimesCardCanAddIntegral(userContext, request.ReceiveMoney);
            var conn = _useraddDapperRepository.Connection;
            conn.Open();
            var trans = conn.BeginTransaction();
            try
            {
                if (request.UserId == 0)
                {
                    //1.1 如果会员不存在，则先添加会员
                    var userHandel = new UserHandle
                    {
                        Id = request.UserId,
                        UserNo = _userService.GetUserNewNo(userContext.AccId).Data.ToString(),
                        UserPhone = request.Phone,
                        UserName = request.UserName,
                        StoreTimes = request.StoreTimes,
                        Integral = nowIntegral,
                        PinYin = Helper.GetPinyin(request.UserName),
                        PY = Helper.GetInitials(request.UserName),
                        OperatorId = userContext.UserId,
                        AccId = userContext.AccId
                    };
                    var userInsertResult = _useraddDapperRepository.Insert(userHandel, trans);
                    if (userInsertResult)
                    {
                        //1.2 绑定会员计次卡,并充次
                        var userTimesCard = new UserStoreTimes
                        {
                            StId = request.UserCardId,
                            AccId = userContext.AccId,
                            UserId = userHandel.Id,
                            StoreTimes = request.StoreTimes,
                            ReceiveMoney = request.ReceiveMoney,
                            EditTime = DateTime.Now,
                            CardName = request.CardName,
                            BindGoodsId = request.BindGoodsId,
                            ExpireDate = request.Expiredate,
                            AccTimesCardId = request.AccCardId
                        };

                        var bindResult = _userTimesCardRepository.Insert(userTimesCard, trans);
                        if (bindResult)
                        {
                            //1.3 记录充次日志
                            var userLog = new UserLog
                            {
                                AccId = userContext.AccId,
                                UserId = userHandel.Id,
                                LogType = (int) UserLogTypeEnum.TimesCardChange,
                                ItemType = (int) UserLogItemTypeEnum.Shopping,
                                OriginValue = 0,
                                EditValue = request.StoreTimes,
                                FinalValue = request.StoreTimes,
                                OperatorId = userContext.UserId,
                                OperatorIp = userContext.IpAddress,
                                OperatorTime = DateTime.Now,
                                Flag = string.Empty,
                                FlagStatus = 0,
                                FlagStatusTime = DateTime.Now,
                                EditMoney = request.ReceiveMoney,
                                EditMoneyType = request.PayType,
                                LogTime = DateTime.Now,
                                OriginAccId = userContext.AccId,
                                BindCardId = request.BindGoodsId,
                                Remark = request.Remark,
                                AddedLgUserId = userContext.UserId
                            };

                            var logResult = AddUserLog(userLog, userContext, trans);
                            if (logResult)
                            {
                                tranFlag = true;
                            }
                        }
                        return new ResponseModel
                        {
                            Code = tranFlag ? (int) ErrorCodeEnum.Success : (int) ErrorCodeEnum.Failed,
                            Data = request
                        };
                    }
                }

                //2.更新用户计次信息
                var timesCard =
                    _userTimesCardRepository.Find(
                        x => x.AccId == userContext.AccId && x.UserId == request.UserId && x.StId == request.UserCardId);
                if (timesCard == null)
                {
                    return new ResponseModel
                    {
                        Code = (int) ErrorCodeEnum.TimeCardGetById,
                        Data = null
                    };
                }

                var timesCardUpdateModel = new UserStoreTimes
                {
                    StId = request.UserCardId,
                    StoreTimes = timesCard.StoreTimes + request.StoreTimes,
                    ReceiveMoney = timesCard.ReceiveMoney + request.ReceiveMoney,
                    EditTime = DateTime.Now
                };

                var timesCardUpdate = _userTimesCardRepository.Update(timesCardUpdateModel);

                //3.计次卡充值
                var userInfo = _userService.GetUserDetail(userContext, request.UserId);
                var updateRequest = new UserHandle
                {
                    Id = request.UserId,
                    StoreTimes = userInfo.UserTimesCardCount + request.StoreTimes,
                    Integral = userInfo.UserIntegral + nowIntegral
                };
                var storeResult = _userService.UpdateUser(updateRequest);

                //4.如果是本店会员则添加日志
                if ((storeResult.Code == (int) ErrorCodeEnum.Success) && timesCardUpdate &&
                    userInfo.AccId == userContext.AccId)
                {
                    var storeLogInfo = new UserLogInfo
                    {
                        AccId = userContext.AccId,
                        OriginalAccId = userContext.AccId,
                        UId = request.UserId,
                        LogType = (int) UserLogTypeEnum.IntegralChange,
                        ItemType = (int) UserLogItemTypeEnum.StoreTimesCard,
                        OriginalVal = userInfo.UserTimesCardCount,
                        EditMoney = request.ReceiveMoney,
                        EditVal = request.StoreTimes,
                        FinalVal = userInfo.UserTimesCardCount + request.StoreTimes,
                        LogTime = DateTime.Now,
                        OperatorTime = DateTime.Now,
                        OperatorId = userContext.UserId,
                        OperatorIp = userContext.IpAddress,
                        Remark = request.Remark,
                        Flag = string.Empty,
                        FlagStatus = 0,
                        FlagStatusTime = DateTime.Now,
                        EditMoneyType = request.PayType,
                        AddedLgUserId = userContext.AccId,
                        BindCardId = request.AccCardId
                    };

                    //5.记录日志
                    _userLogRepository.Insert(storeLogInfo);
                }

                //6.发送提醒短信
                if (request.SendMsg)
                {
                    Task.Run(() => { _shardService.SendSms(userContext); });
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
                }
                else
                {
                    trans.Rollback();
                }
                trans.Dispose();
                conn.Close();
            }

            //7.返回执行结果
            return new ResponseModel
            {
                Code = (int) ErrorCodeEnum.Success,
                Data = request
            };
        }

        /// <summary>
        ///     获取用户的计次卡列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="searchParam"></param>
        /// <returns></returns>
        public ResponseModel GetUserTimesCardList(UserContext userContext, UserTimesCardSearchParam searchParam)
        {
            if (searchParam.CurrentPage == null || searchParam.CurrentPage < 1)
                searchParam.CurrentPage = 1;
            if (searchParam.PageSize == null || searchParam.PageSize < 1)
                searchParam.PageSize = 25;
            var strWhereSql = new StringBuilder();
            var userTimesCardSearchParamResult = new UserTimesCardSearchParamResult();

            //1.搜索框筛选
            if (!string.IsNullOrWhiteSpace(searchParam.UserId))
            {
                strWhereSql.Append(" and m.uid = @keyWord ");
            }

            //2.计次卡名筛选
            if (!string.IsNullOrWhiteSpace(searchParam.CardId))
            {
                strWhereSql.Append(" and m.stID= @cardId ");
            }

            if (searchParam.Status != null)
            {
                //3.状态筛选（1已过期，0正常）
                strWhereSql.Append(searchParam.Status == 0
                    ? "and datediff(day,getdate(),m.closeDate)>=0 "
                    : "and datediff(day,getdate(),m.closeDate)<0  ");
            }

            //4.排序
            var rankName = "stId";
            var sort = "Desc";
            if (!string.IsNullOrWhiteSpace(searchParam.Rank))
            {
                rankName = GetRankAndSortText(searchParam.Rank);
            }
            if (!string.IsNullOrWhiteSpace(searchParam.Sort))
            {
                if (sort.ToLower() == "desc" || sort.ToLower() == "asc")
                {
                    sort = searchParam.Sort;
                }
            }
            var searchStrSql = new StringBuilder();
            var containBranchStoreSqlStr = new StringBuilder();
            var allWhereStrSql = new StringBuilder();
            containBranchStoreSqlStr.Append("SELECT ROW_NUMBER() OVER( ");
            containBranchStoreSqlStr.Append(" ORDER BY " + rankName + "   " + sort + "");
            containBranchStoreSqlStr.Append(
                ") AS rowNumber, m.stID AS userCardId,n.uName AS userName,m.cardName as cardName,m.bindGoodsId AS bindGoodsId,m.uid as UserId,n.uPhone as Phone,m.accTimesCardId as accCardId,");
            containBranchStoreSqlStr.Append(
                "g.gName AS bindGoodsName,m.storeTimes AS leaveTimes,m.storeMoney as storeMoney, ");
            containBranchStoreSqlStr.Append(
                "m.closeDate AS expireDate,CASE WHEN datediff(day,getdate(),closeDate)>=0 THEN 0 ELSE 1 END AS [status]   ");
            containBranchStoreSqlStr.Append("FROM T_User_StoreTimes m ");
            containBranchStoreSqlStr.Append("LEFT JOIN  dbo.T_UserInfo n ON m.accID= n.accID AND m.uid=n.uid ");
            containBranchStoreSqlStr.Append("LEFT JOIN  dbo.T_GoodsInfo g ON m.accID=g.accID AND m.bindGoodsId = g.gid ");
            containBranchStoreSqlStr.Append(
                "WHERE m.accID IN ( select ID  from T_Account WITH (NOLOCK)  where max_shop in(select max_shop from T_Account WITH (NOLOCK) where ID=@accId))  ");
            containBranchStoreSqlStr.Append(strWhereSql);

            allWhereStrSql.Append(
                "AS T WHERE rowNumber BETWEEN (@PageIndex-1)*@PageSize+1  AND @PageSize*@PageIndex ");


            //统计总金额
            var totalStrSql = new StringBuilder();
            totalStrSql.Append(
                "select @TotalUsers=COUNT(DISTINCT m0.UserId),@TotalCards=COUNT(DISTINCT m0.userCardId),@TotalAvaiable=isnull(sum(m0.leaveTimes),0),@TotalNum = count(1),@TotalStoreMoney=isnull(sum(m0.storeMoney),0)  ");
            totalStrSql.Append(" from ( ");
            totalStrSql.Append(containBranchStoreSqlStr);
            totalStrSql.Append(" ) ");
            totalStrSql.Append(" as m0 ");

            var dapperParam = new DynamicParameters(
                new
                {
                    accId = userContext.AccId,
                    keyWord = searchParam.UserId,
                    cardId = searchParam.CardId,
                    searchParam.PageSize,
                    PageIndex = searchParam.CurrentPage
                });

            dapperParam.Add("TotalUsers", dbType: DbType.Int32, direction: ParameterDirection.Output);
            dapperParam.Add("TotalCards", dbType: DbType.Int32, direction: ParameterDirection.Output);
            dapperParam.Add("TotalAvaiable", dbType: DbType.Int32, direction: ParameterDirection.Output);
            dapperParam.Add("TotalNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            dapperParam.Add("TotalStoreMoney", dbType: DbType.Decimal, direction: ParameterDirection.Output,
                precision: 10, scale: 2);

            searchStrSql.Append(" SELECT * FROM (")
                .Append(containBranchStoreSqlStr)
                .Append(" ) ")
                .Append(allWhereStrSql)
                .Append(totalStrSql);
            var sqlQuery = new SqlQuery(searchStrSql.ToString(), dapperParam);
            userTimesCardSearchParamResult.Items =
                _userTimesCardResultItemRepository.FindAll(sqlQuery);
            userTimesCardSearchParamResult.TotalAvaiable = dapperParam.Get<int?>("TotalAvaiable") ?? 0;
            userTimesCardSearchParamResult.TotalCards = dapperParam.Get<int?>("TotalCards") ?? 0;
            userTimesCardSearchParamResult.TotalUsers = dapperParam.Get<int?>("TotalUsers") ?? 0;
            userTimesCardSearchParamResult.TotalSize = dapperParam.Get<int?>("TotalNum") ?? 0;
            userTimesCardSearchParamResult.TotalStoreMoney = dapperParam.Get<decimal?>("TotalStoreMoney") ?? 0;
            userTimesCardSearchParamResult.CurrentPage = searchParam.CurrentPage ?? 1;
            userTimesCardSearchParamResult.PageSize = searchParam.PageSize ?? 25;
            userTimesCardSearchParamResult.TotalPage =
                Convert.ToInt32(
                    Math.Ceiling(Convert.ToDecimal(userTimesCardSearchParamResult.TotalSize)/
                                 Convert.ToDecimal(userTimesCardSearchParamResult.PageSize)));
            //5.返回查询结果
            return new ResponseModel
            {
                Code = (int) ErrorCodeEnum.Success,
                Data = userTimesCardSearchParamResult
            };
        }

        /// <summary>
        ///     获取用户计次卡交易记录
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="searchParam"></param>
        /// <returns></returns>
        public ResponseModel GetUserTimesCardChargeList(UserContext userContext,
            UserTimesCardTransactionRecordSearchParam searchParam)
        {
            if (searchParam.CurrentPage == null || searchParam.CurrentPage < 1)
                searchParam.CurrentPage = 1;
            if (searchParam.PageSize == null || searchParam.PageSize < 1)
                searchParam.PageSize = 25;
            var userTimesCardTransactionRecordResult = new UserTimesCardTransactionRecordResult();
            var searchStrSql = new StringBuilder();
            var strSql = new StringBuilder();
            var whereSqlStr = new StringBuilder();
            var rowStrSql = new StringBuilder();
            rowStrSql.Append(" where RowNumber BETWEEN (@PageIndex-1)*@PageSize+1 ");
            rowStrSql.Append(" AND @PageSize*@PageIndex");
            whereSqlStr.Append(" where m.accId=@accId  ");

            //1.筛选项搜索关键字
            if (!string.IsNullOrWhiteSpace(searchParam.UserId))
            {
                whereSqlStr.Append(
                    " and m.uid=@keyWord  ");
            }
            //2.操作类型（（1：充次 2：消费））
            if (searchParam.OperateType != null && searchParam.OperateType == 1)
            {
                whereSqlStr.Append(" and  m.logType=2  and m.itemType=1 ");
            }
            else if (searchParam.OperateType != null && searchParam.OperateType == 2)
            {
                whereSqlStr.Append(" and  m.logType=2  and m.itemType=2 ");
            }
            else
            {
                whereSqlStr.Append(" and  m.logType=2  and (m.itemType=1 or m.itemType=2 ) ");
            }
            //3.开始时间-结束时间
            if (searchParam.StartDate != null && searchParam.EndDate != null &&
                searchParam.StartDate <= searchParam.EndDate)
            {
                whereSqlStr.Append(" and m.logTime>=@StartDate and m.logTime<=@EndDate ");
            }

            strSql.Append(
                " SELECT ROW_NUMBER() OVER(ORDER BY m.LogTime DESC) AS rowNumber, m.LogTime AS createdAt,m.uid AS userId,m.EditVal,m.EditMoney,m.logType,m.itemType, ");
            strSql.Append(" n.uName AS userName, CASE when m.itemType=1 THEN 1 ELSE 2 END AS operateType, ");
            strSql.Append(" m.EditVal AS editTimes,m.FinalVal AS leaveTimes,g.name AS salesman,m.Remark AS Remark ");
            strSql.Append(" FROM dbo.T_User_LogInfo m ");
            strSql.Append(" LEFT JOIN dbo.T_UserInfo n ");
            strSql.Append(" ON m.uid= n.uid ");
            strSql.Append(" LEFT JOIN dbo.T_Account_User g ");
            strSql.Append(" ON m.addedLgUserId = g.id ");
            strSql.Append(whereSqlStr);

            searchStrSql.Append(" select * from (").Append(strSql).Append(" ) as T ").Append(rowStrSql);
            searchStrSql.Append(" select * into  #TempList from (").Append(strSql).Append(" ) as P ");

            //4.统计汇总信息
            var totalStrSql = new StringBuilder();

            //4.1合计消费次数
            totalStrSql.Append("select @TotalConsumingCount= isnull(sum(EditVal),0) ");
            totalStrSql.Append(" from #TempList where itemType=2 ;");

            //4.2合计会员
            totalStrSql.Append("select @TotalUsers=COUNT(DISTINCT userId) ");
            totalStrSql.Append(" from #TempList; ");

            //4.3合计充次
            totalStrSql.Append("select @TotalInchargeCount= isnull(sum(EditVal),0) ");
            totalStrSql.Append(" from #TempList where itemType=1 ;");

            //4.4合计充次金额
            totalStrSql.Append("select @TotalInchargeAmount= isnull(sum(EditMoney),0) ");
            totalStrSql.Append(" from  #TempList where itemType=1 ;");

            //4.5 总记录
            totalStrSql.Append("select @TotalNum=count(1) ");
            totalStrSql.Append(" from #TempList;");
            totalStrSql.Append(" drop table #TempList;");

            searchStrSql.Append(totalStrSql);

            var dapperParam = new DynamicParameters(
                new
                {
                    accId = userContext.AccId,
                    keyWord = searchParam.UserId,
                    searchParam.StartDate,
                    searchParam.EndDate,
                    searchParam.PageSize,
                    PageIndex = searchParam.CurrentPage
                });
            var sqlQuery = new SqlQuery(searchStrSql.ToString(), dapperParam);
            dapperParam.Add("TotalConsumingCount", dbType: DbType.Int32, direction: ParameterDirection.Output,
                precision: 10, scale: 2);
            dapperParam.Add("TotalUsers", dbType: DbType.Int32, direction: ParameterDirection.Output);
            dapperParam.Add("TotalInchargeCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            dapperParam.Add("TotalInchargeAmount", dbType: DbType.Decimal, direction: ParameterDirection.Output,
                precision: 10, scale: 2);
            dapperParam.Add("TotalNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            userTimesCardTransactionRecordResult.Items =
                _userTimesCardTransactionRecordResultItemRepository.FindAll(sqlQuery);

            //合计消费次数
            userTimesCardTransactionRecordResult.TotalConsumingCount =
                dapperParam.Get<int?>("TotalConsumingCount") ?? 0;
            //合计会员
            userTimesCardTransactionRecordResult.TotalUsers =
                dapperParam.Get<int?>("TotalUsers") ?? 0;
            //合计充次
            userTimesCardTransactionRecordResult.TotalInchargeCount =
                dapperParam.Get<int?>("TotalInchargeCount") ?? 0;
            userTimesCardTransactionRecordResult.TotalInchargeAmount =
                //合计充次金额
                dapperParam.Get<decimal?>("TotalInchargeAmount") ?? 0;
            userTimesCardTransactionRecordResult.CurrentPage = searchParam.CurrentPage ?? 1;
            userTimesCardTransactionRecordResult.PageSize = searchParam.PageSize ?? 25;
            userTimesCardTransactionRecordResult.TotalSize = dapperParam.Get<int?>("TotalNum") ?? 0;
            userTimesCardTransactionRecordResult.TotalPage =
                Convert.ToInt32(
                    Math.Ceiling(Convert.ToDecimal(userTimesCardTransactionRecordResult.TotalSize)/
                                 Convert.ToDecimal(userTimesCardTransactionRecordResult.PageSize)));
            return new ResponseModel
            {
                Code = (int) ErrorCodeEnum.Success,
                Data = userTimesCardTransactionRecordResult
            };
        }

        /// <summary>
        ///     获取当前店铺的所有计次卡项目，提供前台下拉框筛选
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public ResponseModel GetCurrentStoreTimesCardNameByAccId(UserContext userContext)
        {
            var result =
                _accountTimesCardRepository.FindAll(x => x.AccId == userContext.AccId)
                    .ToList().Distinct();
            return new ResponseModel
            {
                Data = result,
                Code = (int) ErrorCodeEnum.Success
            };
        }

        /// <summary>
        ///     获取当前的店铺的服务类项目
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public ResponseModel GetCurrentStoreServiceItemExceptBinging(UserContext userContext)
        {
            var serviceItemIds = _goodsService.GetServiceGoodsInfo(userContext);
            var hasBindingItemsIds =
                _accountTimesCardRepository.FindAll(x => x.AccId == userContext.AccId).Select(x => x.BindGoodsId);
            var bindingItemsIds = hasBindingItemsIds as int[] ?? hasBindingItemsIds.ToArray();
            var result = serviceItemIds.Where(x => !bindingItemsIds.Contains(x.Id)).ToList();
            if (!bindingItemsIds.Contains(0))
            {
                var goodsInfoSummary = new GoodsInfoSummary
                {
                    AccId = userContext.AccId,
                    GName = "无限制",
                    Id = 0,
                    IsService = 1
                };
                result.Add(goodsInfoSummary);
            }
            return new ResponseModel
            {
                Code = (int) ErrorCodeEnum.Success,
                Data = result
            };
        }

        /// <summary>
        ///     获取用户计次卡信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userContext"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public UserStoreTimes GetUserTimesCardInfo(int userId, UserContext userContext, int cardId)
        {
            return
                _userTimesCardRepository.Find(
                    x => x.UserId == userId && x.AccTimesCardId == cardId && x.AccId == userContext.AccId);
        }

        /// <summary>
        ///     根据用户计次卡Id获取用户计次卡信息
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public ResponseModel GetUserTimesCardInfoByCardId(UserContext userContext, int cardId)
        {
            var result = _userTimesCardRepository.Find(x => x.AccId == userContext.AccId && x.StId == cardId);
            return new ResponseModel
            {
                Code = result == null ? (int) ErrorCodeEnum.TimeCardGetById : (int) ErrorCodeEnum.Success,
                Data = result
            };
        }

        /// <summary>
        ///     计次卡充次时，根据会员查询结果，如果是会员：（1）该会员有计次卡，下拉框计次卡列表显示会员已有计次卡，该会员无计次卡，下拉显示
        ///     当前店铺所有计次卡 ； 如果是非会员，下拉显示当前店铺所有计次卡
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public ResponseModel GetUserTimesCardOrAccTimesCard(UserContext userContext, UserHandle userInfo)
        {
            var userTimesCardItems = new List<UserTimesCardItem>();
            //1.非会员显示店铺所有计次卡
            if (userInfo.Id == 0)
            {
                userTimesCardItems = GetAccAllTimesCards(userContext);
            }
            else
            {
                //2.会员
                var result =
                    _userTimesCardRepository.FindAll(x => x.AccId == userContext.AccId && x.UserId == userInfo.Id);

                //2.1先查询该会员是否存在计次卡，存在直接显示当前计次卡
                if (result != null && result.Any())
                {
                    userTimesCardItems.AddRange(result.Select(oItem => new UserTimesCardItem
                    {
                        Id = oItem.StId,
                        CardName = oItem.CardName
                    }));
                }
                else
                {
                    //2.2 会员不存在计次卡则显示店铺所有计次卡
                    userTimesCardItems = GetAccAllTimesCards(userContext);
                }
            }

            return new ResponseModel
            {
                Code = (int) ErrorCodeEnum.Success,
                Data = userTimesCardItems
            };
        }

        /// <summary>
        ///     获取用户充次可获得的积分
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="receiveMoney"></param>
        /// <returns></returns>
        public int GetStoreTimesCardCanAddIntegral(UserContext userContext, decimal receiveMoney)
        {
            decimal propMoney = 0;
            var propIntegral = 0;
            var nowIntegral = 0;
            decimal tempPropIntegral = 0;
            var tempStrPropIntegral = "0";
            var integralConfig = _basicDataService.GetStoreIntegrationConfig(userContext);
            var propList = integralConfig.Split('/');
            decimal.TryParse(propList[0], out propMoney);
            decimal.TryParse(propList[1], out tempPropIntegral);
            tempStrPropIntegral = Math.Round(tempPropIntegral).ToString(CultureInfo.InvariantCulture);
            int.TryParse(tempStrPropIntegral, out propIntegral);
            if (propIntegral < 0)
            {
                propIntegral = 0;
            }
            if (propIntegral == 0)
            {
                nowIntegral = 0;
            }
            else
            {
                if (propMoney > 0)
                {
                    //计算积分
                    var tempInt = receiveMoney/propMoney;
                    nowIntegral = (int) Math.Floor(tempInt*propIntegral);
                }
            }
            return nowIntegral;
        }

        /// <summary>
        ///     获取店铺所有计次卡
        /// </summary>
        /// <param name="userContext"></param>
        private List<UserTimesCardItem> GetAccAllTimesCards(UserContext userContext)
        {
            var userTimesCardItems = new List<UserTimesCardItem>();
            var accTimesCards = _accountTimesCardRepository.FindAll(x => x.AccId == userContext.AccId);
            if (accTimesCards != null && accTimesCards.Any())
            {
                userTimesCardItems.AddRange(accTimesCards.Select(oItem => new UserTimesCardItem
                {
                    Id = oItem.Id,
                    CardName = oItem.CardName
                }));
            }
            return userTimesCardItems;
        }

        /// <summary>
        ///     记录用户日志（计次）
        /// </summary>
        /// <param name="userLog"></param>
        /// <param name="userContext"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public bool AddUserLog(UserLog userLog, UserContext userContext, IDbTransaction transaction)
        {
            var storeLogInfo = new UserLogInfo
            {
                AccId = userContext.AccId,
                OriginalAccId = userContext.AccId,
                UId = (int) userLog.UserId,
                LogType = (int) UserLogTypeEnum.TimesCardChange,
                ItemType = (int) UserLogItemTypeEnum.StoreTimesCard,
                OriginalVal = userLog.OriginValue,
                EditMoney = userLog.EditMoney,
                EditVal = userLog.EditValue,
                FinalVal = userLog.FinalValue,
                LogTime = userLog.LogTime,
                OperatorTime = DateTime.Now,
                OperatorId = userContext.UserId,
                OperatorIp = userContext.IpAddress,
                Remark = userLog.Remark,
                Flag = userLog.Flag,
                FlagStatus = userLog.FlagStatus,
                FlagStatusTime = userLog.FlagStatusTime,
                EditMoneyType = userLog.EditMoneyType,
                AddedLgUserId = (int) userLog.AddedLgUserId,
                BindCardId = (int) userLog.BindCardId
            };

            //记录日志
            return _userLogRepository.Insert(storeLogInfo, transaction);
        }

        /// <summary>
        ///     获取排序字段
        /// </summary>
        /// <param name="rank"></param>
        /// <returns></returns>
        public string GetRankAndSortText(string rank)
        {
            string rankName;
            switch (rank)
            {
                case "userCardId":
                    rankName = "stId";
                    break;
                case "userId":
                    rankName = "userId";
                    break;
                case "cardName":
                    rankName = "cardName";
                    break;
                case "leaveTimes":
                    rankName = "storeTimes";
                    break;
                case "expireDate":
                    rankName = "closeDate";
                    break;
                default:
                    rankName = "stId";
                    break;
            }

            return rankName;
        }
    }
}