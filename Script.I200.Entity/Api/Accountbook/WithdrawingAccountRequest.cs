using Script.I200.Entity.Dto.Accountbook;

namespace Script.I200.Entity.Api.Accountbook
{
    /// <summary>
    /// 提现账号实体
    /// </summary>
    public class WithdrawingAccountRequest
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 银行卡账号
        /// </summary>
        public string PayeeAccount { get; set; }

        /// <summary>
        /// 持卡人姓名
        /// </summary>
        public string PayeeName { get; set; }


        /// <summary>
        /// 开户行Id
        /// </summary>
        public int BankId { get; set; }

        /// <summary>
        /// 开户人身份证号码
        /// </summary>
        public string CardNo { get; set; }

        /// <summary>
        ///开户人身份证正面存储路径
        /// </summary>		
        public string CardPicFront { get; set; }

        /// <summary>
        /// 开户人身份证反面存储路径
        /// </summary>		
        public string CardPicBack { get; set; }


        /// <summary>
        /// 账号开户绑定的手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 账号开户所在省份
        /// </summary>		
        public int ProvinceId { get; set; }

        /// <summary>
        /// 账号开户所在省份Name
        /// </summary>		
        public string ProvinceName { get; set; }

        /// <summary>
        /// 账号开户所在城市
        /// </summary>		
        public int CityId { get; set; }

        /// <summary>
        /// 账号开户所在城市Name
        /// </summary>		
        public string CityName { get; set; }

        /// <summary>
        /// 店铺营业执照存储路径
        /// </summary>		
        public string CardPic { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public int CheckCode { get; set; }

        /// <summary>
        /// 验证码场景，1: 提现, 2: 绑定银行卡, 3: 解绑银行卡
        /// </summary>
        public int Context { get; set; }

        /// <summary>
        /// 验证码渠道，1: 手机, 2: 邮件
        /// </summary>
        public int Channel { get; set; }

        public AccountCreditCardDto ToDto()
        {
            return new AccountCreditCardDto()
            {
                ThirdPartyPaymentId = BankId,
                MasterCardName = PayeeName,
                MasterCardAccount = PayeeAccount,
                IdentityCard = CardNo,
                IdentityCardUrlA = CardPicFront,
                IdentityCardUrlB = CardPicBack,
                ProvinceId = ProvinceId,
                ProvinceName = ProvinceName,
                CityId = CityId,
                CityName = CityName,
                BusinessLicenceUrl = CardPic,
                MobilePhone = Phone
            };
        }
    }
}