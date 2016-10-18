using System;
using System.Data.SqlClient;
using System.Text;
using Dapper;
using Script.I200.Application.Logging;
using Script.I200.Core;
using Script.I200.Core.Caching;
using Script.I200.Core.Config;
using Script.I200.Data;
using Script.I200.Data.MicroOrm.Enums;
using Script.I200.Data.MicroOrm.SqlGenerator;
using Script.I200.Entity;
using Script.I200.Entity.Api.Account;
using Script.I200.Entity.API;
using Script.I200.Entity.Enum;
using Script.I200.Entity.Model.Account;
using Script.I200.Entity.Model.User;
using Script.I200.FrameWork;

namespace Script.I200.Application.Users
{
    public class UserQueryService : IUserQueryService
    {
        private readonly DapperRepository<AccountUser> _accoutUserRepository;
        private readonly DapperRepository<ApiToken> _apiTokenRepository;
        private readonly ILogger _logger;
        private readonly DapperRepository<UserPhoneEmail> _userPhoneDataRepository;
        private readonly DapperRepository<BaseToken> _webTokenRepository;
        private readonly DapperRepository<AccountVersionBasic> _accountVersionRepository;
        //private ICacheManager _cacheManager;

        public UserQueryService()
        {
            _logger = new NLogger();

            var dapperDbContext =
                new DapperDbContext(new SqlConnection(WebConfigSetting.Instance.I200DbConnectionString));

            _userPhoneDataRepository =
                new DapperRepository<UserPhoneEmail>(dapperDbContext);

            _webTokenRepository =
                new DapperRepository<BaseToken>(dapperDbContext);
            _apiTokenRepository =
                new DapperRepository<ApiToken>(dapperDbContext);

            _accoutUserRepository =
                new DapperRepository<AccountUser>(dapperDbContext);

            _accountVersionRepository =
                new DapperRepository<AccountVersionBasic>(dapperDbContext);
            // _cacheManager = new NullCache();
        }

        public UserPhoneEmail GetShopAdministratorPhoneEmail(int accId, bool isEncrypt = false)
        {
            var phoneEmail =
                _userPhoneDataRepository.Find(
                    u => u.AccountId == accId && u.Grade == AccountUserGradeConsts.Administrator);
            if (phoneEmail == null)
            {
                throw new YuanbeiException("获取店铺管理员信息失败，未找到对应信息");
            }

            if (isEncrypt)
            {
                var email = phoneEmail.Email;
                var phone = phoneEmail.Phone;

                var i = email.IndexOf("@", StringComparison.Ordinal);
                if (i < 3)
                {
                    phoneEmail.Email = email.Substring(0, i) + "******" + email.Substring(i);
                }
                else
                {
                    phoneEmail.Email = email.Substring(0, 3) + "******" + email.Substring(i);
                }

                if (phone.Trim().Length == 13)
                {
                    phoneEmail.Phone = phone.Substring(0, 3) + "******" + phone.Substring(9);
                }
            }

            return phoneEmail;
        }

        /// <summary>
        ///     根据Token和Appkey获取用户的相关信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="appKey"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserContext GetUserContext(string token, string appKey, string userId)
        {
            var redisCacheService = new RedisCacheService();
            var expireDate = DateTime.Now.AddHours(-30);
            var intUserId = 0;

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException("token cannot be null");
            }

            //if (!int.TryParse(userId, out intUserId))
            //{
            //    throw new ArgumentException("userId is not valid");
            //}

            var tokenRedisKey = string.IsNullOrWhiteSpace(appKey)
                ? RedisConsts.WebUserTokenKey + userId
                : RedisConsts.AppUserTokenKey + userId;

            var tokenResult = redisCacheService.Get<UserContext>(tokenRedisKey);
            if (tokenResult != null)
            {
                if (CheckSearchToken(token, tokenResult)) return null;
                _logger.Debug(string.Format("Get app user context from redis cache. {0},{1},{2} ", token, appKey,
                    tokenRedisKey));
                return tokenResult;
            }

            _webTokenRepository.Find(
                      t => t.Token == token && t.CreateTime > expireDate && t.CreateTime < DateTime.Now);

            var retToken = string.IsNullOrWhiteSpace(appKey)
                ? _webTokenRepository.Find(
                    t => t.Token == token && t.CreateTime > expireDate && t.CreateTime < DateTime.Now, null,
                    t => t.CreateTime, OrderDirection.DESC)
                : _apiTokenRepository.Find(
                    t => t.Token == token && t.CreateTime > expireDate && t.CreateTime < DateTime.Now, null,
                    t => t.CreateTime, OrderDirection.DESC);

            if (retToken == null)
                return null;

