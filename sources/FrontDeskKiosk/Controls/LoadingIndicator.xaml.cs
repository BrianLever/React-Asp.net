using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace FrontDesk.Kiosk.Controls
{
	/// <summary>
	/// Interaction logic for LoadingIndicator.xaml
	/// </summary>
	public partial class LoadingIndicator : UserControl
    {
        private Storyboard storyboard;

        public LoadingIndicator()
        {
            InitializeComponent();

            this.Visibility = Visibility.Collapsed;
            storyboard = LayoutRoot.Resources["LoadingStoryboard"] as Storyboard;
        }

        public void Show()
        {
            this.Visibility = Visibility.Visible;
            storyboard.Begin();
        }

        public void Hide()
        {
            this.Visibility = Visibility.Collapsed;
            storyboard.Stop();
        }
    }
}
