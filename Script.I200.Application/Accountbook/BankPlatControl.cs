using System.Configuration;
using System.Text;
using CommonLib;
using Script.I200.Entity.Dto.Accountbook;
using Script.I200.Entity.Enum;

namespace Script.I200.Controls.BankPlatService
{
    public class BankPlatControl
    {
        #region 接口信息
        /// <summary>
        /// 资金账户信息地址
        /// </summary>
        private static readonly string ProxyUrl = string.Format("{0}", ConfigurationManager.AppSettings["BankPlatServiceAddress"]);
        /// <summary>
        /// 提现秘钥
        /// </summary>
        private static readonly string BPS_Withdrawal = ConfigurationManager.AppSettings["BPS_Withdrawal"];
        /// <summary>
        /// 收单秘钥
        /// </summary>
        private static readonly string BPS_Acquir = ConfigurationManager.AppSettings["BPS_Acquir"];
        /// <summary>
        /// 查询秘钥
        /// </summary>
        private static readonly string BPS_Select = ConfigurationManager.AppSettings["BPS_Select"];
        /// <summary>
        /// 提现AppKey
        /// </summary>
        private static readonly string BPS_Withdrawal_Key = "OxSurWnWcOXtXzwLQAi6";
        /// <summary>
        /// 收单AppKey
        /// </summary>
        private static readonly string BPS_Acquir_Key = "HwGhLA5Twma7w4gRV2g4";
        /// <summary>
        /// 查询AppKey
        /// </summary>
        private static readonly string BPS_Select_Key = "wgDHIGc3KSDMxCtTkmXc";


        #endregion
        /// <summary>
        /// 生成验证信息
        /// </summary>
        /// <returns></returns>
        public AccountbookRequest CreateAuthCode(BankPlatBusinessEnum businessEnum)
        {
            string stroBusinessKey = "";
            string strAppValue = "";
            switch (businessEnum)
            {
                //收单
                case BankPlatBusinessEnum.Acquir:
                    stroBusinessKey = BPS_Acquir_Key;
                    strAppValue = BPS_Acquir;
                    break;
                //提现
                case BankPlatBusinessEnum.Withdrawal:
                    stroBusinessKey = BPS_Withdrawal_Key;
                    strAppValue = BPS_Withdrawal;
                    break;
                //查询
                case BankPlatBusinessEnum.Select:
                    stroBusinessKey = BPS_Select_Key;
                    strAppValue = BPS_Select;
                    break;
                default:
                    break;
            }
            var requestMd = new AccountbookRequest
            {
                Timestamp = Helper.GetTimeStamp(),
                Nonce = Helper.GetRandomNum(),
                BusinessKey = stroBusinessKey
            };
            var strSign = new StringBuilder();
            strSign.Append(stroBusinessKey);
            strSign.Append(requestMd.Timestamp);
            strSign.Append(requestMd.Nonce);
            strSign.Append(strAppValue);

            requestMd.Signature = Helper.Md5Hash(strSign.ToString());

            return requestMd;
        }
    }
}
