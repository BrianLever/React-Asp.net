
using Common.Logging;
using FrontDesk.Common.InfrastructureServices;
using FrontDeskRpmsService.Infrastructure;

using Ninject.Extensions.Interception.Infrastructure.Language;
using Ninject.Modules;

using RPMS.Common;
using RPMS.Common.Export.Factories;
using RPMS.Common.GlobalConfiguration;
using RPMS.Common.Security;
using RPMS.Data.CareBridge;
using ScreenDox.EHR.Common;
using ScreenDox.EHR.Common.SmartExport;
using ScreenDox.EHR.Common.SmartExport.Repository;

namespace RPMS.FrontDesk
{
    public class WCFNinjectModule : NinjectModule
    {
        public override void Load()
        {

            //Configure bindings
            Bind<ITimeService>().To<TimeService>().InSingletonScope();

            Bind<IPatientService>().To<PatientService>().Intercept().With<TimingInterceptor>();
            Bind<IScreeningResultProcessorFactory>().To<ScreeningResultProcessorFactory>().InSingletonScope();

            Bind<IVisitService>().To<VisitService>().Intercept().With<TimingInterceptor>();

            // patient name validation
            
            Bind<IPatientMatchRepository>().To<PatientMatchRepository>().Intercept().With<TimingInterceptor>();

            Bind<IPatientInfoMatchService>().To<PatientInfoMatchService>().Intercept().With<TimingInterceptor>();

            Bind<IPatientNameCorrectionLogRepository>().To<PatientNameCorrectionLogDb>();
            Bind<IPatientNameCorrectionLogService>().To<PatientNameCorrectionLogService>();
            Bind<IPatientValidationService>().To<PatientValidationService>().Intercept().With<TimingInterceptor>();
            
            Bind<IRpmsCredentialsRepository>().To<RpmsCredentialsRepository>();
            Bind<IRpmsCredentialsService>().To<RpmsCredentialsService>();
            Bind<ICryptographyService>().To<CryptographyService>();

            Bind<IGlobalSettingsRepository>().To<GlobalSettingsDatabase>().InThreadScope();
            Bind<IGlobalSettingsService>().To<GlobalSettingsService>().InThreadScope();


            var globalSettingsService = new GlobalSettingsService();

#if FAKE

            #region FAKE
            Bind<IScreeningExportService>().To<ScreeningExportService>().Intercept().With<TimingInterceptor>();
            Bind<IPatientRepository>().To<RPMS.Data.FakeObjects.FakePatientRepository>();
            Bind<IVisitRepository>().To<RPMS.Data.FakeObjects.FakeVisitRepository>();
            Bind<IScreeningResultsRepository>().To<RPMS.Data.FakeObjects.FakeScreeningResultsRepository>();

            #endregion
#else

            var externalSystem = globalSettingsService.ExternalEhrSystem;
            LogManager.GetLogger<WCFNinjectModule>().InfoFormat("ExternalEhrSystem = [{0}]", externalSystem);


            if (externalSystem == ExternalEhrSystemDescriptor.EhrRpms)
            {
                LogManager.GetLogger<WCFNinjectModule>().Info("Initializing RMPS/BMX connector.");

                //RPMS
                Bind<IScreeningExportService>().To<ScreeningExportService>().Intercept().With<TimingInterceptor>();

                // BMX
                Bind<IPatientRepository>().To<RPMS.Data.BMXNet.BmxPatientRepository>().Intercept().With<TimingInterceptor>();
                Bind<IVisitRepository>().To<RPMS.Data.BMXNet.BmxVisitRepository>().Intercept().With<TimingInterceptor>();
                Bind<IScreeningResultsRepository>().To<RPMS.Data.BMXNet.BmxScreeningResultsRepository>().Intercept().With<TimingInterceptor>();
            }

            else if (externalSystem == ExternalEhrSystemDescriptor.NextGen)
            {
                LogManager.GetLogger<WCFNinjectModule>().Info("Initializing NextGen/CareBridge connector.");

                Bind<IScreeningExportService>().To<CareBridgeScreeningExportService>().Intercept().With<TimingInterceptor>();


                // CareBridge
                Bind<IApiCredentialsService>().To<ApiCredentialsService>().InSingletonScope();

                Bind<IPatientRepository>().To<RPMS.Data.CareBridge.PatientRepository>().Intercept().With<TimingInterceptor>();

                Bind<IVisitRepository>().To<RPMS.Data.CareBridge.VisitRepository>().Intercept().With<TimingInterceptor>();
                Bind<IScreeningResultsRepository>().To<RPMS.Data.CareBridge.ScreeningResultsRepository>().Intercept().With<TimingInterceptor>();

            }
            else
            {
                //do nothing
                LogManager.GetLogger<WCFNinjectModule>().FatalFormat("Unknown external system configuration. ExternalEhrSystem = [{0}]", externalSystem);

            }
#endif

        }
    }
}