using Ninject.Modules;
using RPMS.Common;
using RPMS.Common.Export.Factories;

namespace RPMS.FrontDesk
{
    public class WCFNinjectModule : NinjectModule
    {
        public override void Load()
        {
            //Configure bindings
            #region Patient
            //Bind<IPatientRepository>().To<RPMS.Data.BMXNet.BmxPatientRepository>();
            //Bind<IPatientRepository>().To<RPMS.Data.CachePatientRepository>();
            Bind<IPatientRepository>().To<RPMS.Data.FakeObjects.FakePatientRepository>();

            Bind<IPatientService>().To<PatientService>();

            #endregion

            #region Visits
            //Bind<IVisitRepository>().To<RPMS.Data.BMXNet.BmxVisitRepository>();
            //Bind<IVisitRepository>().To<RPMS.Data.CacheVisitRepository>();
            Bind<IVisitRepository>().To<RPMS.Data.FakeObjects.FakeVisitRepository>();
            Bind<IVisitService>().To<VisitService>();
            #endregion

            #region Exports

            Bind<IScreeningResultProcessorFactory>().To<ScreeningResultProcessorFactory>().InSingletonScope();
            Bind<IScreeningExportService>().To<ScreeningExportService>();
            Bind<IScreeningResultsRepository>().To<RPMS.Data.FakeObjects.FakeScreeningResultsRepository>();
            //Bind<IScreeningResultsRepository>().To<RPMS.Data.CacheScreeningResultsRepository>();
            //Bind<IScreeningResultsRepository>().To<RPMS.Data.BMXNet.BmxScreeningResultsRepository>();

            #endregion

        }
    }
}