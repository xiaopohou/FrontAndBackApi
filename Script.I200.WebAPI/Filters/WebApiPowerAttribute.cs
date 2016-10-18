﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Script.I200.Application.Users;
using Script.I200.Entity.API;
using Script.I200.Entity.Enum;
using Script.I200.WebAPIFramework.WebAPI;

namespace Script.I200.WebAPI.Filters
{
    public class WebApiPowerAttribute : ActionFilterAttribute
    {
        private readonly IUserQueryService _userQueryService = new UserQueryService();

        //Action方法执行之前执行此方法
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var request = actionContext.Request.Headers;
            var responseModel = new ResponseModel();
            var oToken = request.SingleOrDefault(x => x.Key.ToLower() == "token");
            var oAppkey = request.SingleOrDefault(x => x.Key.ToLower() == "appkey");
            var oUserId = request.SingleOrDefault(x => x.Key.ToLower() == "userid");
            var strToken = oToken.Key == null ? string.Empty : oToken.Value.FirstOrDefault();
            var strAppkey = oAppkey.Key == null ? string.Empty : oAppkey.Value.FirstOrDefault();
            var strUserId = oUserId.Key == null ? string.Empty : oUserId.Value.FirstOrDefault();
            var strIpAddress = actionContext.Request.GetClientIpAddress();

            //获取用户的相关信息
            var user = _userQueryService.GetUserContext(strToken, strAppkey, strUserId);
            var userContext = new UserContext();
            if (user != null)
            {
                if (user.AccId == 0)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden,
                        responseModel);
                    actionContext.Response.Headers.Add("BadRequest", "Token值无效");
                    actionContext.Response.StatusCode = HttpStatusCode.BadRequest;
                    responseModel.Code = 400;
                    responseModel.Message = "Token值无效";
                    responseModel.Data = null;
                    return;
                }

                var powerWeight = Convert.ToInt32(AccountUserPowerEnum.UserPowerV2Enum.SalesView);
                //根据店员是否有查看销售列表权限判断是否可以查看提现列表和收单列表以及资金明细列表权限
                if (!_userQueryService.IsPower(powerWeight, user.Powers, user.Role))
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden,
                        responseModel);
                    actionContext.Response.Headers.Add("Forbidden", "当前无权限操作");
                    actionContext.Response.StatusCode = HttpStatusCode.Forbidden;
                    responseModel.Code = 403;
                    responseModel.Message = "当前无权限操作";
                    responseModel.Data = null;
                    return;
                }
                userContext.AccId = user.AccId;
                userContext.UserId = user.UserId;
                userContext.Operater = user.UserId;
                userContext.OperaterName = user.OperaterName;
                userContext.IpAddress = strIpAddress;
                userContext.Token = user.Token;
                userContext.AppKey = user.AppKey;
                userContext.Role = user.Role;
                actionContext.Request.Properties["userInfoContext"] = userContext;
            }
        }
    }
}