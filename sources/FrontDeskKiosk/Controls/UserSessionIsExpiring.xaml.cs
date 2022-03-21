using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FrontDesk.Kiosk.Controls
{
	/// <summary>
	/// Interaction logic for UserSessionIsExpiring.xaml
	/// </summary>
	public partial class UserSessionIsExpiring : UserControl, IScreenControl
    {
        public UserSessionIsExpiring()
        {
            InitializeComponent();
            this.PreviewMouseUp += new MouseButtonEventHandler(UserSessionIsExpiring_MouseUp);
        }

        void UserSessionIsExpiring_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
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
