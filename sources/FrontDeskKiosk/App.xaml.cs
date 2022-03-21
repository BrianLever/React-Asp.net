using Bootstrap.Ninject;

using Common.Logging;

using FrontDesk.Kiosk.Controllers;

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;

namespace FrontDesk.Kiosk
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ILog _logger = LogManager.GetLogger("default");
        public App()
        {
            this.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);

            this.Startup += new StartupEventHandler(App_Startup);
            this.Exit += new ExitEventHandler(App_Exit);

            System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);
        }

        void App_Exit(object sender, ExitEventArgs e)
        {
            //play closing sound
            SoundController.Instance.PlayClosingSound();
            OutOfServiceController.Instance.Stop();
            Thread.Sleep(3000);
        }

        public bool IsFixedScreenSize = false;

        void App_Startup(object sender, StartupEventArgs e)
        {

            //check for -demo_mode param
            if (e.Args.Contains("-fixed_screen"))
            {
                IsFixedScreenSize = true;
            }
            System.Environment.CurrentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location); //set current dir

            //MessageBox.Show(System.Environment.CurrentDirectory);

            //inialize bootstrapper
            Bootstrap.Bootstrapper.With.Ninject().Start();
            //test kiosk configuration

            OutOfServiceController.Instance.ServerConnectionEstablished += OutOfServiceController_ServerConnectionEstablished;


            DisplayMainWindow();

        }


        private void DisplayMainWindow()
        {
            var prevMain = this.MainWindow;

            try
            {
                if (TestKioskKey())
                {
                    //All works
                    this.MainWindow = new MainWindow();
#if DEV_MODE
                    this.MainWindow.Topmost = false;
#endif
                    _logger.Info("[STARTUP] Kiosk Key test has passed. Start main window.");

                }
                else
                {
                    this.MainWindow = new FailedToStartWindow();

                    OutOfServiceController.Instance.SetConnectionFailedState();

                    _logger.Warn("[STARTUP] Failed to start window.");
                }
            }
            catch (KioskConfigurationException ex)
            {
                _logger.Error("[STARTUP] Critical issue during the start. Show Failed to start window.", ex);

                //show error screen
                this.MainWindow = new FailedToStartWindow();
                OutOfServiceController.Instance.SetConnectionFailedState();
            }
            finally
            {

                if (prevMain != null)
                {
                    _logger.Debug("Closing previous main window...");

                    prevMain.Close();

                    _logger.Debug("Previous main window closed.");
                }
            }

            try
            {
                MainWindow.Show();

                var startSucceeded = (MainWindow as IMainWindow)?.IsSucceedFlow ?? false;

                _logger.Info("[STARTUP] Starting monitoring connection to the server and update kiosk activity.");
                OutOfServiceController.Instance.Start();

            }
            catch (Exception ex)
            {
                _logger.Error("[STARTUP] Failed to render Main window.", ex);

                ShowCriticalErrorScreen();
            }
        }


        private void OutOfServiceController_ServerConnectionEstablished(object sender, EventArgs e)
        {
            _logger.Info("Connection restored. Checking the current main window.");

            this.Dispatcher.Invoke(() =>
            {
                if (this.MainWindow is FailedToStartWindow)
                {
                    if (TestKioskKey())
                    {
                        DisplayMainWindow();
                    }
                    else
                    {
                        OutOfServiceController.Instance.SetConnectionFailedState();
                    }
                }
            });

        }


        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            _logger.ErrorFormat("[STARTUP] DispatcherUnhandledException has occured.", e.Exception);

            ShowCriticalErrorScreen();

            e.Handled = true; //handle exception
        }
        /// <summary>
        /// Show critical error screen
        /// </summary>
        private void ShowCriticalErrorScreen()
        {
            var oldMain = this.MainWindow;

            _logger.ErrorFormat("[STARTUP] Critical error during the kiosk start. Error message is shown");

            var screen = new FailedToStartWindow(FrontDesk.Kiosk.Properties.Resources.CriticalErrorMessage);

            this.MainWindow = screen;
            screen.Show();
            if (oldMain != null) oldMain.Close();
        }


        private bool TestKioskKey()
        {
            var keyValid = false;
            string key = Settings.AppSettings.KioskKey;

            if (string.IsNullOrEmpty(key))
            {
                throw new KioskConfigurationException(FrontDesk.Kiosk.Properties.Resources.MissingKioskKeyMessage);
            }

            //call service and check kiok key
            var client = new KioskEndpointService.KioskEndpointClient();
            try
            {
                keyValid = KioskEndpointServiceClientFactory.Execute(c => c.TestKioskInstallation(key));
            }
            catch (Exception ex)
            {
                _logger.ErrorFormat("Failed to connect to WCF service", ex);

                throw new KioskConfigurationException(FrontDesk.Kiosk.Properties.Resources.FailedToVerifyKioskKey, ex);

            }
            if (!keyValid)
            {
                _logger.WarnFormat("Failed to register. Key is not valid.");

                _logger.WarnFormat(FrontDesk.Kiosk.Properties.Resources.InvalidKioskKey, key);
            }

            _logger.DebugFormat("Test Kiosk Installation completed. Result: {0}", keyValid);

            return keyValid;
        }
    }
}
