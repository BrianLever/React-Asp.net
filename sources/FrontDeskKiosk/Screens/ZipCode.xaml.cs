using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FrontDesk.Kiosk.Controls.Keyboard;

namespace FrontDesk.Kiosk.Screens
{
	public partial class ZipCode : UserControl, IVisualScreen
    {
        #region Constructor

        public ZipCode()
        {
            InitializeComponent();

            _screenHelper = new ScreenComponentHelper(this);
        }

        #endregion

        private ScreenComponentHelper _screenHelper;

        private void btnBackspace_KeyPressing(object sender, KeyPressingEventArgs e)
        {
            ucZipCode.Remove();
        }

        private void ucKeyboard_KeyPressing(object sender, KeyPressingEventArgs e)
        {
            ucZipCode.Add(e.KeyCode);
        }

        private void ucKeyboard_KeyPressed(object sender, EventArgs e)
        {
            char[] allowedChars = ucZipCode.CurrentControl.GetAllowedValuesHandler();
            ucKeyboard.SetEnabledState(allowedChars);
            SetBackspaseEnbledState(allowedChars);

            string zip = ucZipCode.Value;
            btnNext.IsEnabled = !String.IsNullOrEmpty(zip);

        }

        private void OnNext(object sender, RoutedEventArgs e)
        {
            string zip = ucZipCode.Value;
            //if (String.IsNullOrEmpty(zip))
            //{
            //    //invalid zip code, show error message
            //    ucZipCode.SetFocus();
            //    ErrorNotificationController.Instance.ShowError(Properties.Resources.Validation_EmptyZipCodeMessage);
            //}
            //else
            //{
            //    ErrorNotificationController.Instance.ClearErrors();
                if (NextScreenClicked != null)
                {
                    NextScreenClicked(this, new NextScreenClickedEventArg(null, zip));
                }
            //}
        }

        private void SetBackspaseEnbledState(char[] allowedChars)
        {
            btnBackspace.Enabled = allowedChars.Contains('\u2408');
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
            ucZipCode.Clear();
            Hide();
        }

        public void Show(bool withAnimation)
        {
            //btnNext.IsEnabled = false;
            ucZipCode.SetFocus();
            btnBackspace.SetFontSize(25);
            char[] allowedChars = ucZipCode.CurrentControl.GetAllowedValuesHandler();
            ucKeyboard.SetEnabledState(allowedChars);
            SetBackspaseEnbledState(allowedChars);
            _screenHelper.Show(withAnimation);
        }

        public void Hide()
        {
            _screenHelper.Hide();
        }

        #endregion
    }
}
