using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Script.I200.Application.Users;
using Script.I200.Core.Config;
using Script.I200.Data;
using Script.I200.Data.MicroOrm.SqlGenerator;
using Script.I200.Entity.Api.Modules;
using Script.I200.Entity.API;
using Script.I200.Entity.Enum;
using Script.I200.Entity.Model.Account;
using AccountUserModule = Script.I200.Entity.Api.Modules.AccountUserModule;
using Module = Script.I200.Entity.Model.Modules.Module;

namespace Script.I200.Application.Modules
{
    public class ModuleService :IModuleService
    {
        private readonly DapperRepository<Module> _moduleDapperRepository;
        private readonly DapperRepository<AccountUserModule> _moduleAccountUserDapperRepository;

        private readonly DapperRepository<Entity.Model.Modules.AccountUserModule> _moduleUserDapperRepository;

        private readonly DapperRepository<Business> _accountBusinessDapperRepository;

        private readonly IUserQueryService _userQueryService = new UserQueryService();

        /// <summary>
        /// 初始化
        /// </summary>
        public ModuleService()
        {
            var dapperContext = new DapperDbContext(new SqlConnection(WebConfigSetting.Instance.I200DbConnectionString));

            _moduleDapperRepository = new DapperRepository<Module>(dapperContext);
            _moduleAccountUserDapperRepository = new DapperRepository<AccountUserModule>(dapperContext);
            _moduleUserDapperRepository = new DapperRepository<Entity.Model.Modules.AccountUserModule>(dapperContext);

            _accountBusinessDapperRepository = new DapperRepository<Business>(dapperContext);
        }

        /// <summary>
        /// 获取管理模块列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public ModuleList GetModules(UserContext userContext)
        {
            var modulesList = new ModuleList
            {
                Free = new List<Entity.Api.Modules.Module>(),
                Paid = new List<Entity.Api.Modules.Module>()
            };

            var dataList = _moduleDapperRepository.FindAll(x => x.Enable == 1).OrderBy(x => x.Sort);
            foreach (var item in dataList)
            {
                var apiModule = new Entity.Api.Modules.Module
                {
                    Id = item.Id,
                    Name = item.Name,
                    Icon = item.Icon,
                    Url = item.Url,
                    ParentMenuKey = item.ParentMenuKey
                };

                if (item.IsPaid==1)
                {
                    modulesList.Paid.Add(apiModule);
                }
                else
                {
                    modulesList.Free.Add(apiModule);
                }
            }

            return modulesList;
        }

        /// <summary>
        /// 获取当前用户店铺管理模块列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public IEnumerable<AccountUserModule> GetModulesByAccount(UserContext userContext)
        {
            var strSqlCoupon = @"--初始化功能
                                IF NOT EXISTS (SELECT 1 FROM dbo.T_Module_AccountUser WHERE AccountId=@AccountId AND AccountUserId=@AccountUserId AND ModuleId IN(1,2,3,4))
                                BEGIN
	                                INSERT INTO dbo.T_Module_AccountUser(AccountId,AccountUserId,ModuleId,Enable,Sort) VALUES(@AccountId,@AccountUserId,1,1,0);
	                                INSERT INTO dbo.T_Module_AccountUser(AccountId,AccountUserId,ModuleId,Enable,Sort) VALUES(@AccountId,@AccountUserId,2,1,0);
	                                INSERT INTO dbo.T_Module_AccountUser(AccountId,AccountUserId,ModuleId,Enable,Sort) VALUES(@AccountId,@AccountUserId,3,1,0);
	                                INSERT INTO dbo.T_Module_AccountUser(AccountId,AccountUserId,ModuleId,Enable,Sort) VALUES(@AccountId,@AccountUserId,4,1,0);
	                                --INSERT INTO dbo.T_Module_AccountUser(AccountId,AccountUserId,ModuleId,Enable,Sort) VALUES(@AccountId,@AccountUserId,10,1,0);
                                END

                                SELECT B.Id,B.Name,A.Sort,B.IsPaid,B.Icon,B.Url,B.ParentMenuKey FROM dbo.T_Module_AccountUser AS A LEFT JOIN dbo.T_Module AS B ON A.ModuleId=B.Id
                                WHERE A.Enable=1 AND B.Enable=1
                                AND AccountId=@AccountId AND AccountUserId=@AccountUserId
                                ORDER BY A.Sort,A.CreateTime ASC";

            var powerWeight = Convert.ToInt32(AccountUserPowerEnum.UserPowerV2Enum.ExpenditureManage);
            //根据店员是否有支出管理权限
            if (!_userQueryService.IsPower(powerWeight, userContext.Powers, userContext.Role))
            {
                strSqlCoupon = @"--初始化功能
                                IF NOT EXISTS (SELECT 1 FROM dbo.T_Module_AccountUser WHERE AccountId=@AccountId AND AccountUserId=@AccountUserId AND ModuleId IN(1,2,3,4))
                                BEGIN
	                                INSERT INTO dbo.T_Module_AccountUser(AccountId,AccountUserId,ModuleId,Enable,Sort) VALUES(@AccountId,@AccountUserId,1,1,0);
	                                INSERT INTO dbo.T_Module_AccountUser(AccountId,AccountUserId,ModuleId,Enable,Sort) VALUES(@AccountId,@AccountUserId,2,1,0);
	                                INSERT INTO dbo.T_Module_AccountUser(AccountId,AccountUserId,ModuleId,Enable,Sort) VALUES(@AccountId,@AccountUserId,3,1,0);
	                                --INSERT INTO dbo.T_Module_AccountUser(AccountId,AccountUserId,ModuleId,Enable,Sort) VALUES(@AccountId,@AccountUserId,4,1,0);
	                                --INSERT INTO dbo.T_Module_AccountUser(AccountId,AccountUserId,ModuleId,Enable,Sort) VALUES(@AccountId,@AccountUserId,10,1,0);
                                END

                                SELECT B.Id,B.Name,A.Sort,B.IsPaid,B.Icon,B.Url,B.ParentMenuKey FROM dbo.T_Module_AccountUser AS A LEFT JOIN dbo.T_Module AS B ON A.ModuleId=B.Id
                                WHERE A.Enable=1 AND B.Enable=1 AND A.ModuleId!=4
                                AND AccountId=@AccountId AND AccountUserId=@AccountUserId
                                ORDER BY A.Sort,A.CreateTime ASC";
            }

            var sqlParams = new
            {
                AccountId = userContext.AccId,
                AccountUserId = userContext.UserId,
            };
            var dapperParam = new DynamicParameters(sqlParams);
            var sqlQuery = new SqlQuery(strSqlCoupon, dapperParam);
            var modulesList = _moduleAccountUserDapperRepository.FindAll(sqlQuery);

            return modulesList;
        }

