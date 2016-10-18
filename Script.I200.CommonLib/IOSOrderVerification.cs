using System.Collections.Generic;

namespace CommonLib
{
    /// <summary>
    /// IOS 验证
    /// </summary>
    public static class IOSOrderVerification
    {
        private static string ApiHostUrlSandBox = "https://sandbox.itunes.apple.com/verifyReceipt";
        private static string ApiHostUrlAppStore = "https://buy.itunes.apple.com/verifyReceipt";

        /// <summary>
        /// 验证订单状态
        /// </summary>
        /// <param name="receiptData"></param>
        /// <param name="base64Enabled">对票据信息进行Base64编码</param>
        /// <param name="isSanbox">沙盒验证模式，测试用</param>
        public static string Verification(string transactionReceipt, bool base64Enabled, bool isSanbox)
        {
            Dictionary<string, string> formData = new Dictionary<string, string>();
            // <- iOS 7+ 验证模式兼容
            if (base64Enabled)
            {
                formData["receipt-data"] = Helper.EncodeBase64(transactionReceipt);
            }
            else
            {
                formData["receipt-data"] = transactionReceipt;
            }
            string returnStr = HttpUtility.HttpPostJson(isSanbox ? ApiHostUrlSandBox : ApiHostUrlAppStore, null, Helper.JsonSerializeObject(formData));
            return returnStr;

        }

    }
}
