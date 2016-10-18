using System.Collections.Generic;

namespace Script.I200.Entity
{
    public class StatusDictionary
    {
        private static object singletonLock = new object();

        private static Dictionary<int, string> dicAccountLicenseNameStatus;
        /// <summary>
        /// 店铺版本列表
        /// </summary>
        public static Dictionary<int, string> DicAccountLicenseNameStatus
        {
            get { return dicAccountLicenseNameStatus; }
        }

        
        static StatusDictionary()
        {
            //店铺版本
            if (dicAccountLicenseNameStatus == null)
            {
                lock (singletonLock)
                {
                    dicAccountLicenseNameStatus = new Dictionary<int, string>();
                    dicAccountLicenseNameStatus.Add(1, "免费版");
                    dicAccountLicenseNameStatus.Add(3, "高级版");
                    dicAccountLicenseNameStatus.Add(5, "行业版");
                }

            }
        }
    }
}
