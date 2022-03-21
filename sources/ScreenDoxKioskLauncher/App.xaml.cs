using Autofac;

using Common.Logging;

using FrontDesk.Common.InfrastructureServices;

using ScreenDoxKioskLauncher.Commands;
using ScreenDoxKioskLauncher.Controllers;
using ScreenDoxKioskLauncher.Infrastructure;
using ScreenDoxKioskLauncher.Services;

using System.Windows;

namespace ScreenDoxKioskLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ILog _logger = LogManager.GetLogger("default");

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var container = RegisterContainer();

            this.DispatcherUnhandledException += App_DispatcherUnhandledException;

            using (var scope = container.BeginLifetimeScope())
            {
                var window = scope.Resolve<MainWindow>();
                window.Show();
            }
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {

            _logger.ErrorFormat("Unhandled exception has occured.", e.Exception);


            e.Handled = true; //handle exception

        }

        protected IContainer RegisterContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EnvironmentProvider>().As<IEnvironmentProvider>().InstancePerLifetimeScope();
            builder.RegisterType<KioskInstallApiClient>().As<IKioskInstallApiClient>();
            builder.RegisterType<CommandFactory>().AsSelf();


            //services
            builder.RegisterType<TimeService>().As<ITimeService>();
            builder.RegisterType<PackageDownloadService>().As<IPackageDownloadService>();
            builder.RegisterType<UpgradeScheduleService>().As<IUpgradeScheduleService>();
            builder.RegisterType<KioskAppManagerService>().As<IKioskAppManagerService>();
            builder.RegisterType<FileManagementService>().As<IFileManagementService>();
            builder.RegisterType<KioskHealthService>().As<IKioskHealthService>();
            builder.RegisterType<ApplicationStateService>().As<IApplicationStateService>().SingleInstance();

            // controllers
            builder.RegisterType<CheckNewVersionAvailableController>().AsSelf();
            builder.RegisterType<UpgradeToNewVersionController>().AsSelf();
            builder.RegisterType<KioskAppAutoStartController>().AsSelf();

            // Add the MainWindowclass and later resolve
            builder.RegisterType<MainWindow>().AsSelf().PropertiesAutowired();

            var container = builder.Build();

            return container;
        }
    }
}
