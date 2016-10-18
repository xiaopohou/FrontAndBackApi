using System.ComponentModel;

namespace Script.I200.Entity.Enum
{
    public enum ErrorCodeEnum
    {
        [Description("成功")] Success = 0,

        #region 资金账号 9000 ~ 9009

        [Description("失败")] Failed = 9000,

        [Description("参数为空")] NullArguments = 9001,

        [Description("未知的验证码类型")] UnknowAuthCodeContex = 9002,

        [Description("未知的验证码渠道")] UnknowAuthCodeChannel = 9003,

        [Description("获取提现账号列表失败")] ErrorGettingWithdrawingAccount = 9004,

        [Description("发送短信失败")] ErrorSendSmsFailed = 9005,

        [Description("手机号不能为空")] PhoneCanNotBeNull = 9006,

        [Description("查询开始时间不能大于结束时间")] SearchDateInvalid = 9007,

        [Description("参数校验失败")] ParamsInvalid = 9008,

        [Description("必须为正确的时间格式")] InvalidDate = 9009,

        #endregion

        #region 储值卡 100000 ~ 100100

        [Description("查询开始时间不能大于结束时间")] StoreMoneySearchDateInvalid = 100000,
        [Description("参数为空")] StoreMoneyParamsIsNull = 100001,
        [Description("失败")] StoreMoneyFailed = 100002,
        [Description("实收金额不能大于储值金额")] StoreMoneyIsMoreThanRechargeMoney = 100003,
        #endregion

        #region 计次卡 100100 ~ 100200

        [Description("计次卡卡名不能重复")] TimeCardExistSameNameTimesCard = 100100,
        [Description("已经存在无限制计次卡")] TimeCardExistUnlimitedTimesCard = 100101,
        [Description("一个服务只能绑定一个计次卡")] TimeCardExistServiceOnly = 100102,
        [Description("商品必须为服务类商品")] TimeCardIsService = 100103,
        [Description("获取不到此计次卡信息")] TimeCardGetById = 100104,
        [Description("参数为空")] TimeCardIsNullArguments = 100105,
        [Description("找不到相关服务类商品")] NotFoundServiceItem = 100106,
        [Description("用户计次卡已存在")] ExistSameUserTimesCard = 100107,
        [Description("失败")] OperateTimesCardFailed = 100108,
        [Description("手机号或会员名不能为空")] PhoneAndUserNameIsNull = 100109,
        #endregion

        #region 短信设置 100200  ~ 100300

        [Description("失败")] SmsSettingFailed = 100200,

        #endregion

        #region 支出管理 100300-100400

        [Description("失败")] ExpenseFailed = 100300,

        [Description("存在相同的分类名称")] HasSameCategoryName = 100301,

        [Description("参数为空")] ExpenseNullArguments = 100304,

        #endregion

        #region 店铺优惠券 100401~100500

        [Description("已领取过此优惠券")] RecievedCoupon = 100401,
        [Description("生成优惠券失败")] CreateCouponFail = 100402,

        #endregion

        #region 硬件商城 100501~100600

        [Description("无数据")] NoMaterialGoods = 100501,

        #endregion

        #region 设置Setting 100601~100700

        [Description("参数为空")] SettingNullArguments = 100601,

        #endregion

        #region 反馈 100701~100800

        [Description("您刚刚已提交了反馈，请稍后再来提交~")] FeedbackLessThanTenMinutes = 100701,

        [Description("失败")] FeedbackFail = 100702,

        #endregion

        #region 心情&签到 100901~101000

        [Description("您刚刚已提交了，请稍后再来提交~")] MoodLessThanOneTimes = 100901,

        [Description("失败")] MoodFail = 100902,

        #endregion

        #region 功能模块 101101~101200

        [Description("您最多可添加8个快捷操作~")] ModuleLessThanEightTimes = 101101,

        [Description("您是免费版，无法完成高级功能快捷添加~")] ModuleNeedPaid = 101102,

        [Description("您至少保留4个快捷操作~")] ModuleMoreThanFourTimes = 101103,

        [Description("失败")] ModuleFail = 101104,

        #endregion

        #region 会员新增 101200~101300
        [Description("已存在相同的会员卡号")]
        SameUserNo = 101200,
        [Description("已存在相同的会员手机号")]
        SameUserPhone = 101201,

        [Description("设置失败")]
        UserFail = 101202,

        [Description("原密码错误~")]
        UserOldPasswordError = 101203,

        [Description("密码不能为空~")]
        UserPasswordEmpty = 101204,
        [Description("已存在相同的会员分组")]
        SameUserGroupName = 101205,
        [Description("找不到对应的会员信息")] NotFoundUserInfo = 101206,

        [Description("新增会员失败")] AddUserFailed = 101207,
        
        #endregion

        #region 销售 101300~104300

        [Description("销售失败")]
        SalesFailed = 101300,

        [Description("还款失败")]
        SalesRepaymentFailed = 101301,

        [Description("退货失败")]
        SalesReturnGoodFailed = 101302,

        [Description("销售数据不存在")]
        SalesNotFind = 101303,

        [Description("销售数据，非欠款！")]
        SalesNotRepayment = 101304,

        #endregion

        [Description("请求错误")] RequestError = 400,

        [Description("无权限操作")] Forbidden = 403,

        [Description("未找到对应的资源")] NotFound = 404,

        [Description("服务器错误")] ServerError = 500,

        #region 全局公共Code（参数为空，时间参数不合法等） 8000~8999

        [Description("参数为空")] ParamIsNullArgument = 8000,

        [Description("查询开始时间不能大于结束时间")] SearchDateParamInvalid = 8001,

        [Description("请确保输入开始时间和结束时间查询")] CheckStartDateOrEndDateIsNull = 8002,

        [Description("名称不能含有特殊字符")] IsValidName = 8003,

        [Description("单页数据最大不能超过100条")] MoreThanMaxSize = 8004,

        [Description("Token过期，查询不到用户信息")] TokenIsExpired = 8005

        #endregion
    }
}