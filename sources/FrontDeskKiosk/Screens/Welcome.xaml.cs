using System;
using System.Windows;
using System.Windows.Controls;

namespace FrontDesk.Kiosk.Screens
{
	/// <summary>
	/// Interaction logic for Welcome.xaml
	/// </summary>
	public partial class Welcome : UserControl, IVisualScreen
    {
        private ScreenComponentHelper _helper;

        public Welcome()
        {
            InitializeComponent();
            _helper = new ScreenComponentHelper(this);
        }

        #region IVisualScreen Members

        public event EventHandler<NextScreenClickedEventArg> NextScreenClicked;

        #endregion

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

       
       

        public void Show(bool withAnimation)
        {

            _helper.Show(withAnimation);
        }

        public void Hide()
        {

            _helper.Hide();
        }

    }
}
