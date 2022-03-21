using Common.Logging;

using ScreenDoxKioskLauncher.Controllers;
using ScreenDoxKioskLauncher.Infrastructure;
using ScreenDoxKioskLauncher.Services;

using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Threading;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace ScreenDoxKioskLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        private readonly ILog _logger = LogManager.GetLogger<MainWindow>();
        private readonly IKioskAppManagerService _kioskAppManagerService;
        private readonly List<IController> _controllers = new List<IController>();

        private Timer _closeWindowTimer = new Timer(TimeSpan.FromSeconds(60).TotalMilliseconds);
        private bool disposedValue;
        private readonly SystemKeysManager _systemKeysManager = new SystemKeysManager();


        public MainWindow(
            CheckNewVersionAvailableController checkNewVersionAvailableController,
            UpgradeToNewVersionController upgradeToNewVersionController,
            KioskAppAutoStartController kioskAppAutoStartController,
            IKioskAppManagerService kioskAppManagerService)
        {
            InitializeComponent();

            _controllers.Add(kioskAppAutoStartController);
            _controllers.Add(checkNewVersionAvailableController);
            _controllers.Add(upgradeToNewVersionController);


            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
            Closed += MainWindow_Closed;
 
            _kioskAppManagerService = kioskAppManagerService ?? throw new ArgumentNullException(nameof(kioskAppManagerService));

            if (AppSettings.WindowModeEnabled)
            {
                WindowStyle = WindowStyle.SingleBorderWindow;
                WindowState = WindowState.Normal;
       
            }

        }

        private void _closeWindowTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //_kioskAppManagerService.StopKioskApp();
            _closeWindowTimer.Stop();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // stopping all open instances
            _kioskAppManagerService.StopKioskApp();
            
            _logger.Info("Starting all registered controllers...");
            _controllers.ForEach(x =>
            {
                Action action = () => x.Start();
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, action);
            });

            _logger.Info("All controllers have started.");


            _logger.Debug("Disabling system keys...");

            _systemKeysManager.DisableSystemKeys();

            _logger.Debug("System keys disabled...");
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
#if ALLOW_CLOSE
            // in debug mode allow closing application
            _logger.Info("Closing Launcher application...");
#else
            _logger.Warn("Attempt to close the Launcher application. Rejecting...");
            // prevent closing 
            e.Cancel = true;

#endif
            return;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            // Might be never executed of 
            OnApplicationClosed();
        }
        private void OnApplicationClosed()
        {
            _logger.Debug("Enabling system keys...");

            _systemKeysManager.EnableSystemKeys();

            _logger.Debug("System keys enabled...");

            _logger.Info("Stopping all registered controllers...");
            _controllers.ForEach(x => x.Stop());
            _logger.Info("All controllers have stopped.");
        }

        #region IDisposable pattern

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                   _closeWindowTimer.Dispose();
                    _systemKeysManager.Dispose();
                }

                _closeWindowTimer = null;

                disposedValue = true;
            }
        }

        ~MainWindow()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
