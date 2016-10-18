using System;
using Script.I200.Entity.API;
using Script.I200.Entity.Enum;

namespace Script.I200.Application.Shared
{
    public class MockSharedService : ISharedService
    {
        public ResponseModel SendVerificationCode(UserContext userContext, int context, int channel)
        {
            return new ResponseModel();
        }

        public ResponseModel GetVerificationAccount(UserContext userContext)
        {
            return new ResponseModel();
        }

        public ResponseModel CheckVerificationCode(UserContext userContext, int context, int channel, int code,
            string phone)
        {
            return new ResponseModel();
        }

        public ResponseModel GetWithdrawingExtraInfo(UserContext userContext)
        {
            return new ResponseModel();
        }

        public ResponseModel GetAccountUserInfo(UserContext userContext)
        {
            throw new NotImplementedException();
        }

        public ResponseModel ExportExcelData(UserContext userContext)
        {
            throw new NotImplementedException();
        }

        public bool SendSms(UserContext userContext)
        {
            throw new NotImplementedException();
        }

        public ResponseModel SendVerificationCode(UserContext userContext, int context, CaptchaPhoneEmailEnum typeEnum, string phoneOrEmail)
        {
            return new ResponseModel();
        }
    }
}