using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Dapper;
using Script.I200.Application.Account;
using Script.I200.Application.Shared;
using Script.I200.Core.Caching;
using Script.I200.Core.Config;
using Script.I200.Data;
using Script.I200.Data.MicroOrm.SqlGenerator;
using Script.I200.Entity;
using Script.I200.Entity.Api.Expenses;
using Script.I200.Entity.API;
using Script.I200.Entity.Enum;
using Script.I200.Entity.Model.Expenses;

namespace Script.I200.Application.Expenses
{
    public class ExpensesService : IExpensesService
    {
        private static readonly int catchTime = 1440;
        private static ISharedService _sharedService = new SharedService();
        private readonly IAccountService _accountService = new AccountService();
        private readonly ICacheManager _cacheManager;
        private readonly DapperRepository<PayClass> _expensesCategoryRepository;
        private readonly DapperRepository<PayRecord> _expensesRepository;

        public ExpensesService()
        {
            var dapperDbContext =
                new DapperDbContext(new SqlConnection(WebConfigSetting.Instance.I200DbConnectionString));
            _expensesRepository = new DapperRepository<PayRecord>(dapperDbContext);
            _expensesCategoryRepository = new DapperRepository<PayClass>(dapperDbContext);
            _cacheManager = new NullCache();
        }

        /// <summary>
        ///     根据Id获取支出记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public ResponseModel GetExpensesById(int id, UserContext userContext)
        {
            var result = _expensesRepository.Find(x => x.Id == id);
            return new ResponseModel
            {
                Code = result != null ? (int) ErrorCodeEnum.Success : (int) ErrorCodeEnum.NotFound,
                Data = result
            };
        }

        /// <summary>
        ///     获取支出记录列表
        /// </summary>
        /// <returns></returns>
        public ResponseModel GetExpenses(UserContext userContext, ExpensesSearchParam param)
        {
            //1.获取筛选的条件
            if (param.CurrentPage == null || param.CurrentPage < 1)
                param.CurrentPage = 1;
            if (param.PageSize == null || param.PageSize < 1)
                param.PageSize = 25;
            var result = new ExpensesSearchResult();
            var mainCategoryName = string.Empty;
            var subCategoryName = string.Empty;
            var strSql = new StringBuilder();
            var sqlWhereBuilder = new StringBuilder();
            var tRowBuilder = new StringBuilder();
            sqlWhereBuilder.Append(" WHERE t_PayRecord.ShopperId= @ShopperId");

            //日期过滤
            if (param.StartDate != null && param.EndDate != null && param.StartDate <= param.EndDate)
            {
                sqlWhereBuilder.Append(" and  t_PayRecord.PayDate >=@StartDate  and t_PayRecord.PayDate <=@EndDate ");
            }
            //支出人员过滤
            if (param.Staff != null && param.Staff > 0)
            {
                sqlWhereBuilder.Append(" and  t_PayRecord.InsertUserId =@InsertUserId ");
            }
            //支出大分类过滤
            if (param.MainCategoryId != null)
            {
                var mainCategory = _expensesCategoryRepository.Find(x => x.Id == param.MainCategoryId);
                if (mainCategory != null)
                {
                    mainCategoryName = mainCategory.Name;
                    sqlWhereBuilder.Append(" and  t_PayRecord.PayMaxType =@PayMaxType ");
                }
            }
            //支出小分类过滤
            if (param.SubCategoryId != null)
            {
                var subCategory = _expensesCategoryRepository.Find(x => x.Id == param.SubCategoryId);
                if (subCategory != null)
                {
                    subCategoryName = subCategory.Name;
                    sqlWhereBuilder.Append(" and  t_PayRecord.PayMinType =@PayMinType ");
                }
            }

            //筛选项
            tRowBuilder.Append("   SELECT ROW_NUMBER() OVER ( ");
            tRowBuilder.Append("       ORDER BY t_PayRecord.ID DESC ");
            tRowBuilder.Append("       ) AS rownumber ");
            tRowBuilder.Append(
                "     ,*,PaySum as  Amount,PayMaxType as MainCategoryName,PayName as Notes,PayMinType as SubCategoryName,ShopperId as MerchanId,insertUserName as UserName");
            tRowBuilder.Append("   FROM t_PayRecord  ");
            tRowBuilder.Append(sqlWhereBuilder);

            //分页查询
            strSql.Append(" SELECT *");
            strSql.Append(" FROM (");
            strSql.Append(tRowBuilder);
            strSql.Append("   ) AS T");
            strSql.Append(" WHERE RowNumber BETWEEN (@PageIndex-1)*@PageSize+1  ");
            strSql.Append("     AND @PageSize*@PageIndex ;");

            //统计支出总金额
            strSql.Append(" SELECT @TotalMoney= ISNULL(SUM(T.PaySum),0),@TotalNum=COUNT(1) from ( ");
            strSql.Append(tRowBuilder);
            strSql.Append("   ) AS T");

            var sqlParams = new
            {
                ShopperId = userContext.AccId,
                param.StartDate,
                param.EndDate,
                InsertUserId = param.Staff,
                PayMaxType = mainCategoryName,
                PayMinType = subCategoryName,
                param.PageSize,
                PageIndex = param.CurrentPage
            };
            var dapperParam = new DynamicParameters(sqlParams);
            dapperParam.Add("TotalMoney", dbType: DbType.Decimal, direction: ParameterDirection.Output, precision: 10, scale: 2);
            dapperParam.Add("TotalNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var sqlQuery = new SqlQuery(strSql.ToString(), dapperParam);
            result.Items = _expensesRepository.FindAll(sqlQuery);
            result.CurrentPage = param.CurrentPage ?? 1;
            result.PageSize = param.PageSize ?? 25;
            result.TotalExpensesAmount = dapperParam.Get<decimal>("TotalMoney");
            result.TotalSize = dapperParam.Get<int>("TotalNum");
            result.TotalPage =
                Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(result.TotalSize)/Convert.ToDecimal(result.PageSize)));

