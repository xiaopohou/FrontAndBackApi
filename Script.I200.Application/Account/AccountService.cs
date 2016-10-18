using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using Script.I200.Application.Accountbook;
using Script.I200.Application.Expenses;
using Script.I200.Application.Users;
using Script.I200.Core.Config;
using Script.I200.Data;
using Script.I200.Data.MicroOrm.SqlGenerator;
using Script.I200.Entity;
using Script.I200.Entity.Api.Account;
using Script.I200.Entity.Api.Accountbook;
using Script.I200.Entity.Api.Expenses;
using Script.I200.Entity.Api.Users;
using Script.I200.Entity.API;
using Script.I200.Entity.Enum;
using Script.I200.Entity.Model.Account;
using Script.I200.Entity.Model.Log;
using Script.I200.Entity.Model.Order;
using Script.I200.Entity.Model.Sales;
using Script.I200.Entity.Model.User;

namespace Script.I200.Application.Account
{
    /// <summary>
    ///     店铺账户相关接口（B端用户）
    /// </summary>
    public class AccountService : IAccountService
    {
        private readonly IAccountbookService _accountbookService = new AccountbookService();
        private readonly DapperRepository<Business> _accountBusinessDapperRepository;
        private readonly DapperRepository<Entity.Model.Account.Account> _accountDapperRepository;
        private readonly DapperRepository<AccountExpand> _accountExpandDapperRepository;
        private readonly DapperRepository<AccountLogo> _accountLogoDapperRepository;
        private readonly DapperRepository<AccountUser> _accountUserDapperRepository;
        private readonly DapperRepository<LogInfo> _accountUserLogInfoDapperRepository;
        private readonly DapperRepository<string> _getRepository;
        private readonly DapperRepository<T_Order_CouponList> _orderGouponDapperRepository;
        private readonly DapperRepository<SalesInfo> _salesDapperRepository;
        private readonly DapperRepository<AccountOverviewDetail> _salesReportDapperRepository;
        private readonly IUserService _userService = new UserService();
        private readonly DapperRepository<UserInfoDetail> _userInfoDapperRepository;
        /// <summary>
        ///     初始化
        /// </summary>
        public AccountService()
        {
            var dapperContext = new DapperDbContext(new SqlConnection(WebConfigSetting.Instance.I200DbConnectionString));

            _accountUserDapperRepository = new DapperRepository<AccountUser>(dapperContext);

            _accountDapperRepository = new DapperRepository<Entity.Model.Account.Account>(dapperContext);
            _accountBusinessDapperRepository = new DapperRepository<Business>(dapperContext);
            _accountExpandDapperRepository = new DapperRepository<AccountExpand>(dapperContext);
            _accountLogoDapperRepository = new DapperRepository<AccountLogo>(dapperContext);

            _orderGouponDapperRepository = new DapperRepository<T_Order_CouponList>(dapperContext);
            _salesDapperRepository = new DapperRepository<SalesInfo>(dapperContext);

            _salesReportDapperRepository = new DapperRepository<AccountOverviewDetail>(dapperContext);

            _accountUserLogInfoDapperRepository = new DapperRepository<LogInfo>(dapperContext);

            _getRepository = new DapperRepository<string>(dapperContext);

            _userInfoDapperRepository= new DapperRepository<UserInfoDetail>(dapperContext);
        }

        /// <summary>
        ///     根据店铺人员的Id获取详细信息
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public AccountUser GetAccountUserInfoById(UserContext userContext, int id)
        {
            return _accountUserDapperRepository.Find(x => x.Id == id && x.AccountId == userContext.AccId);
        }

