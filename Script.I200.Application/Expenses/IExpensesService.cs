using Script.I200.Entity.Api.Expenses;
using Script.I200.Entity.API;
using Script.I200.Entity.Model.Expenses;

namespace Script.I200.Application.Expenses
{
    public interface IExpensesService
    {
        /// <summary>
        /// 根据Id获取支出记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userContext"></param>
        /// <returns></returns>
        ResponseModel GetExpensesById(int id, UserContext userContext);

        /// <summary>
        /// 获取支出记录列表
        /// </summary>
        /// <returns></returns>
        ResponseModel GetExpenses(UserContext userContext, ExpensesSearchParam searchParam);

        /// <summary>
        /// 新增支出记录
        /// </summary>
        /// <returns></returns>
        ResponseModel AddExpenses(PayRecord request, UserContext userContext);

        /// <summary>
        /// 更新支出记录
        /// </summary>
        /// <returns></returns>
        ResponseModel UpdateExpenses(PayRecord request, UserContext userContext);

        /// <summary>
        /// 删除支出记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userContext"></param>
        /// <returns></returns>
        ResponseModel DeleteExpenses(int id, UserContext userContext);


        /// <summary>
        /// 根据支出分类Id获取支出分类
        /// </summary>
        /// <returns></returns>
        ResponseModel GetExpensesCategoryById(int id, UserContext userContext);

        /// <summary>
        /// 获取支出分类
        /// </summary>
        /// <returns></returns>
        ResponseModel GetExpensesCategory(UserContext userContext);

        /// <summary>
        /// 新增支出分类
        /// </summary>
        /// <returns></returns>
        ResponseModel AddExpensesCategory(PayClass request, UserContext userContext);

        /// <summary>
        /// 更新支出分类
        /// </summary>
        /// <returns></returns>
        ResponseModel UpdateExpenseCategory(PayClass request, UserContext userContext);

        /// <summary>
        /// 删除支出分类
        /// </summary>
        /// <returns></returns>
        ResponseModel DeleteExpenseCategory(int id, UserContext userContext);
    }
}