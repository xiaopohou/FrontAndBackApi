using Script.I200.Entity.API;

namespace Script.I200.Application.Sales
{
    /// <summary>
    /// 货品销售
    /// </summary>
    public interface ISalesService
    {
        /// <summary>
        /// 销售还款
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="salesId"></param>
        /// <param name="money"></param>
        /// <param name="remark"></param>
        /// <param name="sendsms"></param>
        /// <returns></returns>
        int SalesRepayment(UserContext userContext, int salesId, decimal money, string remark, int sendsms, int operatorID);

        /// <summary>
        /// 销售退货
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="salesId"></param>
        /// <param name="remark"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        int SalesReturnGoods(UserContext userContext, int salesListId, string remark, int type);
    }
}