            if (retToken.Token != token)
            {
                throw new YuanbeiHttpRequestException((int)ErrorCodeEnum.TokenIsExpired, "token不正确");
            }

            var strSql = new StringBuilder();
            strSql.Append(
                "SELECT m.accountid AS AccId,m.id AS UserId,m.name AS OperaterName,m.grade AS Grade,UserPower AS UserPower, ");
            strSql.Append(
                "n.max_shop AS masterId ,m.PhoneNumber AS PhoneNumber  FROM dbo.T_Account_User m left JOIN  dbo.T_Account n ON  ");
            strSql.Append("m.accountid =n.ID WHERE m.accountid=@accountId AND m.id=@userId ");
            var sqlParams = new
            {
                accountId = retToken.AccId,
                userId = retToken.UserId
            };
            var dapperParam = new DynamicParameters(sqlParams);
            var sqlQuery = new SqlQuery(strSql.ToString(), dapperParam);
            var accountUser = _accoutUserRepository.Find(sqlQuery);
            if (accountUser == null)
                throw new YuanbeiHttpRequestException((int)ErrorCodeEnum.TokenIsExpired, "没有找到用户相关信息");


            return new UserContext
            {
                AccId = retToken.AccId,
                MasterId = accountUser.MasterId,
                UserId = retToken.UserId,
                PhoneNumber = accountUser.PhoneNumber,
                CompanyName = accountUser.CompanyName,
                OperaterName = retToken.UserName,
                Token = retToken.Token,
                AppKey = retToken.AppKey,
                Role = accountUser.Grade == AccountUserGradeConsts.Administrator ? 1 : 0,
                Powers = accountUser.UserPower
            };
        }

        /// <summary>
        ///     是否有权限 2的N次方权限
        /// </summary>
        /// <param name="sysPower">系统权值</param>
        /// <param name="userPower">用户权值</param>
        /// <param name="userRole">用户角色</param>
        /// <returns></returns>
        public bool IsPower(int sysPower, int userPower, int userRole)
        {
            if (userRole == 1)
            {
                return true;
            }

            return (userPower & sysPower) == sysPower;
        }

        private AccountVersionBasic GetAccountVersionBasic(int accId)
        {
            var strSql = @"   SELECT m.aotjb AS [Version]
	                                ,m.endtime AS VersionExpirationTime
	                                ,ISNULL(m.BetaAdvance, 0) AS BetaAdvance
	                                ,ISNULL(n.IndustryName, '') AS IndustryName
                                FROM T_Business m
                                LEFT JOIN T_IndustryShop_Setting n ON m.accountid = n.AccId
                                WHERE m.accountid = @accountId";
            var sqlParams = new
            {
                accountId = accId
            };
            var dapperParam = new DynamicParameters(sqlParams);
            var sqlQuery = new SqlQuery(strSql.ToString(), dapperParam);

            var accountVersion = _accountVersionRepository.Find(sqlQuery);
            return accountVersion;
        }

        public AccountVersion GetAccountVersion(int accId)
        {
            var redisCacheService = new RedisCacheService();
            var accVersionRedisKey = RedisConsts.AccountVersion + accId.ToString();
            var accVersionResult = redisCacheService.Get<AccountVersion>(accVersionRedisKey);
            if (accVersionResult != null)
            {
                _logger.Debug(string.Format("Get app account version from redis cache. {0},{1} ", accId, accVersionRedisKey));
                return accVersionResult;
            }

            var accVersion = GetAccountVersionBasic(accId);
            var entity = new AccountVersion
            {
                Version = accVersion.Version,
                VersionExpirationTime = accVersion.VersionExpirationTime
            };
            entity.VersionName = entity.Version.ToEnumDescriptionString(typeof (AccountVersionEnum));
            //行业版
            if (entity.Version == (int) AccountVersionEnum.Industry)
                entity.SubVersionName = accVersion.IndustryName;
            //试用高级版
            if (entity.Version == (int)AccountVersionEnum.Advanced && accVersion.BetaAdvance == (int)BetaAdvanceEnum.UsingBetaAdvance)
                entity.SubVersionName = Convert.ToInt32(BetaAdvanceEnum.UsingBetaAdvance).ToEnumDescriptionString(typeof(AccountVersionEnum));
            //todo:终身高级版
            //todo:购买后更新redis
            var expireSeconds = 108000;
            redisCacheService.Set(accVersionRedisKey, entity, expireSeconds);
            return entity;
        }

        /// <summary>
        ///     校验查询的Token是否和从Redis里面拿到的Token一致
        /// </summary>
        /// <param name="token"></param>
        /// <param name="webTokenResult"></param>
        /// <returns></returns>
        private static bool CheckSearchToken(string token, UserContext webTokenResult)
        {
            return webTokenResult.Token != token;
        }
    }
}