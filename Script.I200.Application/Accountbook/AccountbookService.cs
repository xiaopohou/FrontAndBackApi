#region

using System;
using System.Collections.Generic;
using System.Linq;
using Script.I200.Application.AuthCode;
using Script.I200.Application.BasicData;
using Script.I200.Application.Proxy;
using Script.I200.Application.Shared;
using Script.I200.Application.Users;
using Script.I200.Entity.Api.Accountbook;
using Script.I200.Entity.API;
using Script.I200.Entity.API.Accountbook;
using Script.I200.Entity.Enum;

#endregion

namespace Script.I200.Application.Accountbook
{
    public class AccountbookService : IAccountbookService
    {
        private readonly IAccountbookProxyService _accountbookProxyService = new AccountbookProxyService();
        private readonly IAuthCodeService _authCodeService = new AuthCodeService();
        //private ISharedService _sharedService = new  MockSharedService();
        private readonly IBasicDataService _basicDataService = new BasicDataService();
        private readonly ISharedService _sharedService = new SharedService();
        //private IAuthCodeService _authCodeService = new MockAuthCodeService();
        private readonly IUserQueryService _userQueryService = new UserQueryService();
        private string _baseUrl = "http://192.168.20.100:8081/api";

        /// <summary>
        ///     绑定提现账号
        /// </summary>
        /// <param name="userContext">店铺相关信息</param>
        /// <param name="account">提现账号信息</param>
        public ResponseModel AddWithdrawingAccount(UserContext userContext, WithdrawingAccountRequest account)
        {
            //根据ProvinceId和CityId获取省名称和城市名称
            var provinceAndCityNames = _basicDataService.GetProvinceAndCityNameById(account.ProvinceId, account.CityId);
            account.ProvinceName = provinceAndCityNames.Item1;
            account.CityName = provinceAndCityNames.Item2;
            var withdrawingAccountResponse = new AccountCreditCardResponse();
            var checkResult = _authCodeService.CheckCaptchaCode(CaptchaEnum.BindCreditCard,
                userContext.AccId, CaptchaPhoneEmailEnum.Phone, account.CheckCode, account.Phone);

            var responseModel = new ResponseModel();
            if (checkResult.IsSuccess)
            {
                var result = _accountbookProxyService.AddWithdrawingAccount(userContext, account);
                if (result.Data != null)
                {
                    var resultModel = result.Data;
                    withdrawingAccountResponse.Id = resultModel.Id;
                    withdrawingAccountResponse.PayeeAccount = resultModel.MasterCardAccount;
                    withdrawingAccountResponse.Phone = resultModel.MobilePhone;
                    withdrawingAccountResponse.PayeeName = resultModel.MasterCardName;
                    withdrawingAccountResponse.State = resultModel.Status;
                    withdrawingAccountResponse.BankName = resultModel.MasterCardBranch;

                    responseModel.Data = resultModel;
                    responseModel.Code = (int) ErrorCodeEnum.Success;
                }
            }
            else
            {
                responseModel.Code = 1;
                responseModel.Data = null;
                responseModel.Message = "验证码校验不正确，请重新输入!";
            }
            return responseModel;
        }

        /// <summary>
        ///     获取提现账户列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public ResponseModel GetWithdrawingAccountList(UserContext userContext)
        {
            var result = _accountbookProxyService.GetWithdrawingAccountList(userContext);
            var responseModel = new ResponseModel();
            var accountCreditCardResponseList = new List<AccountCreditCardResponse>();
            if (result.Code == Convert.ToInt32(ResponseErrorcode.C200))
            {
                if (result.Data != null)
                {
                    var model = result.Data;
                    accountCreditCardResponseList.AddRange(model.Select(item => new AccountCreditCardResponse
                    {
                        Id = item.Id,
                        PayeeName = item.MasterCardName,
                        PayeeAccount = item.MasterCardAccount,
                        BankName = item.ThirdPartyPaymentName,
                        Phone = item.MobilePhone,
                        State = item.Status
                    }));
                }
            }
            responseModel.Data = accountCreditCardResponseList;
            responseModel.Code = result.Code;
            responseModel.Message = result.Message;
            return responseModel;
        }

