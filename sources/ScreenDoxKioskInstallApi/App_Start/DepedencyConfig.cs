using Autofac;
using Autofac.Integration.WebApi;
using FrontDesk.Common.InfrastructureServices;
using ScreenDox.Server.Common.Data;
using ScreenDox.Server.Common.Services;

using ScreenDoxKioskInstallApi.Services;

using System.Reflection;
using System.Web.Http;

namespace ScreenDoxKioskInstallApi
{
    public static class DepedencyConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);
            builder.RegisterWebApiModelBinderProvider();


            //register repositories
            builder.RegisterType<KioskDatabase>().As<IKioskRepository>();

            // register services
            builder.RegisterType<TimeService>().As<ITimeService>();
            builder.RegisterType<KioskService>().As<IKioskService>();
            builder.RegisterType<InstallationPackageService>().AsSelf();


            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        } 
    }
}