        /// <summary>
        /// 呈现某个功能模块
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="modulesId"></param>
        /// <returns></returns>
        public int ShowAccountModule(UserContext userContext, int modulesId)
        {
            //安装失败
            int result = 0;

            var accountBusiness = _accountBusinessDapperRepository.Find(x => x.accountid == userContext.AccId);
            var module = _moduleDapperRepository.Find(x => x.Id == modulesId);

            //免费用户
            if (accountBusiness.aotjb == 0 || accountBusiness.aotjb == 1)
            {
                if (module != null && module.Enable == 1 && module.IsPaid == 1)
                {
                    //付费功能，无法安装
                    result = -2;
                }
                else
                {
                    //可以安装
                    result = 1;
                }
            }
            else
            {
                //可以安装
                result = 1;
            }


            if (result == 1)
            {
                var userModules = _moduleUserDapperRepository.FindAll(x => x.Enable == 1
                                                                    && x.AccountId == userContext.AccId
                                                                    && x.AccountUserId == userContext.UserId);

                if (userModules.Count() >= 8)
                {
                    //安装超过8个模块
                    result = -1;
                }
                else
                {
                    var userModuleExist = _moduleUserDapperRepository.Find(x => x.ModuleId == modulesId
                                                                       && x.AccountId == userContext.AccId
                                                                       && x.AccountUserId == userContext.UserId);
                    if (userModuleExist == null)
                    {
                        var userModuleInsert = new Entity.Model.Modules.AccountUserModule();
                        userModuleInsert.AccountId = userContext.AccId;
                        userModuleInsert.AccountUserId = userContext.UserId;
                        userModuleInsert.ModuleId = modulesId;
                        userModuleInsert.Enable = 1;
                        userModuleInsert.CreateTime = DateTime.Now;
                        userModuleInsert.UpdateTime = DateTime.Now;

                        _moduleUserDapperRepository.Insert(userModuleInsert);

                        userModuleExist = _moduleUserDapperRepository.Find(x => x.ModuleId == modulesId
                                                                            && x.AccountId == userContext.AccId
                                                                            && x.AccountUserId == userContext.UserId);
                    }
                    var userModule = new Entity.Model.Modules.AccountUserModule
                    {
                        Id = userModuleExist.Id,
                        Enable = 1,
                    };

                    bool flag = _moduleUserDapperRepository.Update<Entity.Model.Modules.AccountUserModule>(userModule, item => new { item.Enable });

                    result = flag ? 1 : 0;
                }
            }

            return result;
        }

        /// <summary>
        /// 隐藏某个功能模块
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="modulesId"></param>
        /// <returns></returns>
        public int HideAccountModule(UserContext userContext, int modulesId)
        {
            int result = 0;

            //限制无法移除模块
            if (modulesId == 1 || modulesId == 2 || modulesId == 3)
            {
                return 0;
            }

            var userModules = _moduleUserDapperRepository.FindAll(x => x.Enable == 1
                                                                    && x.AccountId == userContext.AccId
                                                                    && x.AccountUserId == userContext.UserId);

            if (userModules.Count() <= 4)
            {
                result = -1;
            }
            else
            {
                var userModuleExist = _moduleUserDapperRepository.Find(x => x.ModuleId == modulesId
                                                                            && x.AccountId == userContext.AccId
                                                                            && x.AccountUserId == userContext.UserId);
                if (userModuleExist == null)
                {
                    return 0;
                }

                var userModule = new Entity.Model.Modules.AccountUserModule
                {
                    Id = userModuleExist.Id,
                    Enable = 0,
                };

                bool flag = _moduleUserDapperRepository.Update<Entity.Model.Modules.AccountUserModule>(userModule, item => new {item.Enable});

                result = flag ? 1 : 0;
            }

            return result;
        }
    }
}