        /// <summary>
        ///     获取提现记录列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="searchParam"></param>
        /// <returns></returns>
        public ResponseModel SearchWithdrawingJournals(UserContext userContext,
            WithdrawingJournalSearchParam searchParam)
        {
            var result = _accountbookProxyService.SearchWithdrawingJournals(userContext, searchParam);
            var responseModel = new ResponseModel();
            var withdrawingJournalSearchResult = new WithdrawingJournalSearchResult();

            var withdrawingJournalSearchResultItem = new List<WithdrawingJournalSearchResultItem>();
            if (result.Code == Convert.ToInt32(ResponseErrorcode.C200))
            {
                if (result.Data != null)
                {
                    var model = result.Data;
                    if (model.Items.Count > 0)
                    {
                        withdrawingJournalSearchResultItem.AddRange(
                            model.Items.Select(item => new WithdrawingJournalSearchResultItem
                            {
                                Id = item.WithdrawalsTradeId,
                                CreatedAt = item.CreateTime,
                                Amount = item.TradeMoney,
                                Status = item.Status,
                                TransferedAt = item.CompleteTime
                            }));
                    }

                    withdrawingJournalSearchResult.Items = withdrawingJournalSearchResultItem;
                    withdrawingJournalSearchResult.TotalWithdrawingAmount = model.TotalMoney;
                    withdrawingJournalSearchResult.TotalPage = model.TotalPageCount;
                    withdrawingJournalSearchResult.TotalSize = model.TotalNum;
                    withdrawingJournalSearchResult.CurrentPage = model.PageIndex;
                    withdrawingJournalSearchResult.PageSize = model.PageSize;
                }
            }
            responseModel.Data = withdrawingJournalSearchResult;
            responseModel.Code = result.Code;
            responseModel.Message = result.Message;
            return responseModel;
        }

        /// <summary>
        ///     获取资金详情列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="searchParam"></param>
        /// <returns></returns>
        public ResponseModel SearchAccountbookJournals(UserContext userContext,
            WithdrawingJournalSearchParam searchParam)
        {
            var result = _accountbookProxyService.SearchAccountbookJournals(userContext, searchParam);
            var responseModel = new ResponseModel();
            var accountbookJournalSearchResult = new AccountbookJournalSearchResult();
            var accountbookJournalSearchResultItem = new List<AccountbookJournalSearchResultItem>();
            if (result.Code == Convert.ToInt32(ResponseErrorcode.C200))
            {
                if (result.Data != null)
                {
                    var model = result.Data;
                    if (model.Items.Count > 0)
                    {
                        accountbookJournalSearchResultItem.AddRange(
                            model.Items.Select(item => new AccountbookJournalSearchResultItem
                            {
                                Id = item.BillingJournalId,
                                TransferedAt = item.CreateTime,
                                TradeType = item.TradeType,
                                AmountTrade = item.EnterMoney,
                                Balance = item.FinalMoney
                            }));
                    }

                    accountbookJournalSearchResult.Items =
                        accountbookJournalSearchResultItem;
                    accountbookJournalSearchResult.TotalAmountIn = model.TotalMoneyIn;
                    accountbookJournalSearchResult.TotalAmoutOut = model.TotalMoneyOut;
                    accountbookJournalSearchResult.TotalPage = model.TotalPageCount;
                    accountbookJournalSearchResult.TotalSize = model.TotalNum;
                    accountbookJournalSearchResult.PageSize = model.PageSize;
                    accountbookJournalSearchResult.CurrentPage = model.PageIndex;
                }
            }
            responseModel.Data = accountbookJournalSearchResult;
            responseModel.Code = result.Code;
            responseModel.Message = result.Message;
            return responseModel;
        }

