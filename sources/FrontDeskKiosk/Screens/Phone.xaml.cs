using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FrontDesk.Kiosk.Controls.Keyboard;

namespace FrontDesk.Kiosk.Screens
{
	public partial class Phone : UserControl, IVisualScreen
    {
        #region Constructor

        public Phone()
        {
            InitializeComponent();

            _screenHelper = new ScreenComponentHelper(this);
        }

        #endregion

        private ScreenComponentHelper _screenHelper;


        private void btnBackspace_KeyPressing(object sender, KeyPressingEventArgs e)
        {
            ucPhone.Remove();
        }

        private void ucKeyboard_KeyPressing(object sender, KeyPressingEventArgs e)
        {
            ucPhone.Add(e.KeyCode);
        }

        private void ucKeyboard_KeyPressed(object sender, EventArgs e)
        {
            char[] allowedChars = ucPhone.CurrentControl.GetAllowedValuesHandler();
            ucKeyboard.SetEnabledState(allowedChars);
            SetBackspaseEnbledState(allowedChars);

            string phone = ucPhone.Value;
            btnNext.IsEnabled = !String.IsNullOrEmpty(ucPhone.Value);

        }

        private void OnNext(object sender, RoutedEventArgs e)
        {
            string phone = ucPhone.Value;
            //if (String.IsNullOrEmpty(ucPhone.Value))
            //{
            //    ErrorNotificationController.Instance.ShowError(Properties.Resources.Validation_EmptyPhoneMessage);
            //}
            //else
            //{
            //    ErrorNotificationController.Instance.ClearErrors();
                if (NextScreenClicked != null)
                {
                    NextScreenClicked(this, new NextScreenClickedEventArg(null, phone));
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
            ucPhone.Clear();
            Hide();
        }

        public void Show(bool withAnimation)
        {
            //btnNext.IsEnabled = false;
            ucPhone.SetFocus();
            btnBackspace.SetFontSize(25);
            char[] allowedChars = ucPhone.CurrentControl.GetAllowedValuesHandler();
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
