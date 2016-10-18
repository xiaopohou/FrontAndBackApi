using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Script.I200.Core.Config;
using Script.I200.Data;
using Script.I200.Data.MicroOrm.SqlGenerator;
using Script.I200.Entity.API;
using Script.I200.Entity.Model.Account;
using Script.I200.Entity.Model.Log;
using Script.I200.Entity.Model.Mood;

namespace Script.I200.Application.Mood
{
    public class MoodService :IMoodService
    {
        private readonly DapperRepository<LogSign> _accountUserLogSignDapperRepository;
        private readonly DapperRepository<LogInfo> _accountUserLogInfoDapperRepository;
        private readonly DapperRepository<Business> _accountBusinessDapperRepository;
        private readonly DapperRepository<MoodInfo> _accountMoodDapperRepository;
       

        /// <summary>
        /// 初始化
        /// </summary>
        public MoodService()
        {
            var dapperContext = new DapperDbContext(new SqlConnection(WebConfigSetting.Instance.I200DbConnectionString));

            _accountUserLogSignDapperRepository = new DapperRepository<LogSign>(dapperContext);
            _accountBusinessDapperRepository = new DapperRepository<Business>(dapperContext);
            _accountUserLogInfoDapperRepository = new DapperRepository<LogInfo>(dapperContext);
            _accountMoodDapperRepository = new DapperRepository<MoodInfo>(dapperContext);
        }

        /// <summary>
        /// 店铺店员签到
        /// </summary>
        /// <param name="userContext"></param>
        public int AddSignin(UserContext userContext, int signType)
        {
            //TODO:signOption 签到随机产生的激励名句

            var accountBusiness = _accountBusinessDapperRepository.Find(x => x.accountid == userContext.AccId);

            int addSmsNum = 0; //默认短信条数
            int addRegStorage = 0; //默认会员空间
            int addIntegral = 2; //默认积分倍数

            int daygap = -1;
            int serialDay = 0;
            //获取连续登陆次数，今天是否签到
            var strSqlCoupon =
                @"SELECT top(1) @DayGap=DATEDIFF(day,CreatTime,GETDATE()), @SerialDay=SerialDay FROM T_LogSign 
                                WHERE accountID=@AccountId AND CreatTime BETWEEN DATEADD(day,-3,GETDATE()) AND GETDATE()
                                ORDER BY CreatTime DESC;
                                IF @DayGap IS NULL SET @DayGap=-1;
                                IF @SerialDay IS NULL SET @SerialDay=0;
                                ";

            var sqlParams = new
            {
                AccountId = userContext.AccId,
            };
            var dapperParam = new DynamicParameters(sqlParams);
            dapperParam.Add("DayGap", dbType: DbType.Int32, direction: ParameterDirection.Output);
            dapperParam.Add("SerialDay", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var sqlQuery = new SqlQuery(strSqlCoupon, dapperParam);
            _accountUserLogSignDapperRepository.FindAll(sqlQuery);
            daygap = dapperParam.Get<int>("DayGap");
            serialDay = dapperParam.Get<int>("SerialDay");


            if (daygap == 0)
            {
                //今天已签到
                return -1;
            }
            else
            {
                serialDay = serialDay + 1;
                if (signType == 1)
                    addIntegral = IntegralStragetyForWeb(serialDay, addIntegral);
                if (signType == 3)
                    addIntegral = IntegralStragetyForApp(serialDay, addIntegral);
            }

            //新增日志
            LogSign logSignInsert = new LogSign();
            logSignInsert.accountID = userContext.AccId.ToString();
            logSignInsert.CreatTime = DateTime.Now;
            logSignInsert.SignType = signType; //signType：1=网页；3=APP
            logSignInsert.SerialDay = serialDay;
            logSignInsert.Add_Storage = addRegStorage;
            logSignInsert.Add_Sms = addSmsNum;
            logSignInsert.Add_Integral = addIntegral;
            _accountUserLogSignDapperRepository.Insert(logSignInsert);

            //更新业务表积分
            Business businessUpdate = new Business();
            businessUpdate.accountid = userContext.AccId;
            businessUpdate.gsreguser = accountBusiness.gsreguser + addRegStorage;
            businessUpdate.integral = accountBusiness.integral + addIntegral;
            _accountBusinessDapperRepository.Update<Business>(businessUpdate,
                item => new {item.gsreguser, item.integral});

            //LogInfo
            LogInfo logInfoInsert = new LogInfo();
            logInfoInsert.accID = userContext.AccId;
            logInfoInsert.LogType = 7;
            logInfoInsert.Keys = "Integral";
            logInfoInsert.OriginalVal = accountBusiness.integral.ToString();
            logInfoInsert.EditVal = addIntegral.ToString();
            logInfoInsert.FinialVal = businessUpdate.integral.ToString();
            logInfoInsert.CreatTime = DateTime.Now;
            logInfoInsert.ReMark = "每日签到";
            logInfoInsert.Flags = "0";
            _accountUserLogInfoDapperRepository.Insert(logInfoInsert);

            return 1;
        }

        /// <summary>
        /// 提交一条心情
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="pic"></param>
        /// <param name="mood"></param>
        /// <returns></returns>
        public int AddMood(UserContext userContext, string pic, string mood)
        {
            //今天是否提交
            var dataList = _accountMoodDapperRepository.FindAll(
                x => x.AccountID == userContext.AccId && x.Time >= DateTime.Parse(DateTime.Now.ToShortDateString()));

            if (dataList.Any())
            {
                return -1;
            }

            MoodInfo moodInfo = new MoodInfo();
            moodInfo.AccountID = userContext.AccId;
            moodInfo.MoodPic = pic;
            moodInfo.Mood = mood;
            moodInfo.Time = DateTime.Now;

            bool flag = _accountMoodDapperRepository.Insert(moodInfo);

            return flag ? 1 : 0;
        }

        #region 辅助方法

        /// <summary>
        /// 连续登陆积分计算(WEB)
        /// </summary>
        /// <param name="serialDay"></param>
        /// <param name="baseIntrgral"></param>
        /// <returns></returns>
        private int IntegralStragetyForWeb(int serialDay, int baseIntrgral)
        {
            int result;

            int sdTmp = serialDay > 5 ? 5 : serialDay;

            result = sdTmp * baseIntrgral;

            return result;
        }

        /// <summary>
        /// 连续登陆积分计算(APP)
        /// </summary>
        /// <param name="serialDay"></param>
        /// <param name="baseIntrgral"></param>
        /// <returns></returns>
        private int IntegralStragetyForApp(int serialDay, int baseIntrgral)
        {
            int result;

            serialDay = serialDay > 5 ? 5 : serialDay;

            if (serialDay == 4)
                result = 7;
            else
                result = serialDay * 2;

            return result * 2;
        }

        #endregion
    }
}
