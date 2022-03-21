using System.Windows;
using System.Windows.Input;

using FrontDesk.Kiosk.Controllers;

namespace FrontDesk.Kiosk
{
    /// <summary>
    /// Interaction logic for FailedToStartWindow.xaml
    /// </summary>
    public partial class FailedToStartWindow : Window, IMainWindow
    {
        public bool IsSucceedFlow { get; } = false;

        public FailedToStartWindow()
        {
            InitializeComponent();

            grdOutOfServiceFullScreen.ErrorMessageText = Properties.Resources.KioskIsNotInstalledProperlyMessage;
            this.Loaded += new RoutedEventHandler(FailedToStartWindow_Loaded);
        }

        void FailedToStartWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SoundController.Instance.Initialize(meSound);

            // play error sound
            SoundController.Instance.PlayErrorSound();
            
            if (!Settings.AppSettings.ShowCursor)
            {
                //hide cursor
                this.Cursor = Cursors.None;
            }
        }

        public FailedToStartWindow(string errorMessage) : this()
        {
            grdOutOfServiceFullScreen.ErrorMessageText = errorMessage;

        }
    }
}
