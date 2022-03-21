using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FrontDesk.Kiosk.Screens
{
	/// <summary>
	/// Interaction logic for Welcome.xaml
	/// </summary>
	public partial class Home : UserControl, IVisualScreen
    {
        private ScreenComponentHelper _helper;
        private Brush MainWindowBackgrounBackup;

        public Home()
        {
            InitializeComponent();

            _helper = new ScreenComponentHelper(this);

            MainWindowBackgrounBackup = App.Current.MainWindow.Background;


        }

       

        public event EventHandler<NextScreenClickedEventArg> NextScreenClicked;

       
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            OnNextScreenClicked(this, new NextScreenClickedEventArg(null, null));
        }

        protected virtual void OnNextScreenClicked(IVisualScreen screen, NextScreenClickedEventArg e)
        {
            if (NextScreenClicked != null)
            {
                NextScreenClicked(screen, e);
            }
        }


        #region IVisualScreen Members


        public Button GetDefaultButtonForTesting()
        {
            return btnNext;
        }

       


        public void Reset()
        {
            //do nothing
        }

        #endregion

        public void Show(bool withAnimation) {

            App.Current.MainWindow.Background = this.Background;
            _helper.Show(withAnimation);
        }

        public void Hide()
        {


            _helper.Hide();

            App.Current.MainWindow.Background = MainWindowBackgrounBackup;
        }
      
    }
}
