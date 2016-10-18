using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommonLib
{
    /// <summary>
    /// 日志记录
    /// </summary>
   public static class HtmlBrowseLog
    {

       private delegate void HtmlBrowseDelegate(Dictionary<string, string> postData);


       public static void Add(HttpRequest Request)
       {
           bool addStat = false;
           if (addStat)
           {


               string Key = "";
               string Url = Request.Url.AbsoluteUri;
               if (Url.IndexOf(".aspx") > 0)
               {
                   Key = "I200.Aspx";
               }
               else if (Url.IndexOf("saleadd") > 0)
               {
                   Key = "SaleAdd";
               }
               else if (Url.IndexOf(".ashx") > 0)
               {
                   Key = "I200.Ashx";
               }
               if (Key.Length > 0)
               {
                   Dictionary<string, string> postData = new Dictionary<string, string>();
                   //来源页面
                   postData["SourcePage"] = Request.Url.PathAndQuery;
                   postData["SourceIP"] = Request.UserHostAddress;
                   postData["UserAgent"] = Request.UserAgent;
                   postData["Key"] = Key;
                   string PreviousSource = "";
                   var referror = Request.UrlReferrer;
                   if (referror != null)
                   {
                       PreviousSource = referror.PathAndQuery;
                   }
                   postData["PreviousSource"] = PreviousSource;
                   string userName = "";
                   HttpCookie cookie = Request.Cookies["username"];
                   if (cookie != null)
                   {
                       userName = cookie.Value;
                   }
                   postData["userName"] = userName;

                   Dictionary<string, object> OtherJson = new Dictionary<string, object>();
                   string[] Keys = Request.Form.AllKeys;
                   if (Keys.Count() > 1)
                   {
                       foreach (string key in Keys)
                       {
                           if (key != null)
                           {
                               OtherJson[key] = Request.Form[key].ToString();
                           }
                       }
                       postData["OtherJson"] = Helper.JsonSerializeObject(OtherJson);
                   }
                   else
                   {
                       postData["OtherJson"] = Request.Form.ToString();
                   }


                   var tokenDelegate = new HtmlBrowseDelegate(HtmlBrowseDispose);
                   tokenDelegate.BeginInvoke(postData, null, null);
               }
           }
       }


       public static void HtmlBrowseDispose(Dictionary<string,string> postData)
       {
           //string SourcePage, string SourceIP, string UserAgent, string PreviousSource, string OtherJson
           MongoDBAPI mgdbapi = new MongoDBAPI();
           mgdbapi.HtmlBrowse(postData["userName"], postData["SourcePage"], postData["SourceIP"], postData["UserAgent"], postData["PreviousSource"], postData["OtherJson"], postData["Key"]);

       }
    }
}
