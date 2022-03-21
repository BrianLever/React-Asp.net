using Autofac;
using Autofac.Integration.Wcf;
using Autofac.Integration.WebApi;

using Frontdesk.Server.SmartExport.Data;
using Frontdesk.Server.SmartExport.EhrInterfaceService;

using FrontDesk.Common.InfrastructureServices;
using FrontDesk.Common.Screening;
using FrontDesk.Configuration;
using FrontDesk.Server.Configuration;
using FrontDesk.Server.Controllers;
using FrontDesk.Server.Data;
using FrontDesk.Server.Data.BhsVisits;
using FrontDesk.Server.Data.Configuration;
using FrontDesk.Server.Data.KioskDataValues;
using FrontDesk.Server.Data.Logging;
using FrontDesk.Server.Data.ScreeningProfile;
using FrontDesk.Server.Data.ScreenngProfile;
using FrontDesk.Server.Licensing.Services;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Screening.Services;
using FrontDesk.Server.Services;
using FrontDesk.Services;

using RPMS.Common.GlobalConfiguration;
using RPMS.Common.Security;

using ScreenDox.EHR.Common;
using ScreenDox.EHR.Common.SmartExport.Repository;
using ScreenDox.Server.Common.Configuration;
using ScreenDox.Server.Common.Data;
using ScreenDox.Server.Common.Infrastructure;
using ScreenDox.Server.Common.Services;
using ScreenDox.Server.Data;
using ScreenDox.Server.Formatters;
using ScreenDox.Server.Logging;
using ScreenDox.Server.Screening.Services;
using ScreenDox.Server.Security;
using ScreenDox.Server.Security.Services;
using ScreenDox.Server.Services;

using System;
using System.Reflection;
using System.Web.Http;

using Web.Api.Infrastructure.Auth;

namespace ScreenDox.Server.Api
{
    /// <summary>
    /// Dependency config for IoC
    /// </summary>
    public static class DepedencyConfig
    {
        /// <summary>
        /// Register dependencies
        /// </summary>
        public static IContainer Register(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);
            builder.RegisterWebApiModelBinderProvider();


            //register formatters
            builder.RegisterType<TodayDateFormatter>().AsSelf().SingleInstance();

            //register repositories
            builder.RegisterType<UserPrincipalDb>().As<IUserPrincipalRepository>();

            builder.RegisterType<ScreensDb>().As<IScreensRepository>();
            builder.RegisterType<ScreeningResultDb>().As<IScreenerResultReadRepository>();
            builder.RegisterType<ScreeningResultDb>().As<IScreenerResultRepository>();
            builder.RegisterType<UserPrincipalDb>().As<IUserPrincipalRepository>();
            builder.RegisterType<BhsVisitDb>().As<IBhsVisitRepository>();
            builder.RegisterType<BhsDemographicsDb>().As<IBhsDemographicsRepository>();
            builder.RegisterType<BranchLocationDb>().As<IBranchLocationDb>();
            builder.RegisterType<KioskDatabase>().As<IKioskRepository>();
            builder.RegisterType<BhsVisitDb>().As<IBhsVisitRepository>();
            builder.RegisterType<BhsHistoryDb>().As<IBhsHistoryRepository>();
            builder.RegisterType<SecurityLogDb>().As<ISecurityLogRepository>();
            builder.RegisterType<BhsFollowUpDb>().As<IBhsFollowUpRepository>();
            builder.RegisterType<ScreeningProfileDb>().As<IScreeningProfileRepository>();
            builder.RegisterType<ScreeningProfileFrequencyDb>().As<IScreeningProfileFrequencyRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ScreeningProfileMinimalAgeDatabase>().As<IScreeningProfileMinimalAgeRepository>().InstancePerLifetimeScope();
            builder.RegisterType<VisitSettingsDatabase>().As<IVisitSettingsRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ScreeningScoreDb>().As<IScreeningScoreRepository>();
            builder.RegisterType<ScreeningAgesDbProvider>().As<IScreeningAgesSettingsProvider>().InstancePerLifetimeScope();
            builder.RegisterType<IndicatorReportDb>().As<IIndicatorReportRepository>();
            builder.RegisterType<ErrorLogDb>().As<IErrorLogRepository>();
            builder.RegisterType<UserRefreshTokenDb>().As<IUserRefreshTokenRepository>();
            builder.RegisterType<SecurityQuestionDb>().As<ISecurityQuestionRepository>();
            builder.RegisterType<ColumbiaSuicideReportDb>().As<IColumbiaSuicideReportRepository>();
            builder.RegisterType<RpmsCredentialsRepository>().As<IRpmsCredentialsRepository>();
            builder.RegisterType<GlobalSettingsDatabase>().As<IGlobalSettingsRepository>();
            builder.RegisterType<PatientNameCorrectionLogDb>().As<IPatientNameCorrectionLogRepository>();
            builder.RegisterType<SmartExportDb>().As<ISmartExportRepository>();
            builder.RegisterType<ScreeningProfileMinimalAgeDatabase>().As<IKioskMinimalAgeRepository>();
            builder.RegisterType<ErrorLogDb>().As<IErrorLogRepository>();
            builder.RegisterType<ScreeningTimeLogDb>().As<IScreeningTimeLogRepository>();
            builder.RegisterType<PatientScreeningFrequencyDatabase>().As<IPatientScreeningFrequencyRepository>();
            builder.RegisterType<LookupValuesDeleteLogDb>().As<ILookupValuesDeleteLogDb>();