        /// <summary>
        ///     获取店铺基本信息
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public AccountBaseInfo GetAccountBaseInfo(UserContext userContext)
        {
            var accountInfo = _accountDapperRepository.Find(x => x.Id == userContext.AccId);
            var accountBusiness = _accountBusinessDapperRepository.Find(x => x.accountid == userContext.AccId);
            var accountUser =
                _accountUserDapperRepository.Find(x => x.Id == userContext.UserId && x.AccountId == userContext.AccId);
            var accountExpand = _accountExpandDapperRepository.Find(x => x.AccountId == userContext.AccId);
            var accountLogoModel = _accountLogoDapperRepository.Find(x => x.ShopperId == userContext.AccId);

            //头像
            var accountLogo = "/v/assets/images/logo.png";
            var accountAvatar = "/SetUp/assets/img/pic-default.png";
            if (accountLogoModel != null && !string.IsNullOrEmpty(accountLogoModel.ImgUrl))
                accountLogo = "/upload/logo/" + accountLogoModel.ImgUrl;
            if (!string.IsNullOrEmpty(accountExpand.t_Avatar))
                accountAvatar = "http://img.i200.cn" + accountExpand.t_Avatar + "!small";

            //礼金券
            var strSqlCoupon = @"SELECT @ActiveCouponNum=COUNT(1) FROM T_Order_CouponList
                                LEFT JOIN T_Order_CouponInfo ON T_Order_CouponList.groupId=T_Order_CouponInfo.id
                                WHERE T_Order_CouponList.toAccId=@AccountId
                                AND (T_Order_CouponList.couponStatus=0 OR T_Order_CouponList.couponStatus=2)
                                AND (T_Order_CouponInfo.couponType=1 OR T_Order_CouponInfo.couponType=2)";

            var sqlParams = new
            {
                AccountId = userContext.AccId
            };
            var dapperParam = new DynamicParameters(sqlParams);
            dapperParam.Add("ActiveCouponNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var sqlQuery = new SqlQuery(strSqlCoupon, dapperParam);
            _orderGouponDapperRepository.FindAll(sqlQuery);
            var accountActiveCoupon = dapperParam.Get<int>("ActiveCouponNum");

            //今日积分
            var strSqlIntegral = @"SELECT @TodayIntegral=SUM(CAST(EditVal AS INT)) FROM T_LogInfo
                                WHERE accID=@AccountId
                                AND Keys='Integral'
                                AND CreatTime>convert(varchar(10),getdate(),120)
                                IF @TodayIntegral IS NULL SET @TodayIntegral=0;";

            var sqlParams2 = new
            {
                AccountId = userContext.AccId
            };
            var dapperParam2 = new DynamicParameters(sqlParams2);
            dapperParam2.Add("TodayIntegral", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var sqlQuery2 = new SqlQuery(strSqlIntegral, dapperParam2);
            _accountUserLogInfoDapperRepository.FindAll(sqlQuery2);
            var accountIntegralToday = dapperParam2.Get<int>("TodayIntegral");


            //资金账户
            decimal accountTotalMoney = 0;
            var responesModelAccountbook = _accountbookService.GetWithdrawingBalance(userContext);
            if (responesModelAccountbook.Code == 200)
            {
                var responseData = (AccountBalanceResponse) responesModelAccountbook.Data;
                accountTotalMoney = responseData.TotalBalance;
            }

            var accountBaseInfo = new AccountBaseInfo();

            accountBaseInfo.accountId = accountInfo.Id;
            accountBaseInfo.accountName = accountInfo.CompanyName;
            accountBaseInfo.accountContactPhone = accountUser.PhoneNumber;
            if (accountBusiness.aotjb != 3 && accountBusiness.aotjb != 5)
            {
                accountBaseInfo.accountLicense = 1;
            }
            else
            {
                accountBaseInfo.accountLicense = accountBusiness.aotjb;
            }
            accountBaseInfo.accountLicenseName =
                StatusDictionary.DicAccountLicenseNameStatus[accountBaseInfo.accountLicense];
            accountBaseInfo.accountEnterprise = string.IsNullOrEmpty(accountInfo.EnterpriseId) ? 0 : 1;
            accountBaseInfo.accountLogo = accountLogo;
            accountBaseInfo.accountAvatar = accountAvatar;
            accountBaseInfo.accountIntegral = accountBusiness.integral;
            accountBaseInfo.accountIntegralToday = accountIntegralToday;
            accountBaseInfo.accountActiveCoupon = accountActiveCoupon;
            accountBaseInfo.accountTotalMoney = accountTotalMoney;

            accountBaseInfo.accountLoginUser = new accountLoginUser();
            accountBaseInfo.accountLoginUser.userId = accountUser.Id;
            accountBaseInfo.accountLoginUser.userName = accountUser.Name;
            accountBaseInfo.accountLoginUser.role = accountUser.Grade == "管理员" ? 1 : 0;

            return accountBaseInfo;
        }

        /// <summary>
        ///     获取店铺今日运营概况
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public AccountOverview GetAccountOverViewToday(UserContext userContext)
        {
            var accountBusiness = _accountBusinessDapperRepository.Find(x => x.accountid == userContext.AccId);

            //销售情况
            var strSqlCoupon =
                @"SELECT @SalesMoney=cast(sum(RealMoney) as decimal(18, 2)),@GoodsSalesCount=SUM(saleNum) FROM dbo.T_SaleInfo 
                                WHERE accID=@AccountId AND saleTime BETWEEN @sDate AND @eDate;
                                IF @SalesMoney IS NULL SET @SalesMoney=0;
                                IF @GoodsSalesCount IS NULL SET @GoodsSalesCount=0;
                                ";

            var sDate = DateTime.Now.ToShortDateString();
            var eDate = DateTime.Now.ToShortDateString();
            sDate = sDate + " 00:00:00";
            eDate = eDate + " 23:59:59";
            var sqlParams = new
            {
                AccountId = userContext.AccId,
                sDate,
                eDate
            };
            var dapperParam = new DynamicParameters(sqlParams);
            dapperParam.Add("SalesMoney", dbType: DbType.Decimal, direction: ParameterDirection.Output);
            dapperParam.Add("GoodsSalesCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var sqlQuery = new SqlQuery(strSqlCoupon, dapperParam);
            _salesDapperRepository.FindAll(sqlQuery);
            var salesMoney = dapperParam.Get<decimal>("SalesMoney");
            var goodsSalesCount = dapperParam.Get<int>("GoodsSalesCount");

            //库存警告
            var strSqlGoodsWarning = new StringBuilder();
            strSqlGoodsWarning.Append("SELECT @GoodsWarning=COUNT( DISTINCT a.gid) ");
            strSqlGoodsWarning.Append("from T_GoodsInfo ");
            strSqlGoodsWarning.Append("a left join T_Goods_Sku b ");
            strSqlGoodsWarning.Append("on a.gid=b.gid ");
            strSqlGoodsWarning.Append("where a.accID=@accId and  ISNULL(a.isDown,0)=0  ");
            strSqlGoodsWarning.Append(
                "and ((a.IsExtend=1 and ( b.gsQuantity>=b.LimitUpper or b.gsQuantity<=b.LimitLower)) ");
            strSqlGoodsWarning.Append(
                "or (ISNULL(a.IsExtend,0)=0 and (a.gQuantity>=a.LimitUpper or a.gQuantity<=a.LimitLower))) ");
            var sqlParamsGoodsWarning = new
            {
                accId = userContext.AccId
            };
            var dapperParamGoodsWarning = new DynamicParameters(sqlParamsGoodsWarning);
            dapperParamGoodsWarning.Add("GoodsWarning", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var sqlQueryGoodsWarning = new SqlQuery(strSqlGoodsWarning.ToString(), dapperParamGoodsWarning);
            _salesDapperRepository.FindAll(sqlQueryGoodsWarning);
            var goodsWarning = dapperParamGoodsWarning.Get<int>("GoodsWarning");


            //店铺会员生日
            var userBirthdayCount = 0;
            var responseUser = _userService.GetBirthdayUsers(userContext,
                DateTime.Parse(DateTime.Now.ToShortDateString()));
            if (responseUser.Code == (int) ErrorCodeEnum.Success)
            {
                userBirthdayCount = ((IEnumerable<BirthdayUsersResult>) responseUser.Data).Count();
            }

            var expenseService = new ExpensesService();
            var accountOverview = new AccountOverview
            {
                SalesMoney = salesMoney,
                GoodsSalesCount = goodsSalesCount,
                UserBirthdayCount = userBirthdayCount,
                GoodsStockCount = goodsWarning,
                SmsCount = accountBusiness.dxunity
            };
            var searchParams = new ExpensesSearchParam
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0),
                EndDate = DateTime.Now
            };

            //本月支出总额
            var expenseResult = expenseService.GetExpenses(userContext, searchParams);
            if (expenseResult.Code == (int) ErrorCodeEnum.Success)
            {
                accountOverview.ThisMonthExpense = ((ExpensesSearchResult) expenseResult.Data).TotalExpensesAmount;
            }
            //资金账户可提现余额
            accountOverview.TotalMoeny =
                ((AccountBalanceResponse) _accountbookService.GetWithdrawingBalance(userContext).Data).TotalBalance;

            //会员总数
            accountOverview.UsersNum = _userInfoDapperRepository.FindAll(x => x.AccId == userContext.AccId).Count();

            //今日手机橱窗订单总数
            var ordersStrSql = new StringBuilder();
            var searchDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            ordersStrSql.Append(
                "SELECT COUNT(bid) as TotalOrdersNum FROM T_Goods_Booking WHERE accid=@accId AND payType=1 AND bState IN (4,6) AND bInsertTime BETWEEN @StartDate AND @EndDate;");
            var sqlParamMobileOrdersNum = new
            {
                accId = userContext.AccId,
                StartDate = searchDate,
                EndDate = searchDate.AddDays(1).AddSeconds(-1)
            };
            var dapperParamMobileOrdersNum = new DynamicParameters(sqlParamMobileOrdersNum);
            var sqlQueryMobileOrdersNum = new SqlQuery(ordersStrSql.ToString(), dapperParamMobileOrdersNum);
            accountOverview.TodayMobileOrdersNum = Convert.ToInt32(_getRepository.FindAll(sqlQueryMobileOrdersNum).FirstOrDefault());

            return accountOverview;
        }

        /// <summary>
        ///     获取店铺运营概况明细
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<AccountOverviewDetail> GetAccountOverViewDetail(UserContext userContext, int type)
        {
            var sDate = DateTime.Now.ToShortDateString();
            var eDate = DateTime.Now.ToShortDateString();
            var dateAdd = 0;
            var salesReportList = new List<AccountOverviewDetail>();
            switch (type)
            {
                case (int) DateSpanEnum.Yesterday:
                    sDate = DateTime.Now.AddDays(-1).ToShortDateString();
                    eDate = sDate;
                    break;
                case (int) DateSpanEnum.ThisWeek:
                    dateAdd = 7;
                    sDate = DateTime.Now.AddDays(-6).ToShortDateString();
                    break;
                case (int) DateSpanEnum.ThisMonth:
                    dateAdd = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                    sDate = DateTime.Now.ToString("yyyy-MM") + "-01";
                    eDate = DateTime.Parse(sDate).AddMonths(1).AddDays(-1).ToShortDateString();
                    break;
            }

            sDate = sDate + " 00:00:00";
            eDate = eDate + " 23:59:59";

            var strSqlCoupon = new StringBuilder();
            strSqlCoupon.Append("SELECT CONVERT(VARCHAR(10),saleTime,120) AS Date, ");
            strSqlCoupon.Append("SUM(RealMoney) AS salesMoney, ");
            strSqlCoupon.Append("SUM(AbleMoney-RealMoney) AS salesGrossProfit ");
            strSqlCoupon.Append("FROM dbo.T_Sale_List AS A ");
            strSqlCoupon.Append("WHERE accID=@AccountId ");
            strSqlCoupon.Append("AND saleTime BETWEEN @sDate AND @eDate ");
            strSqlCoupon.Append("GROUP BY CONVERT(VARCHAR(10),saleTime,120) ");
            var sqlParams = new
            {
                AccountId = userContext.AccId,
                sDate,
                eDate
            };
            var dapperParam = new DynamicParameters(sqlParams);
            var sqlQuery = new SqlQuery(strSqlCoupon.ToString(), dapperParam);
            var overviewDetailList = _salesReportDapperRepository.FindAll(sqlQuery);
            if (type != 0 && type != -1)
            {
                var dealDate = Convert.ToDateTime(sDate);
                for (var i = 0; i < dateAdd; i++)
                {
                    var tempDate = new DateTime(dealDate.Year, dealDate.Month, dealDate.Day, 0, 0, 0).AddDays(i);
                    if (overviewDetailList == null)
                    {
                        var tempAccountOverviewDetail = new AccountOverviewDetail
                        {
                            Date = tempDate,
                            SalesMoney = 0,
                            SalesGrossProfit = 0
                        };
                        salesReportList.Add(tempAccountOverviewDetail);
                    }
                    else
                    {
                        var accountOverviewDetails = overviewDetailList as AccountOverviewDetail[] ??
                                                     overviewDetailList.ToArray();
                        var overviewDetails = accountOverviewDetails.ToArray();
                        var tempAccountOverviewDetail = new AccountOverviewDetail
                        {
                            Date = tempDate,
                            SalesMoney =
                                overviewDetails.Where(x => x.Date == tempDate)
                                    .Select(x => x.SalesMoney)
                                    .FirstOrDefault(),
                            SalesGrossProfit =
                                overviewDetails.Where(x => x.Date == tempDate)
                                    .Select(x => x.SalesGrossProfit)
                                    .FirstOrDefault()
                        };
                        salesReportList.Add(tempAccountOverviewDetail);
                    }
                }
            }
            else
            {
                salesReportList = overviewDetailList.ToList();
            }

            return salesReportList;
        }
    }
}