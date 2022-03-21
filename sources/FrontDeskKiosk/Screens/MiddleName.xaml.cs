using FrontDesk.Kiosk.Controls.Keyboard;
using FrontDesk.Kiosk.Workflow;

using Ninject;

using System;
using System.Windows;
using System.Windows.Controls;

namespace FrontDesk.Kiosk.Screens
{
    public partial class MiddleName : UserControl, IVisualScreen, IStatefulVisualScreen
    {

        private readonly IKernel Container;

        #region Constructor

        public MiddleName()
        {
            InitializeComponent();

            Container = (IKernel)Bootstrap.Bootstrapper.Container;

            _screenHelper = new ScreenComponentHelper(this);
        }

        #endregion

        public IScreeningResultState ResultState { get; set; }

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
        }

        private void OnNext(object sender, RoutedEventArgs e)
        {
            string middleName = ucText.Text.Trim();
            Container.Get<ErrorNotificationController>().ClearErrors();

            NextScreenClicked?.Invoke(this, new NextScreenClickedEventArg(null, middleName));
        }

        #region IVisualScreen Members

        public event EventHandler<NextScreenClickedEventArg> NextScreenClicked;

        public Button GetDefaultButtonForTesting()
        {
            return btnNext;
        }

        public void Reset()
        {
            ucText.Clear();
            Hide();
        }

        public void Show(bool withAnimation)
        {
            btnBackspace.SetFontSize(25);
            
            //set Middle name from state when doing second round

            var state = this.ResultState;

            if (state.IsRepeatingPatientNameAfterValidationFailed)
            {
                ucText.Text = state.Result?.MiddleName;
            }
            

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
