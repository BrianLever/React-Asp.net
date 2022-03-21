using System.Windows;
using System.Windows.Controls;

namespace FrontDesk.Kiosk.Controls
{
	/// <summary>
	/// Interaction logic for OutOfServiceFullScreen.xaml
	/// </summary>
	public partial class OutOfServiceFullScreen : UserControl, IScreenControl
    {
       
        public OutOfServiceFullScreen()
        {
            InitializeComponent();

            ErrorMessageText = Properties.Resources.KioskIsOutOfServiceMessage;
        }

        #region IScreenControl Members

        public void Show()
        {
            this.Visibility = Visibility.Visible;
        }

        public void Hide()
        {
            this.Visibility = Visibility.Collapsed;
        }

        public string ErrorMessageText
        {
            get { return txtMessage.Text; }
            set { txtMessage.Text = value; }
        }
       

        #endregion
    }
}