            //2.返回查询结果
            return new ResponseModel
            {
                Code = result.Items.Any() ? (int) ErrorCodeEnum.Success : (int) ErrorCodeEnum.NotFound,
                Data = result
            };
        }

        /// <summary>
        ///     新增支出记录
        /// </summary>
        /// <returns></returns>
        public ResponseModel AddExpenses(PayRecord request, UserContext userContext)
        {
            var mainCategory = _expensesCategoryRepository.Find(x => x.Id == request.MainCategoryId);
            var subCategory = _expensesCategoryRepository.Find(x => x.Id == request.SubCategoryId);
            request.MerchanId = userContext.AccId;
            request.OperatorId = userContext.UserId;
            var userInfo = _accountService.GetAccountUserInfoById(userContext, request.UserId);
            request.UserName = userInfo == null ? string.Empty : userInfo.Name;
            request.OperatorIp = userContext.IpAddress;
            request.MainCategoryName = mainCategory == null ? string.Empty : mainCategory.Name;
            request.SubCategoryName = subCategory == null ? string.Empty : subCategory.Name;

            //1.添加支出数据
            var result = _expensesRepository.Insert(request);

            //2.返回新增的支出实体
            return new ResponseModel
            {
                Code = result ? (int) ErrorCodeEnum.Success : (int) ErrorCodeEnum.ExpenseFailed,
                Data = request
            };
        }

