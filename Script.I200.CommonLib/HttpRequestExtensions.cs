using System.Linq;
using System.Web;

namespace CommonLib
{
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// 从Params 中获取查询值 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValue(this HttpRequest request, string key)
        {
            if (!request.Params.AllKeys.Contains(key))
                return string.Empty;

            return request.Params[key].Trim();
        }
    }
}