            // register services
            builder.RegisterType<UserPrincipalService>().As<IUserPrincipalService>().InstancePerLifetimeScope();
            builder.RegisterType<LicenseService>().As<ILicenseService>().SingleInstance();
            builder.RegisterType<TimeService>().As<ITimeService>().SingleInstance().SingleInstance();
            builder.RegisterType<ScreeningDefinitionService>().As<IScreeningDefinitionService>().SingleInstance();
            builder.RegisterType<ScreenService>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<ScreenService>().As<IScreenService>().InstancePerLifetimeScope();
            builder.RegisterType<BhsDemographicsService>().As<IBhsDemographicsService>();
            builder.RegisterType<BranchLocationService>().As<IBranchLocationService>();
            builder.RegisterType<KioskService>().As<IKioskService>();
            builder.RegisterGeneric(typeof(DataCacheService<>)).As(typeof(IDataCacheService<>)).InstancePerLifetimeScope();
            builder.RegisterType<ScreeningResultService>().As<IScreeningResultService>();
            builder.RegisterType<BhsVisitFactory>().As<IBhsVisitFactory>().InstancePerLifetimeScope();
            builder.RegisterType<VisitCreator>().As<IVisitCreator>().InstancePerLifetimeScope();
            builder.RegisterType<BhsDemographicsService>().As<IBhsDemographicsService>();
            builder.RegisterType<BhsFollowUpService>().As<IBhsFollowUpService>();
            builder.RegisterType<BhsFollowUpFactory>().As<IBhsFollowUpFactory>().InstancePerLifetimeScope();
            builder.RegisterType<VisitService>().As<IVisitService>();
            builder.RegisterType<FollowUpService>().As<IFollowUpService>();
            builder.RegisterType<LookupListsDataSource>().As<ILookupListsDataSource>().InstancePerLifetimeScope();
            builder.RegisterType<SecurityLogService>().As<ISecurityLogService>().InstancePerLifetimeScope();
            builder.RegisterType<TypeaheadDataSource>().As<ITypeaheadDataSourceFactory>();
            builder.RegisterType<ScreeningProfileService>().As<IScreeningProfileService>();
            builder.RegisterType<ScreeningProfileFrequencyService>().As<IScreeningProfileFrequencyService>();
            builder.RegisterType<ScreeningProfileMinimalAgeService>().As<IScreeningProfileMinimalAgeService>();
            builder.RegisterType<VisitSettingsService>().As<IVisitSettingsService>();
            builder.RegisterType<ScreeningScoreLevelService>().As<IScreeningScoreLevelService>();
            builder.RegisterType<SystemSettingService>().As<ISystemSettingService>().InstancePerLifetimeScope();
            builder.RegisterType<IndicatorReportService>().As<IIndicatorReportService>();
            builder.RegisterType<ErrorLogService>().As<IErrorLogService>();
            builder.RegisterType<JwtFactory>().As<IJwtFactory>();
            builder.RegisterType<TokenFactory>().As<ITokenFactory>();
            builder.RegisterType<AuthService>().As<IAuthService>();
            builder.RegisterInstance(EhrInterfaceProxy.Instance).As<IEhrInterfaceProxy>().SingleInstance();
            builder.Register<IValidatePatientRecordService>(cc => {
                var context = cc.Resolve<IComponentContext>();
                return context.Resolve<IEhrInterfaceProxy>();
            }).InstancePerLifetimeScope();

            builder.RegisterType<PatientNameCorrectionLogService>().As<IPatientNameCorrectionLogService>();

            builder.RegisterType<ColumbiaSuicideReportService>().As<IColumbiaSuicideReportService>();
            builder.RegisterType<PatientService>().As<IPatientService>();
            builder.RegisterType<CryptographyService>().As<ICryptographyService>();
            builder.RegisterType<RpmsCredentialsService>().As<IRpmsCredentialsService>();
            builder.RegisterType<DefaultDateService>().As<IDateService>();
            builder.RegisterType<GlobalSettingsService>().As<IGlobalSettingsService>();

            builder.RegisterType<RpmsExportController>().AsSelf();

            builder.RegisterType<ScreenDox.Server.Controllers.ScreeningResultCreator>().AsSelf();
            builder.RegisterType<ScreenDox.Server.Services.KioskEndpoint>().As<ScreenDox.Server.Services.IKioskEndpoint>();
            builder.RegisterType<ScreenDox.Server.Services.KioskEndpoint>();
            builder.RegisterType<ErrorLoggerService>().As<IErrorLoggerService>();
            builder.RegisterType<ScreeningTimeLogService>().As<IScreeningTimeLogService>();
            builder.RegisterType<PatientScreeningFrequencyService>().As<IPatientScreeningFrequencyService>()
                .WithParameter(
                    (pi, ctx) => pi.ParameterType == typeof(Func<IPatientScreeningFrequencyRepository>) && pi.Name == "resultsRepositoryFactoryMethod",
                    (pi, ctx) => {
                    var context = ctx.Resolve<IComponentContext>();
                        Func<IPatientScreeningFrequencyRepository> f = () => context.Resolve<IPatientScreeningFrequencyRepository>();
                        return f;
                    }
                );
            builder.RegisterType<LookupListsDataSource>().As<ILookupListsDataSource>().SingleInstance();
            builder.RegisterType<TypeaheadDataSource>().As<ITypeaheadDataSourceFactory>().SingleInstance();




            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // Autofac-injected middleware registered with the container.

            AutofacHostFactory.Container = container;

            return container;

        }
    }
}