        /// <summary>
        ///     获取收单记录列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="searchParam"></param>
        /// <returns></returns>
        public ResponseModel SearchBillingJournals(UserContext userContext, BillingJournalSearchParam searchParam)
        {
            var result = _accountbookProxyService.SearchBillingJournals(userContext, searchParam);
            var responseModel = new ResponseModel();
            var billingJournalSearchResult = new BillingJournalSearchResult();

            var billingJournalSearchResultItem = new List<BillingJournalSearchResultItem>();
            if (result.Code == Convert.ToInt32(ResponseErrorcode.C200))
            {
                if (result.Data != null)
                {
                    var model = result.Data;
                    if (model.Items.Count > 0)
                    {
                        billingJournalSearchResultItem.AddRange(
                            model.Items.Select(item => new BillingJournalSearchResultItem
                            {
                                CreatedAt = item.CreateTime,
                                BusinessId = item.BillingBusinessId,
                                BusinessName =
                                    BillingBusinessType.GetBusinessName((BillingBusinessTypeEnum) item.BillingBusinessId),
                                OutTradeNumber = item.TradeId,
                                JournalNumber = item.JournalOutId,
                                Subject = item.TradeTitle,
                                AmountDue = item.TradeMoney,
                                Poundage = item.PoundageScaleValueDiscount,
                                AmountReceived = item.PayMoney,
                                Status = item.Status
                            }));
                    }

                    billingJournalSearchResult.Items = billingJournalSearchResultItem;
                    billingJournalSearchResult.TotalBillingAmount = model.TotalMoney;
                    billingJournalSearchResult.TotalPage = model.TotalPageCount;
                    billingJournalSearchResult.TotalSize = model.TotalNum;
                    billingJournalSearchResult.PageSize = model.PageSize;
                    billingJournalSearchResult.CurrentPage = model.PageIndex;
                }
            }
            responseModel.Data = billingJournalSearchResult;
            responseModel.Code = result.Code;
            responseModel.Message = result.Message;
            return responseModel;
        }

        /// <summary>
        ///     获取支付方式列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public ResponseModel GetPaymentTypeList(UserContext userContext)
        {
            var result = _accountbookProxyService.GetPaymentTypeList(userContext);

            var responseModel = new ResponseModel();
            var billingPaymentResponseList = new List<BillingPaymentResponse>();
            if (result.Code == Convert.ToInt32(ResponseErrorcode.C200))
            {
                if (result.Data != null)
                {
                    var model = result.Data;
                    billingPaymentResponseList.AddRange(model.Select(item => new BillingPaymentResponse
                    {
                        Id = item.Id,
                        Name = item.Name
                    }));
                }
            }
            responseModel.Data = billingPaymentResponseList;
            responseModel.Code = result.Code;
            responseModel.Message = result.Message;
            return responseModel;
        }