        /// <summary>
        ///     更新支出记录
        /// </summary>
        /// <returns></returns>
        public ResponseModel UpdateExpenses(PayRecord request, UserContext userContext)
        {
            var payRecord = new PayRecord
            {
                Id = request.Id,
                Amount = request.Amount,
                PayDate = request.PayDate,
                Notes = request.Notes
            };

            //1.更新支出数据
            var result = _expensesRepository.Update<PayRecord>(payRecord, item => new
            {
                item.Amount,
                item.PayDate,
                item.Notes
            });

            //2.返回更新的支出数据实体
            return new ResponseModel
            {
                Code = result ? (int) ErrorCodeEnum.Success : (int) ErrorCodeEnum.ExpenseFailed,
                Data = payRecord
            };
        }

        /// <summary>
        ///     删除支出记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public ResponseModel DeleteExpenses(int id, UserContext userContext)
        {
            var request = new PayRecord
            {
                Id = id
            };

            //1.删除支出分类数据
            var result = _expensesRepository.Delete(request);

            //2.返回删除支出分类实体
            return new ResponseModel
            {
                Code = result ? (int) ErrorCodeEnum.Success : (int) ErrorCodeEnum.ExpenseFailed,
                Data = id
            };
        }

        /// <summary>
        ///     根据支出分类id获取支出分类
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public ResponseModel GetExpensesCategoryById(int id, UserContext userContext)
        {
            var selectColumns = new List<Expression<Func<PayClass, object>>>
            {
                item => item.Id,
                item => item.Name,
                item => item.SuperId,
                item => item.MerchantId
            };
            var result =
                _expensesCategoryRepository.Find(x => x.Id == id, selectColumns);
            return new ResponseModel
            {
                Code = result == null ? (int) ErrorCodeEnum.NotFound : (int) ErrorCodeEnum.Success,
                Data = result == null ? null : new {result.Id, result.Name, result.SuperId, result.MerchantId}
            };
        }

        /// <summary>
        ///     获取支出分类
        /// </summary>
        /// <returns></returns>
        public ResponseModel GetExpensesCategory(UserContext userContext)
        {
            var setCatchKey = string.Format("I200:{0}:{1}", RedisConsts.PayClassList, userContext.AccId);
            var redisCacheService = new RedisCacheService();
            var data = redisCacheService.Get<PayClassResult>(setCatchKey);
            return new ResponseModel
            {
                Code = (int) ErrorCodeEnum.Success,
                Data = data ?? GetExpensesCategoryFromDb(userContext)
            };
        }

        /// <summary>
        ///     新增支出分类
        /// </summary>
        /// <returns></returns>
        public ResponseModel AddExpensesCategory(PayClass request, UserContext userContext)
        {
            //1.添加支出分类数据
            //1.1 判断是否存在相同的分类名称
            var hasSameClass = JudgeCurrentShopHaveSameCategory(userContext, request);
            if (hasSameClass)
            {
                return new ResponseModel
                {
                    Code = (int) ErrorCodeEnum.HasSameCategoryName,
                    Data = null
                };
            }

            request.MerchantId = userContext.AccId;
            var result = _expensesCategoryRepository.Insert(request);

            //2.更新Redis缓存数据
            if (result)
            {
                UpdateRedisPayClassCatch(userContext);
            }

            //3.返回新增的支出数据实体
            return new ResponseModel
            {
                Code = result ? (int) ErrorCodeEnum.Success : (int) ErrorCodeEnum.ExpenseFailed,
                Data = request
            };
        }

        /// <summary>
        ///     更新支出分类
        /// </summary>
        /// <returns></returns>
        public ResponseModel UpdateExpenseCategory(PayClass request, UserContext userContext)
        {
            //1.更新支出分类数据
            //1.1 判断是否存在相同的分类名称
            var hasSameClass = JudgeCurrentShopHaveSameCategory(userContext, request);
            if (hasSameClass)
            {
                return new ResponseModel
                {
                    Code = (int) ErrorCodeEnum.HasSameCategoryName,
                    Data = null
                };
            }
            var payClass = new PayClass {Id = request.Id, Name = request.Name};
            var result = _expensesCategoryRepository.Update<PayClass>(payClass, pc => new {pc.Name});

            //2.更新Redis缓存数据
            if (result)
            {
                UpdateRedisPayClassCatch(userContext);
            }

            //3.返回更新的支出数据实体
            return new ResponseModel
            {
                Code = result ? (int) ErrorCodeEnum.Success : (int) ErrorCodeEnum.ExpenseFailed,
                Data = request
            };
        }

