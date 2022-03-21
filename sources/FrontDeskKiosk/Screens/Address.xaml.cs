using System;
using System.Windows;
using System.Windows.Controls;
using FrontDesk.Kiosk.Controls.Keyboard;
using FrontDesk.Kiosk.Controls.TextBoxExtensions;
using System.Windows.Media;

namespace FrontDesk.Kiosk.Screens
{
	public partial class Address : IVisualScreen
    {
        #region Constructor

        public Address()
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
            btnNext.IsEnabled = !ucText.IsEmpty();
        }
        private void OnNext(object sender, RoutedEventArgs e)
        {
            string address = ucText.Text.Trim();

            if (NextScreenClicked != null)
            {
                NextScreenClicked(this, new NextScreenClickedEventArg(null, address));
            }
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
