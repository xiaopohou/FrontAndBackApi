using System;
using System.Text.RegularExpressions;
using System.Web.Http;
using Script.I200.Application.Expenses;
using Script.I200.Entity.Api.Expenses;
using Script.I200.Entity.API;
using Script.I200.Entity.Enum;
using Script.I200.Entity.Model.Expenses;

namespace Script.I200.WebAPI.Controllers
{
    /// <summary>
    ///     支出管理接口
    /// </summary>
    [RoutePrefix("v0")]
    public class ExpensesController : BaseApiController
    {
        private readonly IExpensesService _expensesService;

        /// <summary>
        ///     初始化
        /// </summary>
        public ExpensesController()
        {
            _expensesService = new ExpensesService();
        }

        /// <summary>
        ///     根据Id获取对应的支出记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("expenses/{id}")]
        [HttpGet, HttpOptions]
        public ResponseModel GetExpensesById(int id)
        {
            var result = _expensesService.GetExpensesById(id, GetUserContext());
            return FunctionReturn(result);
        }

        /// <summary>
        ///     获取支出列表
        /// </summary>
        /// <returns></returns>
        [Route("expenses")]
        [HttpGet, HttpOptions]
        public ResponseModel GetExpenses([FromUri] ExpensesSearchParam searchParam)
        {
            if (searchParam == null)
            {
                searchParam = new ExpensesSearchParam();
            }
            //校验时间参数
            var checkResult = CheckDateParams(searchParam);
            if (checkResult.Code > 0)
            {
                return checkResult;
            }

            //校验单页最大数据量不超过100条,防止通过接口单页请求大批量数据
            if (searchParam.PageSize != null && searchParam.PageSize > 100)
            {
                return FunctionReturn(new ResponseModel
                {
                    Code = (int) ErrorCodeEnum.MoreThanMaxSize
                });
            }

            if (searchParam.StartDate != null && searchParam.EndDate != null)
            {
                var newStartDate = (DateTime) searchParam.StartDate;
                searchParam.StartDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day);
                var newEndDate = (DateTime) searchParam.EndDate;
                searchParam.EndDate =
                    new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day).AddDays(1).AddSeconds(-1);
            }
            var result = _expensesService.GetExpenses(GetUserContext(), searchParam);
            return FunctionReturn(result);
        }

        /// <summary>
        ///     校验时间参数
        /// </summary>
        /// <param name="searchParam"></param>
        /// <returns></returns>
        private ResponseModel CheckDateParams(ExpensesSearchParam searchParam)
        {
            var responseResult = new ResponseModel();
            //开始时间和结束时间不能为空
            var checkOneOfDateIsNull = (searchParam.StartDate != null && searchParam.EndDate == null) ||
                                       (searchParam.StartDate == null && searchParam.EndDate != null);
            if (checkOneOfDateIsNull)
            {
                responseResult = Fail(ErrorCodeEnum.CheckStartDateOrEndDateIsNull);
            }

            //开始时间不能小于结束时间
            if (searchParam.StartDate != null && searchParam.EndDate != null &&
                searchParam.StartDate > searchParam.EndDate)
            {
                responseResult = Fail(ErrorCodeEnum.SearchDateParamInvalid);
            }

            return responseResult;
        }

        /// <summary>
        ///     新增支出记录
        /// </summary>
        /// <returns></returns>
        [Route("expenses")]
        [HttpPost, HttpOptions]
        public ResponseModel AddExpenses(PayRecord request)
        {
            //1.入参校验
            ResponseModel checkResult;
            var result = CheckRequestParamIsNull(request, out checkResult);
            if (!result)
            {
                return checkResult;
            }
            if (CheckModelParams(out checkResult)) return checkResult;
            checkResult = _expensesService.AddExpenses(request, GetUserContext());
            return FunctionReturn(checkResult);
        }

        /// <summary>
        ///     更新支出记录
        /// </summary>
        /// <returns></returns>
        [Route("expenses/{id}")]
        [HttpPut, HttpOptions]
        public ResponseModel UpdateExpenses(int id, PayRecord request)
        {
            //1.1 入参校验
            ResponseModel responseModel;
            if (BeforeRequestCheck(request, out responseModel)) return responseModel;
            request.Id = id;
            var checkResult = _expensesService.UpdateExpenses(request, GetUserContext());
            return FunctionReturn(checkResult);
        }

        /// <summary>
        ///     请求前提示参数非空和是否非法校验
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="responseModel"></param>
        /// <returns></returns>
        private bool BeforeRequestCheck<T>(T request, out ResponseModel responseModel)
        {
            ResponseModel checkResult;
            responseModel = null;
            var result = CheckRequestParamIsNull(request, out checkResult);
            if (!result)
            {
                responseModel = checkResult;
                return true;
            }
            if (!CheckModelParams(out checkResult)) return false;
            responseModel = checkResult;
            return true;
        }

        /// <summary>
        ///     删除支出记录
        /// </summary>
        /// <returns></returns>
        [Route("expenses/{id}")]
        [HttpDelete, HttpOptions]
        public ResponseModel DeleteExpenses(int id)
        {
            var result = _expensesService.DeleteExpenses(id, GetUserContext());
            return FunctionReturn(result);
        }

        /// <summary>
        ///     根据支出分类id获取支出分类
        /// </summary>
        /// <returns></returns>
        [Route("expenses-categories/{id}")]
        [HttpGet, HttpOptions]
        public ResponseModel GetExpensesCategoty(int id)
        {
            var list = _expensesService.GetExpensesCategoryById(id, GetUserContext());
            return FunctionReturn(list);
        }

        /// <summary>
        ///     获取支出分类
        /// </summary>
        /// <returns></returns>
        [Route("expenses-categories")]
        [HttpGet, HttpOptions]
        public ResponseModel GetExpensesCategoty()
        {
            var list = _expensesService.GetExpensesCategory(GetUserContext());
            return list.Data == null ? Fail(ErrorCodeEnum.NotFound) : Success(list);
        }

        /// <summary>
        ///     新增支出分类
        /// </summary>
        /// <returns></returns>
        [Route("expenses-categories")]
        [HttpPost, HttpOptions]
        public ResponseModel AddExpensesCategory(PayClass request)
        {
            //入参校验
            ResponseModel responseModel;
            if (BeforeRequestCheck(request, out responseModel)) return responseModel;

            //校验名称是否含有特殊字符
            var regExp = new Regex(@"^[a-zA-Z0-9_\-\u4e00-\u9fa5]+$");
            if (!CheckNameIsValid(request.Name, out responseModel, regExp)) return responseModel;
            var result = _expensesService.AddExpensesCategory(request, GetUserContext());
            return FunctionReturn(result);
        }

        /// <summary>
        ///     更新支出分类
        /// </summary>
        /// <returns></returns>
        [Route("expenses-categories/{id}")]
        [HttpPut, HttpOptions]
        public ResponseModel UpdateExpensesCategory(int id, PayClass request)
        {
            //入参校验
            ResponseModel responseModel;
            if (BeforeRequestCheck(request, out responseModel)) return responseModel;
            request.Id = id;
            var result = _expensesService.UpdateExpenseCategory(request, GetUserContext());
            return FunctionReturn(result);
        }

        /// <summary>
        ///     删除支出分类
        /// </summary>
        /// <returns></returns>
        [Route("expenses-categories/{id}")]
        [HttpDelete, HttpOptions]
        public ResponseModel DeleteExpensesCategory(int id)
        {
            var result = _expensesService.DeleteExpenseCategory(id, GetUserContext());
            return FunctionReturn(result);
        }
    }
}