        /// <summary>
        ///     删除支出分类
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public ResponseModel DeleteExpenseCategory(int id, UserContext userContext)
        {
            //1.删除支出分类数据
            var request = new PayClass
            {
                Id = id
            };
            var result = _expensesCategoryRepository.Delete(request);

            //2.更新Redis缓存数据
            if (result)
            {
                UpdateRedisPayClassCatch(userContext);
            }

            //3.返回删除支出分类实体
            return new ResponseModel
            {
                Code = result ? (int) ErrorCodeEnum.Success : (int) ErrorCodeEnum.ExpenseFailed,
                Data = id
            };
        }

        /// <summary>
        ///     从DB获取支出分类
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        private PayClassResult GetExpensesCategoryFromDb(UserContext userContext)
        {
            var payClassResult = new PayClassResult();
            var mainCategoryList =
                _expensesCategoryRepository.FindAll(
                    x => x.SuperId == 0 && x.MerchantId == userContext.AccId)
                    .Where(x => !string.IsNullOrWhiteSpace(x.Name));
            foreach (var item in mainCategoryList)
            {
                var mainCategory = new MainCategory
                {
                    Id = item.Id,
                    Name = item.Name
                };
                payClassResult.MainCategories.Add(mainCategory);
                var mainCategoryId = item.Id;
                var subCategoryResult =
                    _expensesCategoryRepository.FindAll(
                        x => x.SuperId == mainCategoryId && x.MerchantId == userContext.AccId)
                        .Where(x => !string.IsNullOrWhiteSpace(x.Name));
                var subCategory = new SubCategory
                {
                    MainCategoryId = item.Id,
                    SubCategoryValues = new List<SubCategoryValues>()
                };
                foreach (var subCategoryValues in subCategoryResult.Select(oItem => new SubCategoryValues
                {
                    Id = oItem.Id,
                    Name = oItem.Name
                }))
                {
                    subCategory.SubCategoryValues.Add(subCategoryValues);
                }

                payClassResult.SubCategories.Add(subCategory);
            }
            return payClassResult;
        }

        /// <summary>
        ///     更新支出分类Redis缓存数据
        /// </summary>
        /// <param name="userContext"></param>
        private void UpdateRedisPayClassCatch(UserContext userContext)
        {
            var redisCacheService = new RedisCacheService();
            var currentNeedCatchData = GetExpensesCategoryFromDb(userContext);
            var setCatchKey = string.Format("I200:{0}:{1}", RedisConsts.PayClassList, userContext.AccId);
            redisCacheService.Set(setCatchKey, currentNeedCatchData, catchTime*60);
        }

        /// <summary>
        ///     判断当前的店铺是否存在名称相同的分类
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="payClass"></param>
        /// <returns></returns>
        public bool JudgeCurrentShopHaveSameCategory(UserContext userContext, PayClass payClass)
        {
            var result = false;
            if (payClass.SuperId > 0)
            {
                var searchResult =
                    _expensesCategoryRepository.Find(
                        x =>
                            x.Name == payClass.Name && x.MerchantId == userContext.AccId &&
                            x.SuperId == payClass.SuperId && x.Id!= payClass.Id);

                if (searchResult != null)
                {
                    result = true;
                }
            }
            else
            {
                var searchResult =
                    _expensesCategoryRepository.Find(
                        x =>
                            x.Name == payClass.Name && x.MerchantId == userContext.AccId && x.SuperId == 0 && x.Id!= payClass.Id);

                if (searchResult != null)
                {
                    result = true;
                }
            }

            return result;
        }
    }
}