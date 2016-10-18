using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CommonLib;
using CommonLib.UserSearch;
using Dapper;
using Script.I200.Core.Config;
using Script.I200.Data;
using Script.I200.Data.MicroOrm.SqlGenerator;
using Script.I200.Entity.Api.Coupon;
using Script.I200.Entity.Api.Users;
using Script.I200.Entity.API;
using Script.I200.Entity.Dto.Sales;
using Script.I200.Entity.Enum;
using Script.I200.Entity.Model.User;
using ResponseModel = Script.I200.Entity.API.ResponseModel;
using UserIntegral = Script.I200.Entity.Model.User.UserIntegral;

namespace Script.I200.Application.Users
{
    /// <summary>
    ///     会员相关接口（C端用户）
    /// </summary>
    public class UserService : IUserService
    {
        private readonly DapperRepository<UserBirthday> _userBirthdayDapperRepository;
        private readonly DapperRepository<BirthdayUsersResult> _userBrthdayResultRepository;
        private readonly DapperRepository<UserInfoDetail> _userInfoDapperRepository;
        private readonly DapperRepository<UserHandle> _useraddDapperRepository;
        private readonly DapperRepository<UserMessage> _userMessageDapperRepository;
        private readonly DapperRepository<UserStatistics> _userStatisticsDapperRepository;
        private readonly DapperRepository<string> _getRepository;
        private readonly DapperRepository<UserGroup> _usergroupDapperRepository;
        private readonly DapperRepository<UserTagInfo> _usertagDapperRepository;
        private readonly DapperRepository<UserTagKey> _usertagkeyDapperRepository;
        private readonly DapperRepository<UserTagsGet> _usertagsgetDapperRepository;
        private readonly DapperRepository<UserRank> _userRankRepository;
        private readonly DapperRepository<UserNickName> _usernickDapperRepository;
        private readonly DapperRepository<UserLogInfo> _userLogRepository;
        private readonly DapperRepository<UserIntegral> _userIntegralRepository;
        private readonly DapperRepository<SalesRecord> _userSalesRecordRepository;
        private readonly DapperRepository<SalesDetail> _userSalesDetailRepository;
        private readonly DapperRepository<UserListDetailSumData> _userlistotherdataDapperRepository;
        private readonly DapperRepository<GetAllActiveCoupon> _usergetallactivecouponDapperRepository;
        private readonly DapperRepository<UserCouponModel> _usercouponmodelDapperRepository;
        private readonly string _isOpenElasticsearchSearch =
                ConfigurationManager.AppSettings["IsOpenElasticsearchSearch"];
        /// <summary>
        ///     初始化
        /// </summary>
        public UserService()
        {
            var dapperContext =
                new DapperDbContext(new SqlConnection(WebConfigSetting.Instance.I200DbConnectionString));

            _userBirthdayDapperRepository = new DapperRepository<UserBirthday>(dapperContext);
            _userInfoDapperRepository = new DapperRepository<UserInfoDetail>(dapperContext);
            _userBrthdayResultRepository = new DapperRepository<BirthdayUsersResult>(dapperContext);
            _useraddDapperRepository = new DapperRepository<UserHandle>(dapperContext);
            _getRepository = new DapperRepository<string>(dapperContext);
            _userMessageDapperRepository = new DapperRepository<UserMessage>(dapperContext);
            _usergroupDapperRepository = new DapperRepository<UserGroup>(dapperContext);
            _usertagDapperRepository = new DapperRepository<UserTagInfo>(dapperContext);
            _userStatisticsDapperRepository = new DapperRepository<UserStatistics>(dapperContext);
            _usertagkeyDapperRepository = new DapperRepository<UserTagKey>(dapperContext);
            _userRankRepository = new DapperRepository<UserRank>(dapperContext);
            _usernickDapperRepository = new DapperRepository<UserNickName>(dapperContext);
            _userLogRepository = new DapperRepository<UserLogInfo>(dapperContext);
            _userIntegralRepository = new DapperRepository<UserIntegral>(dapperContext);
            _usertagsgetDapperRepository = new DapperRepository<UserTagsGet>(dapperContext);
            _userSalesRecordRepository = new DapperRepository<SalesRecord>(dapperContext);
            _userSalesDetailRepository = new DapperRepository<SalesDetail>(dapperContext);
            _userlistotherdataDapperRepository = new DapperRepository<UserListDetailSumData>(dapperContext);
            _usergetallactivecouponDapperRepository=new DapperRepository<GetAllActiveCoupon>(dapperContext);
            _usercouponmodelDapperRepository=new DapperRepository<UserCouponModel>(dapperContext);
        }

        /// <summary>
        /// 获取即将过生日的会员
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public ResponseModel GetBirthdayUsers(UserContext userContext, DateTime endDate)
        {
            var strSql = new StringBuilder();
            var dt = DateTime.Now;
            var currentLunar = new ChineseLunisolarCalendar();

            //最近7天生日会员

            //阳历日期
            var strDate = new StringBuilder();
            var lastDay = endDate;
            var lastMonth = lastDay.Month;
            if (lastMonth < dt.Month)
            {
                lastMonth += 12;
            }

            if (dt.Month == lastMonth)
            {
                strDate.Append(string.Format(" bdMonth={0}", dt.Month));
                strDate.Append(string.Format(" and bdDay between {0} and {1}", dt.Day, lastDay.Day));
            }
            else if (lastMonth - dt.Month == 1)
            {
                var tmpDate = dt.AddMonths(1);
                var endDay =
                    Convert.ToDateTime(string.Format("{0}-{1}-{2}", tmpDate.Year, tmpDate.Month, 1)).AddDays(-1).Day;
                strDate.Append(string.Format(" (bdMonth={0} and bdDay between {1} and {2})", dt.Month, dt.Day, endDay));
                strDate.Append(string.Format(" or (bdMonth={0} and bdDay between {1} and {2})", lastDay.Month, 1,
                    lastDay.Day));
            }
            else if (lastMonth - dt.Month == 2)
            {
                var tmpDate = dt.AddMonths(1);
                var endDay =
                    Convert.ToDateTime(string.Format("{0}-{1}-{2}", tmpDate.Year, tmpDate.Month, 1)).AddDays(-1).Day;
                var midDay = dt.AddMonths(1);
                var midDayEndDay =
                    Convert.ToDateTime(string.Format("{0}-{1}-{2}", midDay.Year, midDay.Month, 1)).AddDays(-1).Day;
                strDate.Append(string.Format(" (bdMonth={0} and bdDay between {1} and {2})", dt.Month, dt.Day, endDay));
                strDate.Append(string.Format(" or (bdMonth={0} and bdDay between {1} and {2})", midDay.Month, 1,
                    midDayEndDay));
                strDate.Append(string.Format(" or (bdMonth={0} and bdDay between {1} and {2})", lastDay.Month, 1,
                    lastDay.Day));
            }

            //农历日期
            var strLunarDate = new StringBuilder();
            var lunarDate = Helper.ConvertToLunisolar(dt);
            var lunarLastDate = Helper.ConvertToLunisolar(dt.AddDays(7));

            var nlastMonth = lunarLastDate.Month;

            if (nlastMonth < lunarDate.Month)
            {
                nlastMonth += 12;
            }
            if (lunarDate.Month == nlastMonth)
            {
                strLunarDate.Append(string.Format(" bdMonth={0}", lunarDate.Month));
                strLunarDate.Append(string.Format(" and bdDay between {0} and {1}", lunarDate.Day, lunarLastDate.Day));
            }
            else if (nlastMonth - lunarDate.Month == 1)
            {
                var calendar = new ChineseLunisolarCalendar();
                var endDay = calendar.GetDaysInMonth(lunarDate.Year, lunarDate.Month);
                strLunarDate.Append(string.Format(" (bdMonth={0} and bdDay between {1} and {2})", lunarDate.Month,
                    lunarDate.Day, endDay));
                strLunarDate.Append(string.Format(" or (bdMonth={0} and bdDay between {1} and {2})", lunarLastDate.Month,
                    1, lunarLastDate.Day));
            }
            else if (nlastMonth - lunarDate.Month == 2)
            {
                var calendar = new ChineseLunisolarCalendar();
                var endDay = calendar.GetDaysInMonth(lunarDate.Year, lunarDate.Month);
                var midDay = calendar.GetDaysInMonth(lunarDate.Year, lunarDate.Month + 1);
                strLunarDate.Append(string.Format(" (bdMonth={0} and bdDay between {1} and {2})", lunarDate.Month,
                    lunarDate.Day, endDay));
                strLunarDate.Append(string.Format(" or (bdMonth={0} and bdDay between {1} and {2})", lunarDate.Month + 1,
                    1, midDay));
                strLunarDate.Append(string.Format(" or (bdMonth={0} and bdDay between {1} and {2})", lunarLastDate.Month,
                    1, lunarLastDate.Day));
            }

            strSql.Append(
                "SELECT birthdayID,T_User_Birthday.[uid] as UserId ,T_UserInfo.uName as UserName,T_UserInfo.uPhone as Phone,T_UserInfo.uQQ,T_UserInfo.uNumber,T_UserInfo.uPortrait as PortraitUrl,(CASE IsLunar WHEN 1  THEN  0 ELSE 1 END) AS  IsLunar, bdName, bdYear, bdDate, bdMonth, bdDay,(uIntegral+uIntegralUsed) as uIntegral,T_UserInfo.uRank rankLv,'' rankName FROM T_User_Birthday left outer join T_UserInfo on T_UserInfo.[uid]=T_User_Birthday.[uid] ");
            strSql.Append(" where T_User_Birthday.[accID]=@accID and [IsLunar]=1 ");
            if (!string.IsNullOrWhiteSpace(strDate.ToString()))
            {
                strSql.Append(" and (" + strDate + ")");
            }
            strSql.Append(" order by  [IsLunar],[bdDate] asc; ");
            strSql.Append(
                "SELECT birthdayID,T_User_Birthday.[uid] as UserId,T_UserInfo.uName as UserName,T_UserInfo.uPhone as Phone,T_UserInfo.uQQ,T_UserInfo.uNumber,T_UserInfo.uPortrait as PortraitUrl,(CASE IsLunar WHEN 1  THEN  0 ELSE 1 END) AS  IsLunar, bdName, bdYear, bdDate, bdMonth, bdDay,(uIntegral+uIntegralUsed) as uIntegral,T_UserInfo.uRank rankLv,'' rankName FROM T_User_Birthday left outer join T_UserInfo on T_UserInfo.[uid]=T_User_Birthday.[uid] ");
            strSql.Append(" where T_User_Birthday.[accID]=@accID and [IsLunar]=2  ");
            if (!string.IsNullOrWhiteSpace(strLunarDate.ToString()))
            {
                strSql.Append(" and (" + strLunarDate + ")");
            }
            strSql.Append(" order by  [IsLunar],[bdDate] asc; ");
            var sqlParams = new
            {
                accID = userContext.AccId
            };
            var dapperParam = new DynamicParameters(sqlParams);
            var sqlQuery = new SqlQuery(strSql.ToString(), dapperParam);
            var birthdayUsers = _userBrthdayResultRepository.FindAll(sqlQuery);
            if (birthdayUsers != null)
            {
                foreach (var oUserBithday in birthdayUsers)
                {
                    if (oUserBithday.BdMonth == 0 || oUserBithday.BdDay == 0)
                    {
                        continue;
                    }
                    var birthdayDate =
                        Convert.ToDateTime(DateTime.Now.Year + "-" + oUserBithday.BdMonth + "-" + oUserBithday.BdDay);
                    var birthdayMonth = Convert.ToInt32(oUserBithday.BdMonth);
                    var birthdayDay = Convert.ToInt32(oUserBithday.BdDay);
                    if (oUserBithday.IsLunar)
                    {
                        //生日阴历日期
                        birthdayDate =
                            Convert.ToDateTime(currentLunar.GetYear(DateTime.Now) + "-" + oUserBithday.BdMonth + "-" +
                                               oUserBithday.BdDay);
                        var currentLunarMonth = currentLunar.GetMonth(DateTime.Now);
                        var currnetLunarDay = currentLunar.GetDayOfMonth(DateTime.Now);
                        //是否闰年
                        var isLeapMoth = currentLunar.IsLeapYear(DateTime.Now.Year);
                        var judgeDate = (birthdayMonth < currentLunarMonth) ||
                                        ((birthdayMonth == currentLunarMonth) && (birthdayDay < currnetLunarDay));
                        if (judgeDate)
                        {
                            birthdayDate = birthdayDate.AddYears(1);
                        }
                        //转为阳历日期，计算天数差
                        oUserBithday.BirthdayDayFromNow =
                            (ChineseCalendarHelper.GetDateFromLunarDate(birthdayDate, isLeapMoth) - DateTime.Now.Date)
                                .Days;
                    }
                    else
                    {
                        var judgeDate = (birthdayMonth < DateTime.Now.Month) ||
                                        ((birthdayMonth == DateTime.Now.Month) && (birthdayDay < DateTime.Now.Day));
                        if (judgeDate)
                        {
                            birthdayDate = birthdayDate.AddYears(1);
                        }
                        oUserBithday.BirthdayDayFromNow = (birthdayDate - DateTime.Now.Date).Days;
                    }

                    oUserBithday.Birthday = oUserBithday.BdMonth + "-" + oUserBithday.BdDay;
                }
            }

            //2.返回查询结果集
            return new ResponseModel
            {
                Code = birthdayUsers != null ? (int)ErrorCodeEnum.Success : (int)ErrorCodeEnum.NotFound,
                Data = birthdayUsers
            };
        }

