using System.Web.Http;
using Script.I200.Application.Sales;
using Script.I200.Entity.API;
using Script.I200.Entity.Dto.Sales;
using Script.I200.Entity.Enum;

namespace Script.I200.WebAPI.Controllers
{
    /// <summary>
    /// 销售相关
    /// </summary>
    [RoutePrefix("v0/sales")]
    public class SalesController : BaseApiController
    {
        private readonly ISalesService _salesService;

        /// <summary>
        /// 初始化
        /// </summary>
        public SalesController()
        {
            _salesService = new SalesService();
        }

        /// <summary>
        /// 销售还款
        /// </summary>
        /// <returns></returns>
        [Route("{salesId}/repayment")]
        [HttpPost]
        public ResponseModel SalesRepayment(int salesId, [FromBody]SalesRepaymentDto salesRepaymentDto)
        {
            ResponseModel checkResult;
            var result = CheckRequestParamIsNull(salesRepaymentDto, out checkResult);
            if (!result)
            {
                return checkResult;
            }
            if (CheckModelParams(out checkResult)) return checkResult;

            var data = _salesService.SalesRepayment(GetUserContext(), salesId, salesRepaymentDto.money, salesRepaymentDto.remark, salesRepaymentDto.sendsms, salesRepaymentDto.operatorID);

            if (data==0)
                return Fail(ErrorCodeEnum.SalesRepaymentFailed);
            if (data == -1)
                return Fail(ErrorCodeEnum.SalesNotFind);
            if (data == -2)
                return Fail(ErrorCodeEnum.SalesNotRepayment);

            return Success(data);
        }

        /// <summary>
        /// 销售退货
        /// </summary>
        /// <returns></returns>
        [Route("{salesListId}/returngoods")]
        [HttpPost]
        public ResponseModel SalesReturnGoods(int salesListId, [FromBody]SalesReturnGoodDto salesReturnGoodDto)
        {
            ResponseModel checkResult;
            var result = CheckRequestParamIsNull(salesReturnGoodDto, out checkResult);
            if (!result)
            {
                return checkResult;
            }
            if (CheckModelParams(out checkResult)) return checkResult;

            var data = _salesService.SalesReturnGoods(GetUserContext(), salesListId, salesReturnGoodDto.remark, salesReturnGoodDto.type);

            if (data == 0)
                return Fail(ErrorCodeEnum.SalesReturnGoodFailed);
            if (data == -1)
                return Fail(ErrorCodeEnum.SalesNotFind);

            return Success(data);
        }
    }
}
