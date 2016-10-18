using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Script.I200.Application.Shared;

namespace Script.I200.WebAPI.App_Start
{
    public class DependencyRegistrar
    {
        public static void Register(HttpConfiguration config)
        {
            var build = new ContainerBuilder();


            build.RegisterType<SharedService>().As<ISharedService>().InstancePerRequest();

            var container = build.Build();
            var resolver = new AutofacWebApiDependencyResolver(container);
            config.DependencyResolver = resolver;
        }


    }
}