using System;
using System.Windows;
using System.Windows.Controls;
using FrontDesk.Kiosk.Controls.Keyboard;

namespace FrontDesk.Kiosk.Screens
{
	/// <summary>
	/// City screen
	/// </summary>
	public partial class City : UserControl, IVisualScreen
    {
        #region Constructor

        public City()
        {
            InitializeComponent();

            _screenHelper = new ScreenComponentHelper(this);
        }

        #endregion

        private ScreenComponentHelper _screenHelper;

        private void btnBackspace_KeyPressing(object sender, KeyPressingEventArgs e)
        {
            ucText.Remove();
        }

        private void ucKeyboard_KeyPressing(object sender, KeyPressingEventArgs e)
        {
            ucText.Add(e.KeyCode);
        }

        private void ucText_TextChanged(object sender, TextChangedEventArgs e)
        {
            string city = ucText.Text.Trim();
            btnNext.IsEnabled = !String.IsNullOrEmpty(city);
        }

        private void OnNext(object sender, RoutedEventArgs e)
        {
            string city = ucText.Text.Trim();
            //if (String.IsNullOrEmpty(city))
            //{
            //    //invalid city, show error message
            //    ucText.SetFocus();
            //    ErrorNotificationController.Instance.ShowError(Properties.Resources.Validation_EmptyCityMessage);
            //}
            //else
            //{
            //    ErrorNotificationController.Instance.ClearErrors();
                if (NextScreenClicked != null)
                {
                    NextScreenClicked(this, new NextScreenClickedEventArg(null, city));
                }
            //}
        }


        #region IVisualScreen Members

        public event EventHandler<NextScreenClickedEventArg> NextScreenClicked;

        public Button GetDefaultButtonForTesting()
        {
            return btnNext;
        }

        public void Reset()
        {
            btnNext.IsEnabled = false;
            ucText.Clear();
            Hide();
        }

        public void Show(bool withAnimation)
        {
            //btnNext.IsEnabled = false;
            btnBackspace.SetFontSize(25);
            ucText.SetFocus();
            _screenHelper.Show(withAnimation);
        }

        public void Hide()
        {
            _screenHelper.Hide();
        }

        #endregion
    }
}
