using Ninject.Modules;
using FrontDesk.Kiosk.Workflow;
using FrontDesk.Kiosk.Screens;
using FrontDesk.Services;
using FrontDesk.Kiosk.Services;

namespace FrontDesk.Kiosk.IoC
{
	public class NinjectBootstrapper : NinjectModule
    {

        public override void Load()
        {
            Bind<IPatientScreeningFrequencyRepository>().To<PatientScreeningFrequencyRepository>().InSingletonScope();
            Bind<IScreeningFrequencySpecification>().To<ScreeningFrequencySpecification>().InSingletonScope();
            Bind<VisualScreenController>().ToSelf().InSingletonScope();
            Bind<IUserSessionTimeoutController>().To<UserSessionTimeoutController>().InSingletonScope();
            Bind<ErrorNotificationController>().ToSelf().InSingletonScope();
            Bind<IVisualScreenFactory>().To<VisualScreenFactory>();
            Bind<IScreeningResultState>().ToMethod((context) => ScreeningResultState.Instance);
            Bind<IScreeningSectionAgeService>().To<ScreeningSectionAgeService>();
            Bind<IPatientNameValidationService>().To<PatientNameValidationService>();
        }
    }
}