        /// <summary>
        ///     获取提现账户余额
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public ResponseModel GetWithdrawingBalance(UserContext userContext)
        {
            var result = _accountbookProxyService.GetWithdrawingBalance(userContext);
            var responseModel = new ResponseModel();
            var accountBalanceResponse = new AccountBalanceResponse();
            if (result.Code == Convert.ToInt32(ResponseErrorcode.C200))
            {
                if (result.Data != null)
                {
                    var model = result.Data;
                    accountBalanceResponse.TotalBalance = model.Money + model.TotalFrozenPayMoney;
                    accountBalanceResponse.AvailableBalance = model.Money;
                    accountBalanceResponse.TotalAmount = model.BillingBusinessList.Sum(b => b.TotalMoney);
                    if (model.BillingBusinessList != null &&
                        model.BillingBusinessList.Any(b => b.Id == (int) BillingBusinessTypeEnum.WechatGathering))
                    {
                        accountBalanceResponse.WechatGatheringAmount =
                            model.BillingBusinessList.First(b => b.Id == (int) BillingBusinessTypeEnum.WechatGathering)
                                .TotalMoney;
                        accountBalanceResponse.WechatTotalBillingJournalsNum =
                            model.BillingBusinessList.First(b => b.Id == (int) BillingBusinessTypeEnum.WechatGathering)
                                .TotalBillingJournalsNum;
                    }

                    if (model.BillingBusinessList != null &&
                        model.BillingBusinessList.Any(b => b.Id == (int) BillingBusinessTypeEnum.MobileShop))
                    {
                        accountBalanceResponse.MobileShopGatheringAmount =
                            model.BillingBusinessList.First(b => b.Id == (int) BillingBusinessTypeEnum.MobileShop)
                                .TotalMoney;

                        accountBalanceResponse.MobileShopTotalBillingJournalsNum =
                            model.BillingBusinessList.First(b => b.Id == (int) BillingBusinessTypeEnum.MobileShop)
                                .TotalBillingJournalsNum;
                    }
                }
            }

            responseModel.Data = accountBalanceResponse;
            responseModel.Code = result.Code;
            responseModel.Message = result.Message;
            return responseModel;
        }

        /// <summary>
        ///     新增提现记录(提现)
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseModel AddWithdrawingJournal(UserContext userContext,
            NewWithdrawingJournalRequest request)
        {
            //获取提现用户绑定的手机号码
            var result = _accountbookProxyService.GetWithdrawingAccountList(userContext);
            var withdrawingAccountList = result.Data;
            if (result.Code != Convert.ToInt32(ResponseErrorcode.C200) || withdrawingAccountList == null)
            {
                return new ResponseModel
                {
                    Code = (int) ErrorCodeEnum.ErrorGettingWithdrawingAccount,
                    Data = null,
                    Message = result.Message
                };
            }

            var phone = withdrawingAccountList.First().MobilePhone;
            var responseModel = _sharedService.CheckVerificationCode(userContext, 1, 1, request.CheckCode, phone);
            if (responseModel.Code == (int) ErrorCodeEnum.Success)
            {
                var addResult = _accountbookProxyService.AddWithdrawingJournal(userContext, request);
                if (addResult != null)
                {
                    return new ResponseModel
                    {
                        Code = addResult.Code,
                        Data = addResult.Data,
                        Message = addResult.Message
                    };
                }
            }
            else
            {
                responseModel.Code = 1;
                responseModel.Data = null;
                responseModel.Message = "验证码校验不正确，请重新输入!";
            }
            return responseModel;
        }

        /// <summary>
        ///     发送提现验证码
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public ResponseModel SendWithdrawingAuthCode(UserContext userContext)
        {
            //获取提现用户绑定的手机号码
            var result = _accountbookProxyService.GetWithdrawingAccountList(userContext);
            var withdrawingAccountList = result.Data;

            if (result.Code != Convert.ToInt32(ResponseErrorcode.C200) || withdrawingAccountList == null)
            {
                return new ResponseModel
                {
                    Code = (int) ErrorCodeEnum.ErrorGettingWithdrawingAccount,
                    Data = result.Data,
                    Message = result.Message
                };
            }

            var phone = withdrawingAccountList.First().MobilePhone;
            var model = _authCodeService.SendAuthCode(CaptchaEnum.Withdrawals, userContext.AccId,
                CaptchaPhoneEmailEnum.Phone, phone);
            return new ResponseModel
            {
                Code = model.IsSuccess ? (int) ErrorCodeEnum.Success : (int) ErrorCodeEnum.ErrorSendSmsFailed,
                Message = model.ErrMessage
            };
        }