        /// <summary>
        /// 搜索会员
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public ResponseModel SearchUser(UserContext userContext, string searchValue)
        {
            //1.从Elasticsearch搜索会员
            var userSearch = new SearchBasic();
            var oResult = new List<UserSearchModel>();
            var iMasterId = 0;
            var isBranch = userContext.AccId != userContext.MasterId;
            if (isBranch)
            {
                //搜索分店会员
                iMasterId = userContext.MasterId;
            }
            if (_isOpenElasticsearchSearch == "true")
            {
                oResult = userSearch.UserBasic(searchValue.Trim(), userContext.AccId, iMasterId);
            }
            else
            {
                //2.从Db搜索会员
                var searchDbResult = SearchUsersFromDb(searchValue, userContext, isBranch);
                if (searchDbResult != null && searchDbResult.Any())
                {
                    oResult = searchDbResult as List<UserSearchModel>;
                }
            }

            return new ResponseModel
            {
                Code = oResult != null && oResult.Any() ? (int)ErrorCodeEnum.Success : (int)ErrorCodeEnum.NotFound,
                Data = oResult
            };
        }

        /// <summary>
        /// 新增会员
        /// </summary>
        /// <returns></returns>
        public ResponseModel AddUser(UserHandle request)
        {
            bool tranFlag = false;
            var conn = _useraddDapperRepository.Connection;
            conn.Open();
            var trans = conn.BeginTransaction();
            try
            {
                var result = _useraddDapperRepository.Insert(request, trans);
                if (result)
                {
                    if (request.Birthday != null && (request.Birthday.BdDay != 0 || request.Birthday.BdMonth != 0))
                    {
                        request.Birthday.AccId = request.AccId;
                        request.Birthday.EditTime = DateTime.Now;
                        request.Birthday.UId = request.Id;
                        request.Birthday.BdName = "生日";
                        if (_userBirthdayDapperRepository.Insert(request.Birthday, trans))
                        {
                            tranFlag = true;
                        }
                    }
                    else
                    {
                        tranFlag = true;
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
                    Task.Run(() =>
                    {
                        var userSearch = new SearchBasic();
                        userSearch.UserUpdate(Mapper.Map<UserHandle, UserBasic>(request));
                    });
                }
                else
                {
                    trans.Rollback();
                }
                trans.Dispose();
                conn.Close();
            }


            //2.返回新增的支出实体
            return new ResponseModel
            {
                Code = tranFlag ? (int)ErrorCodeEnum.Success : (int)ErrorCodeEnum.Failed,
                Data = request
            };
        }

        /// <summary>
        /// 获取操作会员详情
        /// </summary>
        /// <returns></returns>
        public UserHandle GetUserHandle(int accId, int userId)
        {
            var result = _useraddDapperRepository.Find(x => x.AccId == accId && x.Id == userId);
            return result;
        }

        /// <summary>
        /// 修改会员
        /// </summary>
        /// <returns></returns>
        public ResponseModel UpdateUser(UserHandle request)
        {
            bool tranFlag = false;
            var conn = _useraddDapperRepository.Connection;
            conn.Open();
            var trans = conn.BeginTransaction();
            try
            {
                var result = _useraddDapperRepository.Update<UserHandle>(request, entity => new
                {
                    entity.UserNo,
                    entity.UserAvatar,
                    entity.UserName,
                    entity.NickId,
                    entity.UserPhone,
                    entity.UserGrade,
                    entity.UserGroup,
                    entity.Remark,
                    entity.OtherPhone,
                    entity.Email,
                    entity.QQ,
                    entity.WeChat,
                    entity.Address
                }, trans);
                if (result)
                {
                    if (request.Birthday.BirthdayId == 0)
                    {
                        request.Birthday.AccId = request.AccId;
                        request.Birthday.EditTime = DateTime.Now;
                        request.Birthday.UId = request.Id;
                        request.Birthday.BdName = "生日";
                        if (_userBirthdayDapperRepository.Insert(request.Birthday, trans))
                        {
                            tranFlag = true;
                            trans.Commit();
                        }
                        else
                        {
                            trans.Rollback();
                        }
                    }
                    else if (request.Birthday.BdDay != 0 || request.Birthday.BdMonth != 0)
                    {
                        request.Birthday.EditTime = DateTime.Now;

                        if (_userBirthdayDapperRepository.Update<UserBirthday>(request.Birthday, entity => new
                        {
                            entity.BdDay,
                            entity.BdMonth,
                            entity.BdYear,
                            entity.EditTime
                        }, trans))
                        {
                            tranFlag = true;
                            trans.Commit();
                        }
                        else
                        {
                            trans.Rollback();
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
                trans.Dispose();
                conn.Close();
            }



            //2.返回新增的支出实体
            return new ResponseModel
            {
                Code = tranFlag ? (int)ErrorCodeEnum.Success : (int)ErrorCodeEnum.Failed,
                Data = GetUserHandle(request.AccId, request.Id)
            };
        }

        /// <summary>
        /// 删除会员
        /// </summary>
        /// <returns></returns>
        public ResponseModel DeleteUser()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 校验-相同的卡号
        /// </summary>
        /// <param name="accId"></param>
        /// <param name="userNo"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool ExistSameUserNo(int accId, string userNo, int userId)
        {
            var result = _userInfoDapperRepository.Find(x => x.AccId == accId && x.UNumber == userNo && x.Uid != userId, null,
                item => new { item.Uid, item.UNumber });
            return result != null;
        }

        /// <summary>
        /// 校验-相同的手机号
        /// </summary>
        /// <param name="accId"></param>
        /// <param name="userPhone"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool ExistSameUserPhone(int accId, string userPhone, int userId)
        {
            var result = _userInfoDapperRepository.Find(x => x.AccId == accId && x.UPhone == userPhone && x.Uid != userId, null,
                item => new { item.Uid, item.UPhone });
            return result != null;
        }

        /// <summary>
        /// 校验-相同的分组名称
        /// </summary>
        /// <param name="accId"></param>
        /// <param name="groupName"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public bool ExistSameUserGroupName(int accId, string groupName, int groupId)
        {
            var result = _usergroupDapperRepository.Find(x => x.AccId == accId && x.GroupName == groupName && x.GroupId != groupId, null,
                item => new { item.GroupId, item.GroupName });
            return result != null;
        }

        /// <summary>
        ///     从Db搜索会员信息
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="userContext"></param>
        /// <param name="isBranch"></param>
        /// <returns></returns>
        public IEnumerable<UserInfoDetail> SearchUsersFromDb(string keyword, UserContext userContext, bool isBranch)
        {
            var strSql = new StringBuilder();
            if (isBranch)
            {
                //搜索分店会员
                strSql.Append(
                    "select ID into #idList from T_Account WITH (NOLOCK) where max_shop in(select max_shop from T_Account WITH (NOLOCK) where ID=@accID);");
                strSql.Append(
                    "SELECT top(20) T_UserInfo.[uid], uNumber, uName, uPhone,uIntegral,uStoreMoney FROM T_UserInfo WITH (NOLOCK)");
                strSql.Append(" where T_UserInfo.accID in (select id from #idList) ");
                strSql.Append(
                    " and ([uPY] like '%'+ @sVal + '%' or [uPinYin] like '%'+ @sVal + '%' or [uName] like '%'+ @sVal + '%' or [uPhone] like '%'+ @sVal + '%' or [uNumber] like '%'+ @sVal + '%');");
                strSql.Append(" drop table #idList;");
            }
            else
            {
                //只搜索本店会员
                strSql.Append(
                    "SELECT top(20) T_UserInfo.[uid], uNumber, uName, uPhone,uIntegral,uStoreMoney FROM T_UserInfo WITH (NOLOCK)");
                strSql.Append(" where T_UserInfo.[accID]=@accID ");
                strSql.Append(
                    " and ([uPY] like '%'+ @sVal + '%' or [uPinYin] like '%'+ @sVal + '%' or [uName] like '%'+ @sVal + '%' or [uPhone] like '%'+ @sVal + '%' or [uNumber] like '%'+ @sVal + '%');");
            }

            var sqlParams = new
            {
                accID = userContext.AccId,
                sValue = keyword
            };
            var dapperParam = new DynamicParameters(sqlParams);
            var sqlQuery = new SqlQuery(strSql.ToString(), dapperParam);
            return _userInfoDapperRepository.FindAll(sqlQuery);
        }

        /// <summary>
        /// 设置消费密码
        /// </summary>
        /// <returns></returns>
        public bool SetUserPassword(UserContext userContext, int userId, string password)
        {
            if (!CheckUserPassword(userContext, userId))
            {
                var updateUserInfoDetail = new UserInfoDetail();
                updateUserInfoDetail.Uid = userId;
                updateUserInfoDetail.UPwd = password;
                bool result = _userInfoDapperRepository.Update<UserInfoDetail>(updateUserInfoDetail,
                    item => new { item.Uid, item.UPwd });

                return result;
            }

            return false;
        }

        /// <summary>
        /// 修改消费密码
        /// </summary>
        /// <returns></returns>
        public int ChangeUserPassword(UserContext userContext, int userId, string oldPassword, string newPassword)
        {
            if (CheckUserPassword(userContext, userId))
            {
                var userInfo = _userInfoDapperRepository.Find(x => x.Uid == userId);

                if (userInfo.UPwd == oldPassword)
                {
                    var updateUserInfoDetail = new UserInfoDetail();
                    updateUserInfoDetail.Uid = userId;
                    updateUserInfoDetail.UPwd = newPassword;
                    bool result = _userInfoDapperRepository.Update<UserInfoDetail>(updateUserInfoDetail,
                        item => new { item.Uid, item.UPwd });

                    return result ? 1 : 0;
                }
                else
                {
                    return -1;//原密码不一致
                }
            }

            return 0;
        }

        /// <summary>
        /// 检查是否有设置消费密码
        /// </summary>
        /// <returns></returns>
        public bool CheckUserPassword(UserContext userContext, int userId)
        {
            var userInfo = _userInfoDapperRepository.Find(x => x.Uid == userId, null, item => new { item.Uid, item.UPwd });

            if (userInfo != null && !string.IsNullOrEmpty(userInfo.UPwd))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 历史消息记录
        /// </summary>
        /// <returns></returns>
        public List<UserMessage> GetUserMessages(UserContext userContext, int userId)
        {
            //获取某店铺会员短信
            var strSqlCoupon = @"
                    select ID into #idList from T_Account where max_shop in(select max_shop from T_Account where ID=@accID);
                    declare @userPhone varchar(20);
                    select @userPhone=uPhone from T_UserInfo where [uid]=@userID and accID in (select id from #idList);
                    select smsContent AS Content,sendtime AS SendTime
                    from T_Sms_List where accID in (select id from #idList) and smsStatus=1 and phoneNum=@userPhone order by sendtime desc;
                    drop table #idList;
                                ";

            var sqlParams = new
            {
                accID = userContext.AccId,
                userID = userId,
            };
            var dapperParam = new DynamicParameters(sqlParams);
            var sqlQuery = new SqlQuery(strSqlCoupon, dapperParam);
            var result = _userMessageDapperRepository.FindAll(sqlQuery).ToList();

            return result;
        }

        /// <summary>
        /// 获取最新会有卡号
        /// </summary>
        /// <param name="accId"></param>
        /// <returns></returns>
        public ResponseModel GetUserNewNo(int accId)
        {
            string sql = @"  DECLARE @AccNum INT
	                                ,@regusercount INT
	                                ,@uNumber NVARCHAR(50)
	                                ,@uNum NVARCHAR(50);
                                SELECT @AccNum = T_Account.reguserid
                                FROM T_Account WITH (NOLOCK)
                                WHERE T_Account.ID = @accID;
                                SET @regusercount = @AccNum;
                                SET @uNumber = cast((1000000 + cast(@AccNum AS INT) + 1) AS VARCHAR(100));
                                SET @uNumber = right(@uNumber, len(@uNumber) - 1);
                                BEGIN
	                                WHILE (
			                                EXISTS (
				                                SELECT uid
				                                FROM T_UserInfo WITH (NOLOCK)
				                                WHERE accID = @accID
					                                AND uNumber = @uNumber
				                                )
			                                )
	                                BEGIN
		                                SET @AccNum = @AccNum + 1;
		                                SET @uNumber = cast((1000000 + cast(@AccNum AS INT) + 1) AS VARCHAR(100));
		                                SET @uNumber = right(@uNumber, len(@uNumber) - 1);
	                                END

	                                IF (@AccNum <> @regusercount)
	                                BEGIN
		                                UPDATE T_Account
		                                SET reguserid = @AccNum
		                                WHERE ID = @accID
	                                END

	                                SET @uNum = @uNumber;
                                END
                                SELECT @uNum;";

            var sqlParams = new
            {
                accID = accId
            };
            var dapperParam = new DynamicParameters(sqlParams);
            var sqlQuery = new SqlQuery(sql, dapperParam);
            var userNewNo = _getRepository.Find(sqlQuery);
            return new ResponseModel
            {
                Code = !string.IsNullOrEmpty(userNewNo) ? (int)ErrorCodeEnum.Success : (int)ErrorCodeEnum.Failed,
                Data = userNewNo
            };

        }

        /// <summary>
        /// 获取会员详情
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserDetail GetUserDetail(UserContext userContext, int userId)
        {
            var userDetail = new UserDetail();

            var userInfo = _userInfoDapperRepository.Find(x => x.Uid == userId && x.AccId == userContext.AccId);
            var birthInfo = _userBirthdayDapperRepository.FindAll(x => x.AccId == userContext.AccId && x.UId == userId).ToList();
            if (userInfo == null)
            {
                return null;
            }
            else
            {
                userDetail.UserId = userId;
                userDetail.UserNo = userInfo.UNumber;
                userDetail.UserName = userInfo.UName;
                userDetail.Birthday = birthInfo.Find(x => x.BdName == "生日");
                userDetail.OtherMemorialDay = birthInfo.FindAll(x => x.BdName != "生日");
                userDetail.QQ = userInfo.Uqq;
                userDetail.WeChat = userInfo.Weixin;
                userDetail.UserPhone = userInfo.UPhone;
                userDetail.Email = userInfo.UEmail;
                userDetail.Address = userInfo.UAddress;
                userDetail.RegTime = userInfo.URegTime;
                userDetail.UserGradeId = userInfo.URank;
                //会员等级
                var userRankEntity = GetUserRank(userContext.AccId).ToList();
                var orDefault = userRankEntity.FirstOrDefault();
                if (orDefault != null)
                {
                    var firstOrDefault = userRankEntity
                               .FirstOrDefault(userRank => userRank.RankLv == userDetail.UserGradeId);
                    if (firstOrDefault != null)
                        userDetail.UserGradeName = firstOrDefault.RankName;
                    userDetail.UserGradeName = orDefault.RankName;
                }

                userDetail.UserGroupId = userInfo.UGroup;
                if (userInfo.UGroup > 0)
                {
                    userDetail.UserGroupName = _usergroupDapperRepository.Find(x => x.AccId == userContext.AccId && x.GroupId == userInfo.UGroup).GroupName;
                }
                userDetail.Remark = userInfo.URemark;
                userDetail.UserAvatar = userInfo.UPortrait;

                userDetail.UserIntegral = userInfo.UIntegral;
                userDetail.UserIntegralAll = userInfo.UIntegral + userInfo.UIntegralUsed;
                userDetail.UserLastButDate = userInfo.ULastBuyDate;

                userDetail.Tags = new List<UserTagsGet>();

                //获取某店铺会员标签
                var strSqlCoupon = @"
                    SELECT A.tk_tagId AS Id,t_tag AS TagName,t_color AS TagColor FROM dbo.T_TagKey AS A LEFT JOIN dbo.T_TagInfo AS B ON A.tk_tagId=B.id
                    WHERE A.accid=@accID AND A.tk_valId=@userID
                                ";

                var sqlParams = new
                {
                    accID = userContext.AccId,
                    userID = userId,
                };
                var dapperParam = new DynamicParameters(sqlParams);
                var sqlQuery = new SqlQuery(strSqlCoupon, dapperParam);
                var tagList = _usertagDapperRepository.FindAll(sqlQuery);
                foreach (var item in tagList)
                {
                    var tag = new UserTagsGet();
                    tag.TagId = item.Id;
                    tag.TagName = item.TagName;
                    tag.TagColor = item.TagColor;

                    userDetail.Tags.Add(tag);
                }

                return userDetail;
            }
        }

        /// <summary>
        /// 获取会员消费统计
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserStatistics GetUserStatistics(UserContext userContext, int userId)
        {
            //获取某店铺会员概要
            var strSqlCoupon = @"
                    select ID into #idList from T_Account WITH (NOLOCK) where max_shop in(select max_shop from T_Account WITH (NOLOCK) where ID=@accID)
 
                    SELECT count(saleID) AS buyTimes,SUM(saleNum) AS buyCount,SUM(RealMoney) AS consumeMoney,SUM(saleNum)/count(saleID) AS consumeAvgMoney,
                    SUM(UnpaidMoney) AS UnPaidMoney,SUM(CASE WHEN UnpaidMoney>0 THEN 1 ELSE 0 END) AS UnPaidCount
                    FROM T_SaleInfo WITH (NOLOCK) where saleKind>0 and accID in (select id from #idList) and [uid]=@userID;
 
                    drop table #idList;
                                ";

            var sqlParams = new
            {
                accID = userContext.AccId,
                userID = userId,
            };
            var dapperParam = new DynamicParameters(sqlParams);
            var sqlQuery = new SqlQuery(strSqlCoupon, dapperParam);
            var result = _userStatisticsDapperRepository.Find(sqlQuery);

            return result;
        }

        /// <summary>
        /// 更换会员头像
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="userId"></param>
        /// <param name="avatar"></param>
        /// <returns></returns>
        public bool ChangeUserAvatar(UserContext userContext, int userId, string avatar)
        {
            var updateUserInfoDetail = new UserInfoDetail();
            updateUserInfoDetail.Uid = userId;
            updateUserInfoDetail.UPortrait = avatar;
            bool result = _userInfoDapperRepository.Update<UserInfoDetail>(updateUserInfoDetail,
                item => new { item.Uid, item.UPortrait });

            return result;
        }

        /// <summary>
        /// 获取标签列表
        /// </summary>
        /// <param name="accId"></param>
        /// <returns></returns>
        public ResponseModel GetTagList(int accId)
        {
            var entity = _usertagDapperRepository.FindAll(x => x.AccId == accId).Select(x => new
            {
                x.Id,
                x.TagName,
                x.TagColor
            });
            return new ResponseModel
            {
                Code = entity.Any() ? (int)ErrorCodeEnum.Success : (int)ErrorCodeEnum.NotFound,
                Data = entity
            };
        }

        /// <summary>
        /// 给会员打标签
        /// </summary>
        /// <param name="accId"></param>
        /// <param name="operatorId"></param>
        /// <param name="userIds"></param>
        /// <param name="tagId"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public ResponseModel UpdateUserTag(int accId, int operatorId, int[] userIds, int tagId, IDbTransaction transaction)
        {
            string strWhere = string.Join(",", userIds);
            var sql = string.Format(@"SELECT uid AS u_id
                                INTO #tempUidTable
                                FROM T_UserInfo
                                WHERE uid IN ({0})
	                                AND accID = @accId;
                                INSERT INTO T_TagKey (
	                                tk_tagId
	                                ,tk_valId
	                                ,tk_type
	                                ,accid
	                                ,insertTime
	                                ,peo_Name
	                                )
                                SELECT @tagID
	                                ,u_id
	                                ,1
	                                ,@accid
	                                ,GETDATE()
	                                ,@pro_id
                                FROM #tempUidTable
                                WHERE u_id NOT IN (
		                                SELECT tk_valId
		                                FROM T_TagKey
		                                WHERE tk_tagId = @tagID
			                                AND accid = @accid
			                                AND tk_type = 1
			                                AND tk_valId IN ({1})
		                                );
                                SELECT @@rowcount
                                DROP TABLE #tempUidTable", strWhere, strWhere);
            var sqlParams = new
            {
                tagID = tagId,
                accid = accId,
                pro_id = operatorId
            };
            var dapperParam = new DynamicParameters(sqlParams);
            var sqlQuery = new SqlQuery(sql, dapperParam);
            var result = Helper.GetInt32(_getRepository.Find(sqlQuery, transaction));
            return new ResponseModel
            {
                Code = result > 0 ? (int)ErrorCodeEnum.Success : (int)ErrorCodeEnum.NotFound,
                Data = result
            };
        }

        /// <summary>
        /// 新增标签并给会员打标签
        /// </summary>
        /// <param name="request"></param>
        /// <param name="accId"></param>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public ResponseModel InsertUserTag(UserTagInfo request, int accId, int[] userIds)
        {
            bool tranFlag = false;
            var conn = _usertagDapperRepository.Connection;
            conn.Open();
            var trans = conn.BeginTransaction();
            try
            {
                var result = _usertagDapperRepository.Insert(request, trans);
                if (result)
                {
                    var updateCode = UpdateUserTag(request.AccId, request.OperatorId, userIds, request.Id, trans).Code;
                    if (updateCode == (int)ErrorCodeEnum.Success)
                    {
                        tranFlag = true;
                        trans.Commit();
                    }
                    else
                    {
                        trans.Rollback();
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
                trans.Dispose();
                conn.Close();
            }
            //2.返回新增的支出实体
            return new ResponseModel
            {
                Code = tranFlag ? (int)ErrorCodeEnum.Success : (int)ErrorCodeEnum.Failed,
                Data = request
            };
        }

        public bool ExistSameUserTagName(int accId, string tagName)
        {
            var result = _usertagDapperRepository.Find(x => x.AccId == accId && x.TagName == tagName, null,
                item => new { item.TagName });
            return result != null;
        }

        public ResponseModel DeleteUserTag(int accId, int userId, int tagId)
        {
            UserTagKey request =
                _usertagkeyDapperRepository.Find(
                    x => x.tk_tagId == tagId && x.tk_valId == userId && x.accid == accId && x.tk_type == 1);
            if (request == null)
                return new ResponseModel
                {
                    Code = (int)ErrorCodeEnum.NotFound,
                    Data = null
                };
            var result = _usertagkeyDapperRepository.Delete(request);
            return new ResponseModel
            {
                Code = result ? (int)ErrorCodeEnum.Success : (int)ErrorCodeEnum.Failed,
                Data = request
            };
        }

        public ResponseModel GetUserNickList(int accId)
        {
            List<UserNickName> basicNickNames = new List<UserNickName>
            {
                new UserNickName
                {
                    nickName = "无",
                    nickID = 0
                },
                new UserNickName
                {
                    nickName = "先生",
                    nickID = 1
                },
                new UserNickName
                {
                    nickName = "女士",
                    nickID = 2
                }
            };
            var entity = basicNickNames.Select(x => new
            {
                x.nickID,
                x.nickName
            });
            basicNickNames.AddRange(_usernickDapperRepository.FindAll(x => x.accID == accId));
            return new ResponseModel
            {
                Code = entity.Any() ? (int)ErrorCodeEnum.Success : (int)ErrorCodeEnum.NotFound,
                Data = entity
            };
        }

        public ResponseModel AddUserNick(UserNickName entity)
        {
            var result = _usernickDapperRepository.Insert(entity);
            return new ResponseModel
            {
                Code = result ? (int)ErrorCodeEnum.Success : (int)ErrorCodeEnum.Failed,
                Data = entity
            };
        }

        public bool ExistSameUserNick(int accId, string nickName)
        {
            var result = _usernickDapperRepository.Find(x => x.accID == accId && x.nickName == nickName, null,
                item => new { item.nickName });
            return result != null;
        }

        /// <summary>
        /// 获取会员分组
        /// </summary>
        /// <param name="accId"></param>
        /// <returns></returns>
        public ResponseModel GetUserGroups(int accId)
        {
            var entity = _usergroupDapperRepository.FindAll(x => x.AccId == accId).OrderBy(x => x.GroupAddTime);
            return new ResponseModel
            {
                Code = entity.Any() ? (int)ErrorCodeEnum.Success : (int)ErrorCodeEnum.NotFound,
                Data = entity
            };
        }

        /// <summary>
        /// 更新会员分组
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="groupId"></param>
        /// <param name="accId"></param>
        /// <returns></returns>
        public ResponseModel UpdateUserGroup(int[] userIds, int groupId, int accId)
        {
            var rowcount = GetUpdateUserGroupRowCount(userIds, groupId, accId);
            return new ResponseModel
            {
                Code = rowcount > 0 ? (int)ErrorCodeEnum.Success : (int)ErrorCodeEnum.NotFound,
                Data = rowcount
            };

        }

        private int GetUpdateUserGroupRowCount(int[] userIds, int groupId, int accId, IDbTransaction transaction = null)
        {
            string strWhere = string.Join(",", userIds);
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" UPDATE T_UserInfo SET ");
            strSql.Append(" uGroup =@uGroup");
            strSql.Append(" where [accID]=@accID ");
            strSql.Append(" and [uid] in (" + strWhere + ");");
            strSql.Append(" select @@rowcount");
            var sqlParams = new
            {
                accID = accId,
                uGroup = groupId
            };
            var dapperParam = new DynamicParameters(sqlParams);
            var sqlQuery = new SqlQuery(strSql.ToString(), dapperParam);
            return Helper.GetInt32(_getRepository.Find(sqlQuery, transaction));
        }

        /// <summary>
        /// 新增会员分组
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public ResponseModel InsertUserGroup(UserGroup entity, int[] userIds)
        {
            bool tranFlag = false;
            var conn = _usergroupDapperRepository.Connection;
            conn.Open();
            var trans = conn.BeginTransaction();
            try
            {
                var result = _usergroupDapperRepository.Insert(entity, trans);
                if (result)
                {
                    var rowcount = GetUpdateUserGroupRowCount(userIds, entity.GroupId, entity.AccId, trans);
                    if (rowcount > 0)
                    {
                        tranFlag = true;
                        trans.Commit();
                    }
                    else
                    {
                        trans.Rollback();
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
                trans.Dispose();
                conn.Close();
            }
            //2.返回新增的支出实体
            return new ResponseModel
            {
                Code = tranFlag ? (int)ErrorCodeEnum.Success : (int)ErrorCodeEnum.Failed,
                Data = entity
            };
        }

        /// <summary>
        /// 获取会员积分列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="userId"></param>
        /// <param name="searchType"></param>
        /// <returns></returns>
        public Entity.Api.Users.UserIntegral GetUserIntegral(UserContext userContext, int userId, UserIntegralSearchTypeEnum searchType)
        {
            //设置某店铺会员积分
            var strSqlCoupon = @"
                    --积分列表
                     select ID into #idList from T_Account where max_shop in(select max_shop from T_Account where ID=@accID)
 
                     SELECT userLogID, [uid],itemType, OriginalVal, EditVal, FinalVal, LogTime, operatorTime, operatorID, Remark,isnull(T_Account_User.name,'店员已删除') as operator into #Integral 
                     FROM T_User_LogInfo left OUTER JOIN T_Account_User ON T_Account_User.id = T_User_LogInfo.operatorID
                     where T_User_LogInfo.[uid]=@userID and logType=3 and T_User_LogInfo.accID in (select id from #idList)
                     order by LogTime desc;
 
                     SELECT userLogID, [uid],itemType, OriginalVal, EditVal AS EditValue, FinalVal AS FinalValue, LogTime, operatorTime, operatorID, Remark,operator
                     from #Integral order by LogTime desc;
 
                     drop table #idList ;
 
                     drop table #Integral;
                                ";

            var sqlParams = new
            {
                accID = userContext.AccId,
                userID = userId,
            };
            var dapperParam = new DynamicParameters(sqlParams);
            var sqlQuery = new SqlQuery(strSqlCoupon, dapperParam);
            var result = _userIntegralRepository.FindAll(sqlQuery);

            //获取用户基本信息
            var userInfo = _userInfoDapperRepository.Find(x => x.Uid == userId && x.AccId == userContext.AccId);

            var userIntegral = new Entity.Api.Users.UserIntegral();
            userIntegral.AvailableIntegral = userInfo.UIntegral;
            userIntegral.UnAvailableIntegral = userInfo.UIntegralUsed;
            userIntegral.TotalIntegral = userInfo.UIntegral + userInfo.UIntegralUsed;

            userIntegral.Items = new List<UserIntegralDetail>();
            foreach (var item in result)
            {
                var itemDetail = new UserIntegralDetail();
                itemDetail.CreateTime = item.LogTime;
                itemDetail.EditIntegral = (int)item.EditValue;
                itemDetail.FinalIntegral = (int)item.FinalValue;
                itemDetail.OperatorID = item.Operator;

                string content = string.Empty;
                switch ((UserLogItemTypeEnum)item.ItemType)
                {
                    case UserLogItemTypeEnum.Shopping:
                        content = "购买商品";
                        break;
                    case UserLogItemTypeEnum.ExchangeIntegral:
                        content = "兑换积分";
                        break;
                    case UserLogItemTypeEnum.ModifyIntegral:
                        content = "修改积分";
                        break;
                    case UserLogItemTypeEnum.ReturnGoods:
                        content = "会员退货";
                        break;
                    case UserLogItemTypeEnum.StoreTimesCard:
                        content = "计次卡充值";
                        break;
                    case UserLogItemTypeEnum.IntegralArrival:
                        content = "积分抵现";
                        break;
                    default:
                        break;
                }
                itemDetail.Content = content;

                userIntegral.Items.Add(itemDetail);
            }

            return userIntegral;
        }

        /// <summary>
        /// 会员积分设置
        /// </summary>
        /// <param name="setType"></param>
        /// <param name="userContext"></param>
        /// <param name="userId"></param>
        /// <param name="integral"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public bool SetUserIntegral(UserIntegralSetTypeEnum setType, UserContext userContext, int userId, int integral, string remark)
        {
            //获取用户基本信息
            var userInfo = _userInfoDapperRepository.Find(x => x.Uid == userId && x.AccId == userContext.AccId);

            //设置某店铺会员积分
            var strSql = @"
                    --会员积分设置
                    Declare @OriginAccID int,@orginVal int,@FinalVal int;
 
                    select ID into #idList from T_Account where max_shop in(select max_shop from T_Account where ID=@accID)

                    --兑换
                    if(@type=1)
                    begin
	                    Select @orginVal=isnull(uIntegral,0),@OriginAccID=accID 
	                    FROM T_UserInfo where [uid]=@uid and accID in (select id from #idList);
	                    if(@orginVal>=@sVal)
	                    begin
		                    Set @FinalVal=@orginVal-@sVal;
	                    end
	                    else
	                    begin
		                    Set @FinalVal=0;
	                    end
	                    Update T_UserInfo Set uIntegral=@FinalVal,uIntegralUsed=ISNULL(uIntegralUsed,0)+@sVal where [uid]=@uid and accID in (select id from #idList);
	                    select @Return=@@ROWCOUNT;
                    end

                    --修改
                    if(@type=2)
                    begin
	                    Select @orginVal=isnull(uIntegral,0),@OriginAccID=accID 
	                    FROM T_UserInfo where [uid]=@uid and accID in (select id from #idList);
	                    Update T_UserInfo Set uIntegral=@sVal where [uid]=@uid and accID in (select id from #idList);
	                    select @Return=@@ROWCOUNT;
                    end

                     --添加
                    if(@type=3)
                    begin
	                    Select @orginVal=isnull(uIntegral,0),@OriginAccID=accID 
	                    FROM T_UserInfo where [uid]=@uid and accID in (select id from #idList);
	                    Update T_UserInfo Set uIntegral= @orginVal + @sVal where [uid]=@uid and accID in (select id from #idList);
	                    select @Return=@@ROWCOUNT;
                    end

                    select @Return;

                    --会员等级
                    update T_UserInfo Set  uRank=T_User_Rank.rankLv from T_UserInfo left outer join T_User_Rank on T_UserInfo.accID=T_User_Rank.accID
                    where T_UserInfo.accID=@accID  and T_UserInfo.uid=@uid and [integralLow]<=(uIntegral+uIntegralUsed) and [integralHigh]>=(uIntegral+uIntegralUsed) and T_UserInfo.uLockRank=0
                                ";

            var sqlParams = new
            {
                accID = userContext.AccId,
                uid = userId,
                sVal = integral,
                type = (int)setType,
            };
            var dapperParam = new DynamicParameters(sqlParams);
            dapperParam.Add("Return", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var sqlQuery = new SqlQuery(strSql, dapperParam);
            _userStatisticsDapperRepository.Find(sqlQuery);
            var result = dapperParam.Get<int>("Return");

            if (result > 0)
            {
                //记录日志
                var integralLogInfo = new UserLogInfo
                {
                    AccId = userContext.AccId,
                    OriginalAccId = userContext.AccId,
                    UId = userId,
                    LogType = (int)UserLogTypeEnum.IntegralChange,
                    LogTime = DateTime.Now,
                    OperatorTime = DateTime.Now,
                    OperatorId = userContext.UserId,
                    OperatorIp = userContext.IpAddress,
                    Remark = remark,
                    Flag = string.Empty,
                    FlagStatus = 0,
                    FlagStatusTime = DateTime.Now,
                    EditMoney = 0,
                    EditMoneyType = 0,
                    AddedLgUserId = userContext.UserId,
                    BindCardId = 0
                };
                if (setType == UserIntegralSetTypeEnum.Exchange)
                {
                    integralLogInfo.ItemType = (int)UserLogItemTypeEnum.ExchangeIntegral;
                    var finalIntegral = 0;
                    if (userInfo.UIntegral >= integral)
                    {
                        finalIntegral = userInfo.UIntegral - integral;
                    }
                    integralLogInfo.OriginalVal = userInfo.UIntegral;
                    integralLogInfo.EditVal = integral;
                    integralLogInfo.FinalVal = finalIntegral;
                }

                if (setType == UserIntegralSetTypeEnum.Change)
                {
                    integralLogInfo.ItemType = (int)UserLogItemTypeEnum.ModifyIntegral;
                    integralLogInfo.OriginalVal = userInfo.UIntegral;
                    integralLogInfo.EditVal = integral;
                    integralLogInfo.FinalVal = integral;
                }

                if (setType == UserIntegralSetTypeEnum.Add)
                {
                    integralLogInfo.ItemType = (int)UserLogItemTypeEnum.ReturnGoods;
                    integralLogInfo.OriginalVal = userInfo.UIntegral;
                    integralLogInfo.EditVal = integral;
                    integralLogInfo.FinalVal = userInfo.UIntegral + integral;
                }

                _userLogRepository.Insert(integralLogInfo);
            }

            return result > 0;
        }

        /// <summary>
        /// 会员消费记录
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="userId"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public UserSale GetUserSales(UserContext userContext, int userId, string keywords)
        {
            //某店铺会员销售记录
            var strSqlSalesRecord = @"
                    --对应销售列表（临时表）
                    select * into #List from (
                    SELECT row_number() over(order by T_SaleInfo.saleTime desc) as rowNumer,T_SaleInfo.saleID as st_saleID,T_SaleInfo.saleNo as st_saleNo,
                    T_SaleInfo.isRetail as st_isRetail,T_SaleInfo.uid as st_uid,T_SaleInfo.saleTime as st_saleTime,T_SaleInfo.insertTime as st_insertTime,T_SaleInfo.saleKind as st_saleKind,
                    T_SaleInfo.saleNum as st_saleNum,T_SaleInfo.payType as st_payType,T_SaleInfo.AbleMoney as st_AbleMoney,T_SaleInfo.RealMoney as st_RealMoney,T_SaleInfo.DiffMoney as st_DiffMoney,
                    T_SaleInfo.StoreTimes as st_StoreTimes,T_SaleInfo.StoreMoney as st_StoreMoney,T_SaleInfo.CashMoney as st_CashMoney,T_SaleInfo.CardMoney as st_CardMoney,
                    T_SaleInfo.UnpaidMoney as st_UnpaidMoney,T_SaleInfo.CouponMoney,T_SaleInfo.SerialNum as st_SerialNum,T_SaleInfo.IntegralMoney,T_SaleInfo.IntegralUsed,Lists.* 
                    from T_SaleInfo (nolock) 
                    CROSS apply (select top(1) * from T_Sale_List (nolock)  where T_Sale_List.accID=@accID and T_SaleInfo.saleID=T_Sale_List.saleID and T_Sale_List.GoodsName like '%'+ @keyword + '%' order by T_Sale_List.saleListID asc)
                    Lists where T_SaleInfo.accID=@accID and T_SaleInfo.[uid]=@uid ) as List

                    --SalesRecord
                    select distinct st_saleID as saleID,st_saleNo as saleNo,st_isRetail as isRetail,st_uid as [uid],st_saleTime as saleTime,st_insertTime as insertTime,st_saleKind as saleKind,
                    st_saleNum as saleNum,st_payType as payType,st_AbleMoney as AbleMoney,st_RealMoney as RealMoney,st_DiffMoney as DiffMoney,st_StoreTimes as StoreTimes,st_StoreMoney as StoreMoney,
                    st_CashMoney as CashMoney,st_CardMoney as CardMoney,st_UnpaidMoney as UnpaidMoney,CouponMoney,st_SerialNum as SerialNum,T_UserInfo.uName,IntegralMoney,IntegralUsed
                    FROM #List left outer join T_UserInfo on T_UserInfo.[uid]=#List.st_uid order by  saleTime desc ;

                    Drop Table #List;
                                ";

            var sqlParamsSalesRecord = new
            {
                accID = userContext.AccId,
                uid = userId,
                keyword = keywords,
            };
            var dapperParamSalesRecord = new DynamicParameters(sqlParamsSalesRecord);
            var sqlQuerySalesRecord = new SqlQuery(strSqlSalesRecord, dapperParamSalesRecord);
            var salesRecord = _userSalesRecordRepository.FindAll(sqlQuerySalesRecord).ToList();

            //某店铺会员销售明细记录
            var strSqlSalesDetail = @"
                   --对应销售列表（临时表）
                 select * into #List from (
                 SELECT row_number() over(order by T_SaleInfo.saleTime desc) as rowNumer,T_SaleInfo.saleID as st_saleID,T_SaleInfo.saleNo as st_saleNo,
                 T_SaleInfo.isRetail as st_isRetail,T_SaleInfo.uid as st_uid,T_SaleInfo.saleTime as st_saleTime,T_SaleInfo.insertTime as st_insertTime,T_SaleInfo.saleKind as st_saleKind,
                 T_SaleInfo.saleNum as st_saleNum,T_SaleInfo.payType as st_payType,T_SaleInfo.AbleMoney as st_AbleMoney,T_SaleInfo.RealMoney as st_RealMoney,T_SaleInfo.DiffMoney as st_DiffMoney,
                 T_SaleInfo.StoreTimes as st_StoreTimes,T_SaleInfo.StoreMoney as st_StoreMoney,T_SaleInfo.CashMoney as st_CashMoney,T_SaleInfo.CardMoney as st_CardMoney,
                 T_SaleInfo.UnpaidMoney as st_UnpaidMoney,T_SaleInfo.CouponMoney,T_SaleInfo.SerialNum as st_SerialNum,T_SaleInfo.IntegralMoney,T_SaleInfo.IntegralUsed,Lists.* 
                 from T_SaleInfo (nolock) 
                 CROSS apply (select top(1) * from T_Sale_List (nolock)  where T_Sale_List.accID=@accID and T_SaleInfo.saleID=T_Sale_List.saleID and T_Sale_List.GoodsName like '%'+ @keyword + '%' order by T_Sale_List.saleListID asc)
                 Lists where T_SaleInfo.accID=@accID and T_SaleInfo.[uid]=@uid ) as List

                 --SalesDetail
                 SELECT saleListID, saleID, saleNo, userID, isRetail, maxClass, minClass, maxClassID, minClassID,
                 GoodsID, GoodsName, Specification, GoodsNum, Price, Discount,AbleMoney, FixMoney, RealMoney, isIntegral, CashierID, Remark, saleTime, insertTime, UpdateTime,
                 T_Account_User.name as CashierName,returnStatus,returnFlag,returnDesc,returnRemark,returnTime,SerialNum,t_GoodsPic.gPicUrl
                 FROM #List left outer join T_Account_User on T_Account_User.id=#List.CashierID left outer join t_GoodsPic on t_GoodsPic.gId=#List.GoodsID and t_GoodsPic.gPicOrder=1 and t_GoodsPic.accid=#List.accID
                 order by  saleTime desc ; 

                 Drop Table #List;
                                ";

            var sqlParamsSalesDetail = new
            {
                accID = userContext.AccId,
                uid = userId,
                keyword = keywords,
            };
            var dapperParamSalesDetail = new DynamicParameters(sqlParamsSalesDetail);
            var sqlQuerySalesDetail = new SqlQuery(strSqlSalesDetail, dapperParamSalesDetail);
            var salesDetail = _userSalesDetailRepository.FindAll(sqlQuerySalesDetail).ToList();

            var userSale = new UserSale();

            //销售记录总计
            decimal sumMoney = 0;    //合计总金额
            decimal sumUnpaid = 0;   //合计未付款总额
            double sumNumber = 0;   //合计总数量 
            userSale.Items = new List<UserSaleItem>();
            foreach (var itemSalesRecord in salesRecord)
            {
                sumMoney += Convert.ToDecimal(itemSalesRecord.RealMoney);
                sumNumber += itemSalesRecord.saleNum;
                //统计未付款总额
                sumUnpaid += itemSalesRecord.UnpaidMoney;

                var userSalesRecord = new UserSaleItem();

                //明细
                var salesDetailList = salesDetail.FindAll(x => x.saleID == itemSalesRecord.saleID);
                var index = 0;
                //销售记录分组总计
                decimal sumMoneyGroup = 0;    //合计总金额
                decimal sumNumberGroup = 0;   //合计总数量 
                userSalesRecord.Items = new List<UserSaleDetail>();
                foreach (var itemSalesDetail in salesDetailList)
                {
                    var userSalesDetail = new UserSaleDetail();

                    userSalesDetail.Subject = salesDetailList[index].GoodsName;
                    userSalesDetail.Price = salesDetailList[index].Price;
                    userSalesDetail.Quantity = salesDetailList[index].GoodsNum;
                    userSalesDetail.TotalAmount = salesDetailList[index].Price * salesDetailList[index].GoodsNum;
                    userSalesDetail.PayType = itemSalesRecord.payType;
                    userSalesDetail.PayState = itemSalesRecord.UnpaidMoney > 0 ? 1 : 0;
                    userSalesDetail.SaleTime = salesDetailList[index].saleTime;

                    userSalesRecord.Items.Add(userSalesDetail);

                    sumMoneyGroup += userSalesDetail.TotalAmount;
                    sumNumberGroup += userSalesDetail.Quantity;

                    index++;
                }

                userSalesRecord.PayType = itemSalesRecord.payType;
                userSalesRecord.PayState = itemSalesRecord.UnpaidMoney > 0 ? 1 : 0;

                if (salesDetailList.Count > 1)
                {
                    userSalesRecord.Complex = 1;

                    userSalesRecord.Subject = salesDetailList[0].GoodsName;
                    userSalesRecord.Quantity = sumMoneyGroup;
                    userSalesRecord.TotalAmount = sumMoneyGroup;
                }
                else
                {
                    userSalesRecord.Complex = 0;
                    userSalesRecord.Subject = salesDetailList[0].GoodsName;
                    userSalesRecord.Price = salesDetailList[0].Price;
                    userSalesRecord.Quantity = salesDetailList[0].GoodsNum;
                    userSalesRecord.TotalAmount = salesDetailList[0].Price * salesDetailList[0].GoodsNum;
                    userSalesRecord.SaleTime = salesDetailList[0].saleTime;
                }

                userSale.Items.Add(userSalesRecord);
            }

            return userSale;
        }

        #region 获取会员列表
        /// <summary>
        /// 获取会员列表
        /// </summary>
        /// <param name="serarchParam"></param>
        /// <param name="accId"></param>
        /// <returns></returns>
        public ResponseModel GetUserList(UserListSearch serarchParam, int accId)
        {
            var result = _isOpenElasticsearchSearch == "true" && !string.IsNullOrEmpty(serarchParam.KeyWord)
                ? GetUserListSearchResult(serarchParam, accId, GetUidsFromEs(serarchParam, accId))
                : GetUserListSearchResult(serarchParam, accId);
            if (result != null && result.Items.Any())
            {
                var userListOtherSumData = GetUserListOtherSumData(result.Items.Select(x => x.UserId), accId).ToList();
                var userTagsList = GetUserTagsGets(result.Items.Select(x => x.UserId), accId).ToList();
                var userRankEntity = GetUserRank(accId).ToList();
                var orDefault = userRankEntity.FirstOrDefault();
                var defaultRankName = "";
                if (orDefault != null)
                {
                    defaultRankName = orDefault.RankName;
                }
                using (var enumerator = result.Items.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        var item = enumerator.Current;
                        var firstOrDefault = userRankEntity
                               .FirstOrDefault(userRank => userRank.RankLv == item.UserGradeId);
                        if (firstOrDefault != null)
                            item.UserGradeName = firstOrDefault.RankName;
                        item.UserGradeName = defaultRankName;

                        foreach (var data in userListOtherSumData.Where(data => item.UserId == data.UserId))
                        {
                            item.UserStoreMoney = data.UserStoreMoney;
                            item.UserTimesCardCount = data.UserTimesCardCount;
                            item.UserUnpaidMoney = data.UserUnpaidMoney;
                        }
                        foreach (var entity in userTagsList.Where(entity => entity.UserId == item.UserId))
                        {
                            if (item.Tags != null)
                            {
                                item.Tags.Add(entity);
                            }
                            else
                            {
                                item.Tags = new List<UserTagsGet> { entity };
                            }
                        }
                    }
                }

            }

            return new ResponseModel
            {
                Code = result != null ? (int)ErrorCodeEnum.Success : (int)ErrorCodeEnum.Failed,
                Data = result
            };
        }

        /// <summary>
        /// 校验手机号是否重复
        /// </summary>
        /// <param name="phoneno"></param>
        /// <param name="accId"></param>
        /// <returns></returns>
        public ResponseModel CheckPhoneNo(string phoneno, int accId)
        {
            var result = _userInfoDapperRepository.Find(x => x.AccId == accId && x.UPhone == phoneno, null,
                item => new { item.UPhone });
            return new ResponseModel
            {
                Code = result == null ? (int)ErrorCodeEnum.Success : (int)ErrorCodeEnum.Failed,
                Data = phoneno
            };
        }

        /// <summary>
        /// 获取店铺所有可用优惠券
        /// </summary>
        /// <param name="accId"></param>
        /// <returns></returns>
        public ResponseModel GetAccountAllCouponInfo(int accId)
        {
            string sql = @"SELECT id
	                        ,couponValue
	                        ,couponStatus
	                        ,couponDesc
	                        ,maxLimitNum
                        INTO #List
                        FROM T_CouponInfo
                        WHERE accID = @accID
	                        AND couponStatus = 0
	                        AND DATEDIFF(DAY, endDate, GETDATE()) <= 0
                        SELECT groupID
	                        ,COUNT(id) AS Num
                        INTO #Summary
                        FROM T_CouponList
                        WHERE accID = @accID
	                        AND groupID IN (
		                        SELECT id
		                        FROM #List
		                        )
                        GROUP BY groupID
                        SELECT id
	                        ,couponValue
	                        ,couponStatus
	                        ,couponDesc
	                        ,(maxLimitNum - isnull(Num, 0)) AS BalanceNum
                        FROM #List
                        LEFT OUTER JOIN #Summary ON #Summary.groupID = #List.id
                        ORDER BY BalanceNum DESC
                        DROP TABLE #List;
                        DROP TABLE #Summary;";
            var sqlParams = new
            {
                accID = accId
            };
            var dapperParam = new DynamicParameters(sqlParams);
            var sqlQuery = new SqlQuery(sql, dapperParam);
            var result=_usergetallactivecouponDapperRepository.FindAll(sqlQuery);
            return new ResponseModel
            {
                Code = result != null ? (int)ErrorCodeEnum.Success : (int)ErrorCodeEnum.NotFound,
                Data = result
            };
        }

        /// <summary>
        /// 获取会员所有可用优惠券
        /// </summary>
        /// <param name="accId"></param>
        /// <param name="userid"></param>
        /// <param name="masterId"></param>
        /// <returns></returns>
        public ResponseModel GetUserCouponAll(int accId, int userid,int masterId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("  SELECT TA.id  ");
            strSql.Append("      ,groupID  ");
            strSql.Append("      ,TA.accID  ");
            strSql.Append("      ,couponID  ");
            strSql.Append("      ,TA.couponValue  ");
            strSql.Append("      ,TA.couponStatus  ");
            strSql.Append("      ,TA.createDate  ");
            strSql.Append("      ,TA.endDate  ");
            strSql.Append("      ,usedDate  ");
            strSql.Append("      ,toUserID  ");
            strSql.Append("      ,useUserID  ");
            strSql.Append("      ,flag  ");
            strSql.Append("      ,toUserName  ");
            strSql.Append("      ,useUserName  ");
            strSql.Append("      ,TB.couponClass  ");
            strSql.Append("      ,TB.couponType  ");
            strSql.Append("      ,TB.couponRule as couponRuleID ");
            strSql.Append("      ,TB.couponRuleVal  ");
            strSql.Append("      ,TB.couponStatus AS CouponInfoStatus  ");
            strSql.Append("      ,TB.couponDesc  ");
            strSql.Append("      ,TB.crossShop  ");
            strSql.Append("  FROM T_CouponList TA  ");
            strSql.Append("  INNER JOIN T_CouponInfo TB ON TB.id = TA.groupID  ");
            strSql.Append("  WHERE TA.couponStatus = 2  ");
            strSql.Append("      AND TA.toUserID = @userId  ");
            strSql.Append(CheckIsMyParentAccountUser(accId, userid)
                ? "      AND TA.accID = @accID  "
                : "      AND TB.crossShop = 1  ");
            var sqlParams = new
            {
                accid = accId,
                userId=userid
            };
            var dapperParam = new DynamicParameters(sqlParams);
            var sqlQuery = new SqlQuery(strSql.ToString(), dapperParam);
            var result= _usercouponmodelDapperRepository.FindAll(sqlQuery).ToList();
            if (result.Count>0)
            {
                foreach (var couponModel in result)
                {
                    couponModel.CouponStatusDesc = Enum.GetName(typeof (CouponEnum.CouponListStatus),
                        couponModel.CouponStatus);
                    couponModel.CouponType = Enum.GetName(typeof(CouponEnum.CouponType),
                        Helper.GetInt32(couponModel.CouponType));
                    couponModel.CouponRuleDesc = Enum.GetName(typeof(CouponEnum.CouponRule),
                        couponModel.CouponRuleId);
                    couponModel.CouponInfoStatusDesc = Enum.GetName(typeof(CouponEnum.CouponInfoStatus),
                        couponModel.CouponInfoStatus);
                }
            }
            return new ResponseModel
            {
                Code = result.Any()? (int)ErrorCodeEnum.Success : (int)ErrorCodeEnum.NotFound,
                Data = result
            };
        }

        /// <summary>
        /// 批量发送优惠券
        /// </summary>
        /// <param name="request"></param>
        /// <param name="getUserContext"></param>
        /// <returns></returns>
        public ResponseModel SendUserCoupon(UserCouponBind request, UserContext getUserContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取会员标签列表
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="accId"></param>
        /// <returns></returns>
        private IEnumerable<UserTagsGet> GetUserTagsGets(IEnumerable<int> userIds, int accId)
        {
            string strWhere = string.Join(",", userIds);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("  SELECT ");
            strSql.Append("  t_tag AS TagName");
            strSql.Append("        ,t_color AS TagColor");
            strSql.Append("        ,tk_valId AS UserId");
            strSql.Append("        ,tk_tagId AS TagId");
            strSql.Append("  FROM (");
            strSql.Append("        SELECT id");
            strSql.Append("              ,t_tag");
            strSql.Append("              ,t_color");
            strSql.Append("        FROM T_TagInfo WITH (NOLOCK) ");
            strSql.Append("        WHERE accid = @accid");
            strSql.Append("        ) a");
            strSql.Append("  INNER JOIN (");
            strSql.Append("        SELECT tk_valId");
            strSql.Append("              ,tk_tagId");
            strSql.Append("        FROM T_TagKey WITH (NOLOCK) ");
            strSql.Append("        WHERE tk_valId IN (" + strWhere + ")");
            strSql.Append("              AND accid = @accid");
            strSql.Append("              AND tk_type = 1");
            strSql.Append("        ) b ON a.id = b.tk_tagId");
            strSql.Append("  ORDER BY tk_valId DESC");
            strSql.Append("        ,id DESC");
            var sqlParams = new
            {
                accid = accId
            };
            var dapperParam = new DynamicParameters(sqlParams);
            var sqlQuery = new SqlQuery(strSql.ToString(), dapperParam);
            return _usertagsgetDapperRepository.FindAll(sqlQuery);
        }

        /// <summary>
        /// 获取会员列表其他数据
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="accId"></param>
        /// <returns></returns>
        private IEnumerable<UserListDetailSumData> GetUserListOtherSumData(IEnumerable<int> userIds, int accId)
        {
            string strWhere = string.Join(",", userIds);
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT [uid] AS UserId");
            strSql.Append("       ,uStoreMoney AS UserStoreMoney");
            strSql.Append("       ,0 AS UserTimesCardCount");
            strSql.Append("       ,0 AS UserUnpaidMoney");
            strSql.Append(" INTO #list");
            strSql.Append(" FROM T_UserInfo  WITH (NOLOCK) ");
            strSql.Append(" WHERE accID = @accid");
            strSql.Append("       AND uid IN (" + strWhere + ")");
            strSql.Append(" UPDATE #list");
            strSql.Append(" SET UserTimesCardCount = b.UserTimesCardCount");
            strSql.Append(" FROM (");
            strSql.Append("       SELECT uid AS UserId");
            strSql.Append("             ,COUNT(stID) AS UserTimesCardCount");
            strSql.Append("       FROM T_User_StoreTimes  WITH (NOLOCK) ");
            strSql.Append("       WHERE accID = @accid");
            strSql.Append("             AND uid IN (" + strWhere + ")");
            strSql.Append("       GROUP BY uid");
            strSql.Append("       ) b");
            strSql.Append(" WHERE b.UserId = #list.UserId;");
            strSql.Append(" UPDATE #list");
            strSql.Append(" SET UserUnpaidMoney = b.UserUnpaidMoney");
            strSql.Append(" FROM (");
            strSql.Append("       SELECT [uid] AS UserId");
            strSql.Append("             ,SUM(UnpaidMoney) AS UserUnpaidMoney");
            strSql.Append("       FROM T_SaleInfo  WITH (NOLOCK) ");
            strSql.Append("       WHERE accid = @accid");
            strSql.Append("             AND uid IN (" + strWhere + ")");
            strSql.Append("             AND UnpaidMoney > 0");
            strSql.Append("       GROUP BY [uid]");
            strSql.Append("       ) b");
            strSql.Append(" WHERE b.UserId = #list.UserId;");
            strSql.Append(" SELECT *");
            strSql.Append(" FROM #list;");
            strSql.Append(" DROP TABLE #list;");

            var sqlParams = new
            {
                accid = accId
            };
            var dapperParam = new DynamicParameters(sqlParams);
            var sqlQuery = new SqlQuery(strSql.ToString(), dapperParam);
            return _userlistotherdataDapperRepository.FindAll(sqlQuery);
        }

        /// <summary>
        /// 从ES获取UID列表
        /// </summary>
        /// <param name="serarchParam"></param>
        /// <param name="accId"></param>
        /// <returns></returns>
        private long[] GetUidsFromEs(UserListSearch serarchParam, int accId)
        {
            var userSearch = new SearchBasic();
            var searchModel = userSearch.UserBasic(serarchParam.KeyWord, accId, 0).FindAll(x => x.AccId == accId); ;
            if (searchModel.Count == 0)
                return null;
            var uidInts = searchModel.Select(x => x.uid).ToArray();
            return uidInts;
        }

        /// <summary>
        /// 会员列表获取主体方法
        /// </summary>
        /// <param name="serarchParam"></param>
        /// <param name="accId"></param>
        /// <param name="uidsInts"></param>
        /// <returns></returns>
        private UserListSearchResult GetUserListSearchResult(UserListSearch serarchParam, int accId, long[] uidsInts = null)
        {
            UserListSearchResult result = new UserListSearchResult();
            result.CurrentPage = Helper.GetInt32(serarchParam.CurrentPage.ToString());
            result.PageSize = Helper.GetInt32(serarchParam.PageSize.ToString());
            var rowWhereSql = new StringBuilder();
            string orderWhere = "";

            if (!string.IsNullOrEmpty(serarchParam.KeyWord))
                rowWhereSql.Append(" and (uNumber like '%'+ @keyword + '%' or uPhone like '%'+ @keyword + '%' or uName like '%'+ @keyword + '%' or uPY like '%'+ @keyword + '%' or uPinYin like '%'+ @keyword + '%')");
            if (serarchParam.UserGroup != -1)
                rowWhereSql.Append(serarchParam.UserGroup == 0 ? " and isnull( uGroup,0)=@uGroup " : " and uGroup=@uGroup");
            if (serarchParam.UserGrade != 0)
                rowWhereSql.Append(" and uRank=@uGrade");
            if (serarchParam.UserTags != null && serarchParam.UserTags.Length > 0)
                rowWhereSql.Append(" and uid in(select tk_valId from T_TagKey where tk_tagId in(" + string.Join(",", serarchParam.UserTags) + ") and accID=@accID and tk_type=1)");
            if (uidsInts != null && uidsInts.Length > 0)
                rowWhereSql.Append(" and uid in( " + string.Join(",", uidsInts) + ") ");
            if (serarchParam.UserId > 0)
                rowWhereSql.Append(" and uid =@userid");
            if (!string.IsNullOrEmpty(serarchParam.SortColumn))
            {
                var sortStr = serarchParam.SortRank ?? "";
                switch (serarchParam.SortColumn.ToLower().Trim())
                {
                    case "username":
                        orderWhere += sortStr == "desc" ? "uName desc," : "uName asc,";
                        break;
                    case "ustoremoney":
                        orderWhere += sortStr == "desc" ? "uStoreMoney desc," : "uStoreMoney asc,";
                        break;
                    case "userintegral":
                        orderWhere += sortStr == "desc" ? "isnull(uintegral,0)) desc," : "isnull(uintegral,0)) asc,";
                        break;
                    case "userno":
                        orderWhere += sortStr == "desc" ? "uNumber desc," : "uNumber asc,";
                        break;
                }
            }
            StringBuilder rowSql = new StringBuilder();
            rowSql.Append(" SELECT row_number() OVER (  ");
            rowSql.Append("                         ORDER BY " + orderWhere + " [uid] DESC  ");
            rowSql.Append("                         ) AS RowNumber  ");
            rowSql.Append("                 ,accID AS AccId  ");
            rowSql.Append("                 ,[uid] AS Uid  ");
            rowSql.Append("                 ,uNumber AS UNumber  ");
            rowSql.Append("                 ,uName AS UName  ");
            rowSql.Append("                 ,uPhone AS UPhone  ");
            rowSql.Append("                 ,isnull(uRank, 0) AS URank  ");
            rowSql.Append("                 ,isnull(uGroup, 0) AS UGroup  ");
            rowSql.Append("                 ,uIntegral AS UIntegral  ");
            rowSql.Append("                 ,uStoreMoney AS UStoreMoney  ");
            rowSql.Append("                 ,uLastBuyDate AS ULastBuyDate  ");
            rowSql.Append("                 ,isnull(uPortrait, '') AS UPortrait  ");
            rowSql.Append("         FROM T_UserInfo WITH (NOLOCK)  ");
            rowSql.Append("         WHERE accID = @accID  " + rowWhereSql);

            var strSql = new StringBuilder();
            //分页查询
            strSql.Append(" SELECT T.* ");
            strSql.Append("     ,isnull(G.gpName, '') AS UserGroupName ");
            strSql.Append(" FROM (" + rowSql + " ) AS T");
            strSql.Append(" LEFT JOIN T_User_Group G ON G.groupID = t.UGroup");
            strSql.Append(" WHERE T.RowNumber BETWEEN (@PageIndex-1)*@PageSize+1  ");
            strSql.Append("     AND @PageSize*@PageIndex ;");

            //统计金额
            strSql.Append(" SELECT @TotalNum=COUNT(1) FROM ( ");
            strSql.Append(rowSql);
            strSql.Append("   ) AS T");

            var sqlParams = new
            {
                keyword = serarchParam.KeyWord,
                uGroup = serarchParam.UserGroup,
                uGrade = serarchParam.UserGrade,
                accID = accId,
                PageIndex = serarchParam.CurrentPage ?? 1,
                PageSize = serarchParam.PageSize ?? 20,
                userid = serarchParam.UserId
            };
            var dapperParam = new DynamicParameters(sqlParams);
            dapperParam.Add("TotalNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var sqlQuery = new SqlQuery(strSql.ToString(), dapperParam);

            result.Items =
                Mapper.Map<IEnumerable<UserInfoDetail>, IEnumerable<UserListDetails>>(_userInfoDapperRepository.FindAll(sqlQuery));
            result.CurrentPage = serarchParam.CurrentPage ?? 1;
            result.PageSize = serarchParam.PageSize ?? 20;
            result.TotalSize = dapperParam.Get<int>("TotalNum");
            result.TotalPage =
                Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(result.TotalSize) / Convert.ToDecimal(result.PageSize)));

            return result;
        }

        #endregion

        /// <summary>
        /// 获取会员等级列表
        /// </summary>
        /// <param name="accId"></param>
        /// <returns></returns>
        private IEnumerable<UserRank> GetUserRank(int accId)
        {
            return _userRankRepository.FindAll(x => x.AccId == accId);
        }
        /// <summary>
        /// 检查是否是本店会员
        /// </summary>
        /// <param name="accId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        private bool CheckIsMyUser(int accId, int userid)
        {
            var result = _userInfoDapperRepository.Find(x => x.AccId == accId && x.Uid == userid , null,
                item => new { item.Uid, item.AccId });
            return result != null;
        }
        /// <summary>
        /// 检查是否是本店总店下其他店铺会员
        /// </summary>
        /// <param name="accId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        private bool CheckIsMyParentAccountUser(int accId, int userid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT ID ");
            strSql.Append(" INTO #idList ");
            strSql.Append(" FROM T_Account ");
            strSql.Append(" WHERE max_shop IN ( ");
            strSql.Append("         SELECT max_shop ");
            strSql.Append("         FROM T_Account ");
            strSql.Append("         WHERE ID = @accID ");
            strSql.Append("         ) ");
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM T_UserInfo T ");
            strSql.Append(" INNER JOIN #idList L ON T.accID = L.ID ");
            strSql.Append(" WHERE T.[uid] = @UserId ");
            strSql.Append(" DROP TABLE #idList ");
            var sqlParams = new
            {
                accID = accId,
                UserId=userid
            };
            var dapperParam = new DynamicParameters(sqlParams);
            var sqlQuery = new SqlQuery(strSql.ToString(), dapperParam);
            var result = _userInfoDapperRepository.FindAll(sqlQuery);
            return result != null;
        }
    }
}