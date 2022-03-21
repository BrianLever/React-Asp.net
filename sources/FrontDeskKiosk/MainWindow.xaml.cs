using Common.Logging;

using FrontDesk.Kiosk.Controllers;
using FrontDesk.Kiosk.Discovery;
using FrontDesk.Kiosk.Screens;
using FrontDesk.Kiosk.Services;
using FrontDesk.Kiosk.Settings;
using FrontDesk.Kiosk.Workflow;

using Ninject;

using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace FrontDesk.Kiosk
{
    public partial class MainWindow : Window, IMainWindow
    {
        private readonly IKernel Container;
        private readonly ILog _logger = LogManager.GetLogger<MainWindow>();

        private readonly IUserSessionTimeoutController _userSessionTimeoutController;
        private readonly IDataSyncController _lookupTableDataSyncController = new LookupTableDataSyncController();
        private readonly ISelfDiscoveryService _selfDiscoveryService = new SelfDiscoveryService();

        private string WindowTitle = "ScreenDox Health Behavioral Screener";
        private readonly ErrorNotificationController _errorNotificationController;

        public bool IsSucceedFlow { get; } = true;

        public MainWindow()
        {
            InitializeComponent();

            _logger.Debug("[MAIN WINDOW START] Initializing contructor...");
            //create IoC container and resolve all singleton objects
            Container = (IKernel)Bootstrap.Bootstrapper.Container;
            _userSessionTimeoutController = Container.Get<IUserSessionTimeoutController>();
            _errorNotificationController = Container.Get<ErrorNotificationController>();

            this.PreviewMouseDown += new MouseButtonEventHandler(MainWindow_PreviewMouseDown);
            this.Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);
            this.Deactivated += MainWindow_Deactivated;
            this.PreviewLostKeyboardFocus += MainWindow_PreviewLostKeyboardFocus;

            if (AppSettings.WindowModeEnabled)
            {
                WindowStyle = WindowStyle.SingleBorderWindow;
                WindowState = WindowState.Normal;
                this.SizeChanged += MainWindow_SizeChanged;
            }

            _logger.Debug("[MAIN WINDOW START] Contructor initialized...");
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Title = string.Format("{0} | W: {1}px H:{2}px", WindowTitle, e.NewSize.Width, e.NewSize.Height);
        }

        // Keep user's session unexprired
        void MainWindow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //update last user's activity time to not expire user's session
            _userSessionTimeoutController.LastUsedTime = DateTime.Now;
        }
        // Stop monitoring user session expiration time
        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _userSessionTimeoutController.StopMonitoring();
            OutOfServiceController.Instance.Stop();
            ScreeningMinimalAgeDataSyncController.Instance.Stop();
            _lookupTableDataSyncController.Stop();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // set wait cursor while loading
            this.Cursor = Cursors.AppStarting;

            _logger.Debug("[MAIN WINDOW START] Entering Window_Loaded...");

            //Initialize error notification controller
            _errorNotificationController.Initialize(spErrorContainer, txbErrorMessage);

            _logger.Debug("[MAIN WINDOW START] Initialized error notification controller.");

            //initialize user session timeout controller
            _userSessionTimeoutController.LastUsedTime = DateTime.Now;
            _userSessionTimeoutController.SessionTimeoutPeriod = TimeSpan.FromSeconds(Settings.AppSettings.UserSessionTimeoutInSeconds);
            _userSessionTimeoutController.SessionExpiringNotificationTimeout = _userSessionTimeoutController.SessionTimeoutPeriod - TimeSpan.FromSeconds(Settings.AppSettings.PressMeButtonTimeBeforeUserSessionTimeoutInSeconds);

            _logger.Debug("[MAIN WINDOW START] Initialized user session timeout controller.");

            // init control in seperate thread to not block the window
            Dispatcher.BeginInvoke(DispatcherPriority.DataBind, (Action)delegate () { InitializeScreenComponents(); });

            if (!Settings.AppSettings.ShowCursor)
            {
                //hide cursor
                this.Cursor = Cursors.None;
            }
            else
            {
                this.Cursor = Cursors.Arrow;
            }


            if (((App)(Application.Current)).IsFixedScreenSize)
            {
                this.Width = 1280;
                this.Height = 1024;
                WindowStartupLocation = WindowStartupLocation.Manual;
                Left = 0;
                Top = 0;
                this.WindowState = WindowState.Normal;
            }

            _logger.Debug("[MAIN WINDOW START] Setting app version...");
            if (AppSettings.DisplayVersionOnMainScreen)
            {
                txtVersion.Text = "Version " + _selfDiscoveryService.GetAppVersion();
            }
            txtVersion.Visibility = AppSettings.DisplayVersionOnMainScreen ? Visibility.Visible : Visibility.Collapsed;

            _logger.Debug("[MAIN WINDOW START] Completed Window_Loaded...");
        }

        private void InitializeScreenComponents()
        {
            // this code moved to unblocking thread because on some of the clinic-configured Kiosk initialization of SQL Compact connection 
            // takes about 15 secods and blocks UI.
            
            //initialize WF UI
            VisualScreenController visualScreenControler = Container.Get<VisualScreenController>();

            _logger.Debug("[MAIN WINDOW START] Begin WF UI initialization...");
            visualScreenControler.Initialize(this.grdScreens, grdOutOfServiceNotification, grdOutOfServiceFullScreen, ucSessionExpiringSoon);

            _logger.Debug("[MAIN WINDOW START] initialized WF UI.");

            _userSessionTimeoutController.StartMonitoring();

            _logger.Debug("[MAIN WINDOW START] User session timeout started monitoring.");

            //get min age settings
            ScreeningMinimalAgeDataSyncController.Instance.Start();
            _logger.Debug("[MAIN WINDOW START] Started .ScreeningMinimalAgeDataSyncController.");

            //sync lookup tables
            _lookupTableDataSyncController.Start();
            _logger.Debug("[MAIN WINDOW START] Started sync lookup tables.");


            // load cached data
            ScreenTypeAheadDataSources.Default.Refresh();
            _logger.Debug("[MAIN WINDOW START] Loaded cached data.");


            _logger.Debug("[MAIN WINDOW START] Initializing sound controller...");
            //init sound controller
            SoundController.Instance.Initialize(meSound);

            _logger.Debug("[MAIN WINDOW START] Playing startup sound...");
            //play startup sound
            SoundController.Instance.PlayStartupSound();

        }

        // code to prevent focus lost
        private void MainWindow_Deactivated(object sender, EventArgs e)
        {
            Window window = (Window)sender;
#if !DEV_MODE
            window.Topmost = true;
#endif
            _logger.Debug("Focus lost (Deactivated). Returned Top Most to Kiosk App...");
        }
        private void MainWindow_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Window window = (Window)sender;
#if !DEV_MODE
            window.Topmost = true;
#endif
            _logger.Debug("Focus lost (PreviewLostKeyboardFocus). Returned Top Most to Kiosk App...");
        }
    }
}
