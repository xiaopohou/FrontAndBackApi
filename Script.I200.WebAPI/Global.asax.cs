
using System;
using System.Web;
using System.Web.Http;
using NLog;

namespace Script.I200.WebAPI
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            MapperInit.InitMapping();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            // GlobalConfiguration.Configure(DependencyRegistrar.Register);
        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            try
            {
                var logger =LogManager.GetCurrentClassLogger();
                logger.Error(exception);
            }
            catch (Exception)
            {
               
            }
        }
    }
}