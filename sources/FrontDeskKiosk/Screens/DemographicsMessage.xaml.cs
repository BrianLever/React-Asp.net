using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace FrontDesk.Kiosk.Screens
{
	/// <summary>
	/// Interaction logic for Welcome.xaml
	/// </summary>
	public partial class DemographicsMessage : UserControl, IVisualScreen
    {

        private ScreenComponentHelper _screenHelper;


        private void OnNext(object sender, RoutedEventArgs e)
        {

            NextScreenClicked?.Invoke(this, new NextScreenClickedEventArg(null, string.Empty));
        }

        #region ISupportInitialize Members

        

        #endregion
        public DemographicsMessage()
        {
            InitializeComponent();

            _screenHelper = new ScreenComponentHelper(this);

        }



        #region IVisualScreen Members

        public event EventHandler<NextScreenClickedEventArg> NextScreenClicked;

        public Button GetDefaultButtonForTesting()
        {
            return btnNext;
        }

        
        public void Show(bool withAnimation)
        {
            _screenHelper.Show(withAnimation);
        }

        public void Hide()
        {
            _screenHelper.Hide();
        }

        public void Reset()
        {
            
        }

        #endregion


    }
}
