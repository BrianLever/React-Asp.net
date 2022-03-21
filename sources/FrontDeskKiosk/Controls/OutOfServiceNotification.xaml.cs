using System.Windows;
using System.Windows.Controls;

namespace FrontDesk.Kiosk.Controls
{
	/// <summary>
	/// Interaction logic for OutOfServiceNotification.xaml
	/// </summary>
	public partial class OutOfServiceNotification : UserControl, IScreenControl
    {
        public OutOfServiceNotification()
        {
            InitializeComponent();
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


      
        #endregion
    }
}
