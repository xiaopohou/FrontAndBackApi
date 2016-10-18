using System;

namespace Script.I200.Entity.Dto.Accountbook
{
    public class C_AccountCreditCardShowDto
    {

        /// <summary>
        /// 实体编号
        /// </summary>	
        public int Id { get; set; }

        /// <summary>
        /// 店铺编号
        /// </summary>		
        public int AccountId { get; set; }

        /// <summary>
        /// 第三方支付编号；关联：T_Billing_ThirdPartyPayment.Id
        /// </summary>		
        public int ThirdPartyPaymentId { get; set; }
        /// <summary>
        /// 第三方支付名称；关联：T_Billing_ThirdPartyPayment.Name
        /// </summary>		
        public string ThirdPartyPaymentName { get; set; }
        /// <summary>
        /// 账号持有人姓名
        /// </summary>		
        public string MasterCardName { get; set; }
        /// <summary>
        /// 账号信息
        /// </summary>		
        public string MasterCardAccount { get; set; }
        /// <summary>
        /// 账号开户分行名称
        /// </summary>		
        public string MasterCardBranch { get; set; }
        /// <summary>
        /// 账号开户支行名称
        /// </summary>		
        public string MasterCardSubBranch { get; set; }
        /// <summary>
        /// 开户行是否招行；LOV：0=否，1=是
        /// </summary>		
        public int MasterCardType { get; set; }

        /// <summary>
        /// 账号开户身份证号码
        /// </summary>		
        public string IdentityCard { get; set; }

        /// <summary>
        /// 账号开户绑定的手机号
        /// </summary>
        public string MobilePhone { get; set; }

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
        public string BusinessLicenceUrl { get; set; }

        /// <summary>
        ///开户人身份证正面存储路径
        /// </summary>		
        public string IdentityCardUrlA { get; set; }

        /// <summary>
        /// 开户人身份证反面存储路径
        /// </summary>		
        public string IdentityCardUrlB { get; set; }

        /// <summary>
        /// 状态；LOV：0=未通过审核，1=通过审核
        /// </summary>		
        public int Status { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>		
        public DateTime AuditTime { get; set; }
        /// <summary>
        /// 审核操作人Id
        /// </summary>		
        public string AuditOperater { get; set; }
        /// <summary>
        /// 审核操作人Name
        /// </summary>		
        public string AuditOperaterName { get; set; }

        /// <summary>
        /// 店铺营业执照存储路径
        /// </summary>		
        public string BusinessLicenceUrlThumbnail
        {
            get { return this.BusinessLicenceUrl + "!thumbnail"; }
        }

        /// <summary>
        ///开户人身份证正面存储路径
        /// </summary>		
        public string IdentityCardUrlAThumbnail
        {
            get { return this.IdentityCardUrlA + "!thumbnail"; }
        }

        /// <summary>
        /// 开户人身份证反面存储路径
        /// </summary>		
        public string IdentityCardUrlBThumbnail
        {
            get { return this.IdentityCardUrlB + "!thumbnail"; }
        }

        /// <summary>
        /// 店铺营业执照存储路径
        /// </summary>		
        public string BusinessLicenceUrlLarge
        {
            get { return this.BusinessLicenceUrl + "!large"; }
        }

        /// <summary>
        ///开户人身份证正面存储路径
        /// </summary>		
        public string IdentityCardUrlALarge
        {
            get { return this.IdentityCardUrlA + "!large"; }
        }

        /// <summary>
        /// 开户人身份证反面存储路径
        /// </summary>		
        public string IdentityCardUrlBLarge
        {
            get { return this.IdentityCardUrlB + "!large"; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; }
    }
}
