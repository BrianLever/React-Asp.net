using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Integration.Wcf;
using Common.Logging;
using FrontDesk.Common.InfrastructureServices;
using FrontDesk.Server.Mappers;
using FrontDesk.Server.Services;
using FrontDesk.StateObjects;
using Hangfire;
using Microsoft.Owin;
using Owin;
using ScreenDox.EHR.Common;
using ScreenDox.EHR.Common.SmartExport.Repository;

[assembly: OwinStartup(typeof(FrontDesk.Server.App.Startup))]

namespace FrontDesk.Server.App
{
    public class Startup
    {
        private ILog _logger = LogManager.GetLogger<Startup>();

        public void Configuration(IAppBuilder app)
        {

            var container = RegisterDependecies();
            // Autofac-injected middleware registered with the container.
            app.UseAutofacMiddleware(container);
            AutofacHostFactory.Container = container;

            _logger.Info("Registered dependencies.");

            AutoMapper.Mapper.Initialize(cfg => cfg.AddProfile<ScreeningMapCreator>());
        }

        public static IContainer RegisterDependecies()
        {
            var builder = new ContainerBuilder();
            
            // register services
            builder.RegisterType<KioskEndpoint>();
     
            // register depedencies
            builder.Register<IValidatePatientRecordService>(c => EhrInterfaceProxy.Instance).InstancePerLifetimeScope();


            builder.RegisterType<PatientNameCorrectionLogDb>().As< IPatientNameCorrectionLogRepository>();
            builder.RegisterType<TimeService>().As<ITimeService>();
            builder.RegisterType<PatientNameCorrectionLogService>().As<IPatientNameCorrectionLogService>();




            return builder.Build(); 
        }

    }
}
