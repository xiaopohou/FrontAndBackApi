using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using fastJSON;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPinyin;
using RestSharp;

namespace CommonLib
{
    public static class Helper
    {
        /// <summary>
        /// 王振松代理商ID
        /// </summary>
        public const int AgentIdW = 184;

        #region GetInitials 获得拼音首字母
        /// <summary>
        /// 获得拼音首字母
        /// </summary>
        /// <param name="sText">待转换文字</param>
        /// <returns></returns>
        public static string GetInitials(string sText)
        {
            return Pinyin.GetInitials(sText);
        }
        #endregion

        #region GetPinyin 获得中文全拼(大写)
        /// <summary>
        /// 获得中文全拼(大写)
        /// </summary>
        /// <param name="sText">待转换文字</param>
        /// <returns></returns>
        public static string GetPinyin(string sText)
        {
            return Pinyin.GetPinyin(sText).Replace(" ", "").ToUpper();
        }
        #endregion

        #region GetClientIP 客户端IP
        /// <summary>
        /// 客户端IP(穿过代理服务器取远程用户真实IP地址)
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            //try
            //{
            //    if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            //        return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            //    else
            //        return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            //}
            //catch { return ""; }

            string result = String.Empty;
            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (result != null && result != String.Empty)
            {
                //可能有代理     
                if (result.IndexOf(".") == -1)    //没有"."肯定是非IPv4格式     
                    result = null;
                else
                {
                    if (result.IndexOf(",") != -1)
                    {
                        //有","，估计多个代理。取第一个不是内网的IP。     
                        result = result.Replace(" ", "").Replace("\"", "");
                        string[] temparyip = result.Split(",;".ToCharArray());
                        for (int i = 0; i < temparyip.Length; i++)
                        {
                            if (IsIPAddress(temparyip[i])
                                && temparyip[i].Substring(0, 3) != "10."
                                && temparyip[i].Substring(0, 7) != "192.168"
                                && temparyip[i].Substring(0, 7) != "172.16.")
                            {
                                return temparyip[i];    //找到不是内网的地址     
                            }
                        }
                    }
                    else if (IsIPAddress(result)) //代理即是IP格式     
                        return result;
                    else
                        result = null;    //代理中的内容 非IP，取IP     
                }
            }
            string IpAddress = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null && HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != String.Empty) ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (null == result || result == String.Empty)
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (result == null || result == String.Empty)
                result = HttpContext.Current.Request.UserHostAddress;
            return result;
            //return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
        }
        #endregion

        #region IsIPAddress 判断是否为IP地址
        /// <summary>
        /// 判断是否为IP地址
        /// </summary>
        /// <param name="sIp"></param>
        /// <returns></returns>
        public static bool IsIPAddress(string sIp)
        {
            if (sIp == null || sIp == string.Empty || sIp.Length < 7 || sIp.Length > 15) return false;
            string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";
            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
            return regex.IsMatch(sIp);
        }
        #endregion

        #region IsNumber 判断是否为纯数字
        /// <summary>
        /// 判断是否为纯数字
        /// </summary>
        /// <param name="sNumber">待测试值</param>
        /// <param name="iSize">数字满足最小长度</param>
        /// <returns></returns>
        public static bool IsNumber(string sNumber, int iSize)
        {
            bool result = false;

            Regex reg = new Regex("^[0-9]+$");
            Match ma = reg.Match(sNumber);
            if (iSize > 0)
            {
                if (ma.Success && sNumber.Length >= iSize)
                {
                    result = true;
                }
            }
            else
            {
                if (ma.Success)
                {
                    result = true;
                }
            }

            return result;
        }
        #endregion

        #region Md5Hash 获取字符串MD5哈希值
        /// <summary>
        /// 获取字符串MD5哈希值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Md5Hash(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
        #endregion

        #region EncryptDES DES加密字符串
        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="Key">密钥</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string EncryptDES(string encryptString, byte[] Key)
        {
            try
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                des.Key = Key;
                des.Mode = CipherMode.ECB;
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, des.CreateEncryptor(), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();
                foreach (byte b in mStream.ToArray())
                {
                    ret.AppendFormat("{0:X2}", b);
                }
                return ret.ToString();
            }
            catch
            {
                return encryptString;
            }
        }
        #endregion

        #region DecryptDES DES解密字符串
        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DecryptDES(string decryptString, byte[] Key)
        {
            try
            {
                byte[] inputByteArray = new byte[decryptString.Length / 2];
                for (int x = 0; x < decryptString.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(decryptString.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                des.Key = Key;
                des.Mode = CipherMode.ECB;
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, des.CreateDecryptor(), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }
        #endregion

        #region EncodeBase64 Base64加密字符串
        /// <summary>
        /// 使用Base64算法加密字符串
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string EncodeBase64(string code)
        {
            string encode = "";
            byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(code);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = code;
            }
            return encode;
        }
        #endregion

        #region DecodeBase64 Base64解密字符串
        /// <summary>
        /// 解密Base64算法加密字符串
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string DecodeBase64(string code)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(code);
            try
            {
                decode = Encoding.GetEncoding("utf-8").GetString(bytes);
            }
            catch
            {
                decode = code;
            }
            return decode;
        }
        #endregion

        #region CreateUUID 压缩Guid编码(12位)
        /// <summary>
        /// 生成压缩为12位的Guid编码
        /// </summary>
        /// <returns></returns>
        public static string CreateUUID()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            long long_guid = BitConverter.ToInt64(buffer, 0);

            string _Value = Math.Abs(long_guid).ToString();

            byte[] buf = new byte[_Value.Length];
            int p = 0;
            for (int i = 0; i < _Value.Length; )
            {
                byte ph = Convert.ToByte(_Value[i]);

                int fix = 1;
                if ((i + 1) < _Value.Length)
                {
                    byte pl = Convert.ToByte(_Value[i + 1]);
                    buf[p] = (byte)((ph << 4) + pl);
                    fix = 2;
                }
                else
                {
                    buf[p] = (byte)(ph);
                }

                if ((i + 3) < _Value.Length)
                {
                    if (Convert.ToInt16(_Value.Substring(i, 3)) < 256)
                    {
                        buf[p] = Convert.ToByte(_Value.Substring(i, 3));
                        fix = 3;
                    }
                }
                p++;
                i = i + fix;
            }
            byte[] buf2 = new byte[p];
            for (int i = 0; i < p; i++)
            {
                buf2[i] = buf[i];
            }
            string cRtn = Convert.ToBase64String(buf2);
            if (cRtn == null)
            {
                cRtn = "";
            }
            cRtn = cRtn.ToLower();
            cRtn = cRtn.Replace("/", "");
            cRtn = cRtn.Replace("+", "");
            cRtn = cRtn.Replace("=", "");
            if (cRtn.Length == 12)
            {
                return cRtn.ToUpper();
            }
            else
            {
                return CreateUUID();
            }

        }
        #endregion

        #region SHA1_Encrypt SHA1加密函数
        /// <summary>
        /// SHA1加密函数
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        public static string SHA1_Encrypt(string sourceString)
        {
            byte[] strRes = Encoding.UTF8.GetBytes(sourceString);
            HashAlgorithm hashSha = new SHA1CryptoServiceProvider();
            strRes = hashSha.ComputeHash(strRes);
            var enText = new StringBuilder();
            foreach (byte iByte in strRes)
            {
                enText.AppendFormat("{0:x2}", iByte);
            }
            return enText.ToString();
        }
        #endregion

        #region HmacSha1Sign HmacSHA1加密
        /// <summary>
        /// HmacSHA1加密
        /// </summary>
        /// <param name="text"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string HmacSha1Sign(string text, string key)
        {
            Encoding encode = Encoding.GetEncoding("utf-8");
            byte[] byteData = encode.GetBytes(text);
            byte[] byteKey = encode.GetBytes(key);
            HMACSHA1 hmac = new HMACSHA1(byteKey);
            CryptoStream cs = new CryptoStream(Stream.Null, hmac, CryptoStreamMode.Write);
            cs.Write(byteData, 0, byteData.Length);
            cs.Close();
            return Convert.ToBase64String(hmac.Hash);
        }
        #endregion

        #region GetTimeStamp 获取时间戳
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        #endregion

        #region GetRandomNum 获得随机数(6位长度)
        /// <summary>
        /// 获得随机数(6位长度)
        /// </summary>
        /// <returns></returns>
        public static string GetRandomNum()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            return r.Next(100000, 999999).ToString();
        }
        #endregion

        #region SendHttpPost 发送Post请求
        /// <summary>
        /// 发送Post请求
        /// </summary>
        /// <param name="httpUrl">请求地址</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public static string SendHttpPost(string httpUrl, Dictionary<string, string> parameters)
        {
            return HttpUtility.HttpPost(httpUrl, null, parameters, null);
        }
        #endregion

        #region SendHttpGet 发送Get请求
        /// <summary>
        /// 发送Get请求
        /// </summary>
        /// <param name="sHttpUrl">Url地址</param>
        /// <returns></returns>
        public static string SendHttpGet(string httpUrl, IDictionary<string, string> parameters)
        {
            string strParameter = string.Empty;
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
                strParameter = "?" + buffer.ToString();
            }

            WebRequest req = WebRequest.Create(httpUrl + strParameter);
            HttpWebRequest httpreg = (HttpWebRequest)req;
            httpreg.Method = "GET";
            httpreg.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0)";
            WebResponse resp = httpreg.GetResponse();
            StreamReader reader = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("utf-8"));
            return reader.ReadToEnd();
        }

        #endregion

        #region JsonSerializeObject 序列化对象为Json字符串
        /// <summary>
        /// 序列化对象为Json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string JsonSerializeObject(object obj)
        {
            JSON.Parameters.SerializeNullValues = true;
            JSON.Parameters.ShowReadOnlyProperties = true;
            JSON.Parameters.UseUTCDateTime = false;
            JSON.Parameters.UsingGlobalTypes = false;
            JSON.Parameters.EnableAnonymousTypes = true;
            JSON.Parameters.UseEscapedUnicode = false;
            JSON.Parameters.UseOptimizedDatasetSchema = true;

            return JSON.ToJSON(obj);
        }

        /// <summary>
        /// 通过Newtonsoft,序列化对象为Json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string JsonSerializeObjectWithNS(object obj)
        {
            var jSonStr = JsonConvert.SerializeObject(obj);

            return jSonStr;
        }
        #endregion

        #region JsonDeserializeObject 反序列化Json对象
        /// <summary>
        /// 反序列化Json对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static T JsonDeserializeObject<T>(string strJson)
        {
            JSON.Parameters.SerializeNullValues = true;
            JSON.Parameters.ShowReadOnlyProperties = true;
            JSON.Parameters.UseUTCDateTime = false;
            JSON.Parameters.UsingGlobalTypes = false;
            JSON.Parameters.EnableAnonymousTypes = true;

            return JSON.ToObject<T>(strJson);
        }

        #endregion

        #region JsonDeserializeObject 反序列化Json为dynamic对象
        /// <summary>
        /// 反序列化Json为dynamic对象
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static dynamic JsonDeserializeObject(string strJson)
        {
            JSON.Parameters.SerializeNullValues = true;
            JSON.Parameters.ShowReadOnlyProperties = true;
            JSON.Parameters.UseUTCDateTime = false;
            JSON.Parameters.UsingGlobalTypes = false;
            JSON.Parameters.EnableAnonymousTypes = true;

            return JSON.ToDynamic(strJson);
        }

        /// <summary>
        /// 通过Newtonsoft,反序列化Json为JObject对象
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static JObject JsonDeserializeObjectWithNS(string strJson)
        {
            var jObject = new JObject();
            //判断是否为json字符串，是则进行转换
            if (strJson.IndexOf('{') == -1 || string.IsNullOrWhiteSpace(strJson))
            {
                return jObject;
            }
            jObject = (JObject)JsonConvert.DeserializeObject(strJson);
            return jObject;
        }


        #endregion

        #region ShortUrl 生成长度为6的短地址字符串
        /// <summary>
        /// 生成长度为6的短地址字符串
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string[] ShortUrl(string url)
        {
            //可以自定义生成MD5加密字符传前的混合KEY 
            string key = "ShengYiZhuanJia";
            //要使用生成URL的字符 
            var chars = new string[]{ 
                 "a","b","c","d","e","f","g","h", 
                 "i","j","k","l","m","n","o","p", 
                 "q","r","s","t","u","v","w","x", 
                 "y","z","0","1","2","3","4","5", 
                 "6","7","8","9","A","B","C","D", 
                 "E","F","G","H","I","J","K","L", 
                 "M","N","O","P","Q","R","S","T", 
                 "U","V","W","X","Y","Z" 
              };
            //对传入网址进行MD5加密 
            string hex = Md5Hash(key + url).ToUpper();
            var resUrl = new string[4];
            for (int i = 0; i < 4; i++)
            {
                //把加密字符按照8位一组16进制与0x3FFFFFFF进行位与运算 
                int hexint = 0x3FFFFFFF & Convert.ToInt32("0x" + hex.Substring(i * 8, 8), 16);
                string outChars = string.Empty;
                for (int j = 0; j < 6; j++)
                {
                    //把得到的值与0x0000003D进行位与运算，取得字符数组chars索引 
                    int index = 0x0000003D & hexint;
                    //把取得的字符相加 
                    outChars += chars[index];
                    //每次循环按位右移5位 
                    hexint = hexint >> 5;
                }
                //把字符串存入对应索引的输出数组 
                resUrl[i] = outChars;
            }
            return resUrl;
        } 
        #endregion

        #region VerPrefixName 版本名称处理
        /// <summary>
        /// 设置版本显示名称
        /// </summary>
        /// <param name="accVer">店铺版本</param>
        /// <param name="AgentId">代理商</param>
        /// <param name="VerprefixName">版本前缀</param>
        /// <returns></returns>
        public static string SetVerPrefixName(int accVer, int AgentId, string VerprefixName)
        {
            var VN = "免费版";
            if (accVer == 2)
            {
                VN = "标准版";
            }
            else if (accVer == 3)
            {
                if (AgentId == AgentIdW || AgentId == 28)
                {
                    VN = "专家版";
                }
                else
                {
                    VN = "高级版";
                }
            }
            else if (accVer == 4)
            {
                VN = "专家版";
            }
            else if (accVer == 5)
            {
                VN = "行业版";
            }
            if (VerprefixName != null && VerprefixName.Length > 0)
            {
                VN = VerprefixName + VN;
            }
            return VN;
        }
        #endregion

        public static string FormatMoney(double? money)
        {
            if (!money.HasValue)
            {
                return "0.00";
            }

            return money.Value.ToString("F2");
        }

        #region RestRequest Http Rest请求
        /// <summary>
        /// Http Rest请求
        /// </summary>
        /// <param name="baseUrl">Host</param>
        /// <param name="resource">RecoureUrl</param>
        /// <param name="method">Method</param>
        /// <param name="parameters">Parameters</param>
        /// <param name="headers">Headers</param>
        /// <returns></returns>
        public static string RestRequest(string baseUrl, string resource, Method method, Dictionary<string, string> parameters, Dictionary<string, string> headers = null)
        {
            string strResult = "";

            var client = new RestClient(baseUrl);
            var request = new RestRequest(resource, method);
            if (headers != null)
            {
                if (headers.Count > 0)
                {
                    foreach (var headItem in headers)
                    {
                        request.AddHeader(headItem.Key, headItem.Value);
                    }
                }
            }

            if (method == Method.POST || method == Method.PUT)
            {
                if (parameters != null)
                {
                    if (parameters.Count > 0)
                    {
                        {
                            foreach (var item in parameters)
                            {
                                request.AddParameter(item.Key, item.Value);
                            }
                        }
                        
                    }
                }
            }

            if (method == Method.GET)
            {
                if (parameters != null)
                {
                    if (parameters.Count > 0)
                    {
                        foreach (var item in parameters)
                        {
                            request.AddQueryParameter(item.Key, item.Value);
                        }
                    }
                }
            }

            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                strResult = response.Content;
            }

            return strResult;
        } 
        #endregion

        #region RestPost HttpPost请求
        /// <summary>
        /// HttpPost请求
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="resource"></param>
        /// <param name="parameters"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string RestPost(string baseUrl, string resource, Dictionary<string, string> parameters, Dictionary<string, string> headers = null)
        {
            var method = Method.POST;
            return RestRequest(baseUrl, resource, method, parameters, headers);
        } 
        #endregion

        #region RestGet HtppGet请求
        /// <summary>
        /// HtppGet请求
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="resource"></param>
        /// <param name="parameters"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string RestGet(string baseUrl, string resource, Dictionary<string, string> parameters, Dictionary<string, string> headers = null)
        {
            var method = Method.GET;
            return RestRequest(baseUrl, resource, method, parameters, headers);
        }
        #endregion

        #region GetDecimal 获取decimal        

        public static decimal GetDecimal(string key, decimal defaultVal = 0m)
        {
            decimal value = default(decimal);
            if ((decimal.TryParse(key, out value)))
            {
                return value;
            }
            else
            {
                return defaultVal;
            }
        }
        #endregion

        #region GetInt32 获取int

        public static int GetInt32(string key, int defaultVal = 0)
        {
            int value = default(int);
            if ((int.TryParse(key, out value)))
            {
                return value;
            }
            else
            {
                return defaultVal;
            }
        }
        #endregion

        #region IsLongLifeAccId 是否是终身版高级版

        public static bool IsLongLifeAccId(int accId)
        {
            bool bResult = false;
            List<int> longLiftList = new List<int>
            {
                817,
                2002,
                8640,
                13091,
                13173,
                20591,
                21594,
                23333,
                23523,
                23627,
                24555,
                25755,
                26271,
                29388,
                30267,
                31698,
                31895,
                33504,
                33807,
                34044,
                35160,
                35483,
                37170,
                37250,
                37705,
                41079,
                42152,
                44242,
                45600,
                47718,
                49944,
                54795,
                62145,
                62775,
                63802,
                66315,
                69991,
                72386,
                77404,
                77773,
                79944,
                88764,
                90923,
                91270,
                91320,
                92629,
                95018,
                101240,
                103522,
                107058,
                107136,
                107865,
                109633,
                112632,
                119815,
                120025,
                122568,
                123534,
                126365,
                128552,
                128613,
                131110,
                133413,
                133415,
                133589,
                135179,
                138185,
                143256,
                144816,
                145724,
                146932,
                147128,
                147226,
                148093,
                148471,
                149672,
                151095,
                153031,
                153335,
                154463,
                154501,
                154786,
                155114,
                156760,
                157413,
                157191,
                152524,
                29306,
                29307,
                51227,
                55340,
                143889
            };
            if (longLiftList.Contains(accId))
            {
                bResult = true;
            }
            return bResult;
        }

        #endregion

        #region IsOdd 判断奇数偶数
        //判断奇数偶数
        public static bool IsOdd(int n)
        {
            return Convert.ToBoolean(n & 1);
        } 
        #endregion

        #region decimal转为中文大写汉字
        /// <summary>
        /// decimal转为中文大写汉字
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private static string DecimalToCHS(decimal num)
        {
            string str1 = "零壹贰叁肆伍陆柒捌玖";            //0-9所对应的汉字 
            string str2 = "万仟佰拾亿仟佰拾万仟佰拾元角分"; //数字位所对应的汉字 
            string str3 = "";    //从原num值中取出的值 
            string str4 = "";    //数字的字符串形式 
            string str5 = "";  //人民币大写金额形式 
            int i;    //循环变量 
            int j;    //num的值乘以100的字符串长度 
            string ch1 = "";    //数字的汉语读法 
            string ch2 = "";    //数字位的汉字读法 
            int nzero = 0;  //用来计算连续的零值是几个 
            int temp;            //从原num值中取出的值

            num = Math.Round(Math.Abs(num), 2);    //将num取绝对值并四舍五入取2位小数 
            str4 = ((long)(num * 100)).ToString();        //将num乘100并转换成字符串形式 
            j = str4.Length;      //找出最高位 
            if (j > 15) { return "溢出"; }
            str2 = str2.Substring(15 - j);   //取出对应位数的str2的值。如：200.55,j为5所以tr2=佰拾元角分

            //循环取出每一位需要转换的值 
            for (i = 0; i < j; i++)
            {
                str3 = str4.Substring(i, 1);          //取出需转换的某一位的值 
                temp = Convert.ToInt32(str3);      //转换为数字 
                if (i != (j - 3) && i != (j - 7) && i != (j - 11) && i != (j - 15))
                {
                    //当所取位数不为元、万、亿、万亿上的数字时 
                    if (str3 == "0")
                    {
                        ch1 = "";
                        ch2 = "";
                        nzero = nzero + 1;
                    }
                    else
                    {
                        if (str3 != "0" && nzero != 0)
                        {
                            ch1 = "零" + str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                    }
                }
                else
                {
                    //该位是万亿，亿，万，元位等关键位 
                    if (str3 != "0" && nzero != 0)
                    {
                        ch1 = "零" + str1.Substring(temp * 1, 1);
                        ch2 = str2.Substring(i, 1);
                        nzero = 0;
                    }
                    else
                    {
                        if (str3 != "0" && nzero == 0)
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            if (str3 == "0" && nzero >= 3)
                            {
                                ch1 = "";
                                ch2 = "";
                                nzero = nzero + 1;
                            }
                            else
                            {
                                if (j >= 11)
                                {
                                    ch1 = "";
                                    nzero = nzero + 1;
                                }
                                else
                                {
                                    ch1 = "";
                                    ch2 = str2.Substring(i, 1);
                                    nzero = nzero + 1;
                                }
                            }
                        }
                    }
                }
                if (i == (j - 11) || i == (j - 3))
                {
                    //如果该位是亿位或元位，则必须写上 
                    ch2 = str2.Substring(i, 1);
                }
                str5 = str5 + ch1 + ch2;

                if (i == j - 1 && str3 == "0")
                {
                    //最后一位（分）为0时，加上“整” 
                    str5 = str5 + '整';
                }
            }
            if (num == 0)
            {
                str5 = "零元整";
            }
            return str5;
        }
        #endregion

        /// <summary>
        /// 有密码的AES加密 
        /// </summary>
        /// <param name="text">加密字符</param>
        /// <param name="password">加密的密码</param>
        /// <param name="iv">密钥</param>
        /// <returns></returns>
        public static string AesEncrypt(string text, string password, string iv)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();

            rijndaelCipher.Mode = CipherMode.CBC;

            rijndaelCipher.Padding = PaddingMode.PKCS7;

            rijndaelCipher.KeySize = 128;

            rijndaelCipher.BlockSize = 128;

            byte[] pwdBytes = Encoding.UTF8.GetBytes(password);

            byte[] keyBytes = new byte[16];

            int len = pwdBytes.Length;

            if (len > keyBytes.Length) len = keyBytes.Length;

            Array.Copy(pwdBytes, keyBytes, len);

            rijndaelCipher.Key = keyBytes;


            byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
            rijndaelCipher.IV = ivBytes;

            ICryptoTransform transform = rijndaelCipher.CreateEncryptor();

            byte[] plainText = Encoding.UTF8.GetBytes(text);

            byte[] cipherBytes = transform.TransformFinalBlock(plainText, 0, plainText.Length);

            return Convert.ToBase64String(cipherBytes);
        }

        /// <summary>
        /// 随机生成密钥
        /// </summary>
        /// <returns></returns>
        public static string GetIv(int n)
        {
            char[] arrChar = new char[]
            {
                'a', 'b', 'd', 'c', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'p', 'r', 'q', 's', 't', 'u', 'v',
                'w', 'z', 'y', 'x',
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'Q', 'P', 'R', 'T', 'S', 'V', 'U',
                'W', 'X', 'Y', 'Z'
            };

            StringBuilder num = new StringBuilder();

            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < n; i++)
            {
                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());
            }

            return num.ToString();
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="text"></param>
        /// <param name="password"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string AesDecrypt(string text, string password, string iv)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();

            rijndaelCipher.Mode = CipherMode.CBC;

            rijndaelCipher.Padding = PaddingMode.PKCS7;

            rijndaelCipher.KeySize = 128;

            rijndaelCipher.BlockSize = 128;

            byte[] encryptedData = Convert.FromBase64String(text);

            byte[] pwdBytes = Encoding.UTF8.GetBytes(password);

            byte[] keyBytes = new byte[16];

            int len = pwdBytes.Length;

            if (len > keyBytes.Length) len = keyBytes.Length;

            Array.Copy(pwdBytes, keyBytes, len);

            rijndaelCipher.Key = keyBytes;

            byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
            rijndaelCipher.IV = ivBytes;

            ICryptoTransform transform = rijndaelCipher.CreateDecryptor();

            byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);

            return Encoding.UTF8.GetString(plainText);
        }

        #region 等数拆分
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunksize)
        {
            while (source.Any())
            {
                yield return source.Take(chunksize);
                source = source.Skip(chunksize);
            }
        }
        #endregion

        /// <summary>
        ///     根据公历,返回对应农历日期
        /// </summary>
        /// <param name="date">公历日期</param>
        /// <returns></returns>
        public static ChinaDate ConvertToLunisolar(DateTime date)
        {
            ChineseLunisolarCalendar calendar = new ChineseLunisolarCalendar();
            if (date > calendar.MaxSupportedDateTime || date < calendar.MinSupportedDateTime)
            {
                //日期范围：1901 年 2 月 19 日 - 2101 年 1 月 28 日
                throw new Exception(string.Format("日期超出范围！必须在{0}到{1}之间！",
                    calendar.MinSupportedDateTime.ToString("yyyy-MM-dd"),
                    calendar.MaxSupportedDateTime.ToString("yyyy-MM-dd")));
            }

            int iYear = calendar.GetYear(date);
            int iMonth = calendar.GetMonth(date);
            int leapMonth = calendar.GetLeapMonth(iYear);
            //判断是否闰月
            var isLeapMonth = iMonth == leapMonth;
            if (leapMonth != 0 && iMonth >= leapMonth)
            {
                iMonth--;
            }
            int iDay = calendar.GetDayOfMonth(date);

            var chinaDate = new ChinaDate();
            chinaDate.Year = iYear;
            chinaDate.Month = iMonth;
            chinaDate.Day = iDay;

            return chinaDate;
        }

        /// <summary>
        ///     根据公历,返回对应农历日期
        /// </summary>
        /// <param name="dt">公历日期</param>
        /// <returns></returns>
        public static string ConvertToLunisolarDate(DateTime dt)
        {
            ChineseLunisolarCalendar calendar = new ChineseLunisolarCalendar();
            if (dt > calendar.MaxSupportedDateTime || dt < calendar.MinSupportedDateTime)
            {
                //日期范围：1901 年 2 月 19 日 - 2101 年 1 月 28 日
                throw new Exception(string.Format("日期超出范围！必须在{0}到{1}之间！",
                    calendar.MinSupportedDateTime.ToString("yyyy-MM-dd"),
                    calendar.MaxSupportedDateTime.ToString("yyyy-MM-dd")));
            }

            int iYear = calendar.GetYear(dt);
            int iMonth = calendar.GetMonth(dt);
            int leapMonth = calendar.GetLeapMonth(iYear);
            //判断是否闰月
            var isLeapMonth = iMonth == leapMonth;
            if (leapMonth != 0 && iMonth >= leapMonth)
            {
                iMonth--;
            }
            int iDay = calendar.GetDayOfMonth(dt);

            return (iYear + "-" + iMonth + "-" + iDay);
        }

        /// <summary>
        ///     根据农历,返回对应公历日期
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <param name="day">天</param>
        /// <returns></returns>
        public static DateTime ConvertLunisolarToDate(int year, int month, int day)
        {
            var calendar = new ChineseLunisolarCalendar();
            try
            {
                int leapMonth = calendar.GetLeapMonth(year);

                //bool isLeapMonth = month == leapMonth;
                var iMonth = month;
                if (leapMonth != 0 && month >= leapMonth)
                {
                    iMonth++;
                }
                return calendar.ToDateTime(year, iMonth, day, 0, 0, 0, 0);
            }
            catch (Exception ex)
            {
                //Logger.Error(string.Format("农历日期转换错误[{0}-{1}-{2}]:", year, month, day), ex);
                return Convert.ToDateTime("1900-1-1");
            }
        }

        /// <summary>
        ///     农历
        /// </summary>
        public class ChinaDate
        {
            /// <summary>
            ///     年份
            /// </summary>
            public int Year { get; set; }

            /// <summary>
            ///     月份
            /// </summary>
            public int Month { get; set; }

            /// <summary>
            ///     天
            /// </summary>
            public int Day { get; set; }
        }
    }

}