        /// <summary>
        ///     发送绑定银行卡提现验证码
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public ResponseModel SendBindWithdrawingAccountAuthCode(UserContext userContext, string phone)
        {
            var model = _authCodeService.SendAuthCode(CaptchaEnum.BindCreditCard, userContext.AccId,
                CaptchaPhoneEmailEnum.Phone, phone);
            return new ResponseModel
            {
                Code = model.IsSuccess ? (int) ErrorCodeEnum.Success : (int) ErrorCodeEnum.ErrorSendSmsFailed,
                Message = model.ErrMessage
            };
        }

        /// <summary>
        ///     获取当前操作用户的权限
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ResponseModel GetCurrentOperatorPower(UserContext userContext, int type)
        {
            if (type == Convert.ToInt32(PowerEnum.RoleCheck))
            {
                if (userContext.Role != 1)
                {
                    return new ResponseModel
                    {
                        Code = (int) ErrorCodeEnum.Forbidden,
                        Message = "当前无权限操作",
                        Data = null
                    };
                }
            }
            else if (type == Convert.ToInt32(PowerEnum.ViewCheck))
            {
                var powerWeight = Convert.ToInt32(AccountUserPowerEnum.UserPowerV2Enum.SalesView);
                //根据店员是否有查看销售列表权限判断是否可以查看提现列表和收单列表以及资金明细列表权限
                if (!_userQueryService.IsPower(powerWeight, userContext.Powers, userContext.Role))
                {
                    return new ResponseModel
                    {
                        Code = (int) ErrorCodeEnum.Forbidden,
                        Message = "当前无权限操作",
                        Data = null
                    };
                }
            }
            return new ResponseModel
            {
                Code = (int) ErrorCodeEnum.Success,
                Message = "获取数据成功",
                Data = null
            };
        }

        /// <summary>
        ///     业务列表记录
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public ResponseModel GetBillingBusiness(UserContext userContext)
        {
            var result = _accountbookProxyService.GetBillingBusiness(userContext);
            if (result == null)
            {
                return new ResponseModel
                {
                    Code = 1,
                    Data = null,
                    Message = "数据为空"
                };
            }
            return new ResponseModel
            {
                Code = result.Code,
                Data = result.Data,
                Message = result.Message
            };
        }

        /// <summary>
        ///     获取提现和收单列表状态
        /// </summary>
        /// <returns></returns>
        public ResponseModel GetWithdrawingStatusAndBillingStatus()
        {
            var responseModel = new ResponseModel();
            var listStatus =
                new Dictionary<string, Dictionary<string, string>>();
            var withdrawingStatus = new Dictionary<string, string>
            {
                {"4", "提现中"},
                {"5", "提现成功"},
                {"6", "提现失败"}
            };

            var billingStatus = new Dictionary<string, string>()
            {
               {"1", "交易中"},
               {"4", "冻结资金"}, 
               {"1000", "支付成功"} 
            };
               

            //微信收款交易状态值
            var weChatBillingStatus = new Dictionary<string, string>
            {
                {"1", "交易中"},
                {"4", "结算中"},
                {"1000", "交易成功"}
            };

            //手机橱窗交易状态值
            var mobileShowCaseBillingStatus = new Dictionary<string, string>
            {
                {"1", "待付款"},
                {"4", "待收货"},
                {"1000", "交易成功"}
            };

            var withdrawingAccountStatus = new Dictionary<string, string>
            {
                {"0", "审核中"},
                {"1", "已认证"}
            };

            var businessType = new Dictionary<string, string>
            {
                {"2", "微信收款"},
                {"4", "手机橱窗"}
            };

            // 添加收单和提现记录列表状态
            listStatus.Add("withdrawingStatus", withdrawingStatus);
            listStatus.Add("billingStatus", billingStatus);

            listStatus.Add("weChatBillingStatus", weChatBillingStatus);
            listStatus.Add("mobileShowCaseBillingStatus", mobileShowCaseBillingStatus);

            listStatus.Add("withdrawingAccountStatus", withdrawingAccountStatus);
            listStatus.Add("billingBusinessType", businessType);
            responseModel.Data = listStatus;
            return responseModel;
        }
    }
}