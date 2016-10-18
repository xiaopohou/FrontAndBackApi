using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Script.I200.Core.Config;
using Script.I200.Data;
using Script.I200.Entity.Api.Settings;
using Script.I200.Entity.API;
using Script.I200.Entity.Enum;
using Script.I200.Entity.Model.Settings;
using Script.I200.FrameWork;

namespace Script.I200.Application.Settings
{
    /// <summary>
    ///     设置相关接口
    /// </summary>
    public class SettingsService : ISettingsService
    {
        private readonly DapperRepository<SmsTemplates> _smsTemplateDapperRepository;

        /// <summary>
        ///     初始化
        /// </summary>
        public SettingsService()
        {
            var dapperDbContext =
                new DapperDbContext(new SqlConnection(WebConfigSetting.Instance.I200DbConnectionString));
            _smsTemplateDapperRepository = new DapperRepository<SmsTemplates>(dapperDbContext);
        }

        /// <summary>
        ///     获取店铺短信模板列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public ResponseModel GetAccountSmsTeampltes(UserContext userContext, int categoryId)
        {
            //获取当前短信的主题（生日祝福、计次卡充值等）
            var smsStamp = categoryId.ToEnumDescriptionString(typeof (AccountSmsTeampltesEnum));
            var selectColumns = new List<Expression<Func<SmsTemplates, object>>>
            {
                item => item.Id,
                item => item.Category,
                item => item.Template
            };
            var smsTemplates =
                _smsTemplateDapperRepository.FindAll(
                    x => x.AccountId == userContext.AccId && x.Category == smsStamp, selectColumns)
                    .Select(x => new {x.Id, x.Category, x.Template});

            if (!smsTemplates.Any())
            {
                var systemSmsTempaltes = new SystemSmsTempaltes();
                var smsTemplatesList = new List<SmsTemplates>();
                var systemTemplateResult = systemSmsTempaltes.GetSystemSmsTemplate()[6].FirstOrDefault();
                var smsTemplateDefault = new SmsTemplates
                {
                    Id = categoryId.ToString(),
                    Category = smsStamp,
                    Template = systemTemplateResult
                };
                smsTemplatesList.Add(smsTemplateDefault);
                return new ResponseModel
                {
                    Code = (int) ErrorCodeEnum.Success,
                    Data = smsTemplatesList
                };
            }

            return new ResponseModel
            {
                Code = (int) ErrorCodeEnum.Success,
                Data = smsTemplates
            };
        }

        /// <summary>
        ///     获取系统短信模板列表
        /// </summary>
        /// <returns></returns>
        public ResponseModel GetSystemSmsTeampltes(UserContext userContext, int categoryId)
        {
            var systemSmsTempaltes = new SystemSmsTempaltes();
            var smsJsonTemplates = systemSmsTempaltes.GetSystemSmsTemplate()[categoryId];
            return new ResponseModel
            {
                Code = smsJsonTemplates != null ? (int) ErrorCodeEnum.Success : (int) ErrorCodeEnum.SmsSettingFailed,
                Data = smsJsonTemplates
            };
        }
    }
}