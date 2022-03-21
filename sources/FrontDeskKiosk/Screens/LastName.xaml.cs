using System;
using System.Windows;
using System.Windows.Controls;
using FrontDesk.Kiosk.Controls.Keyboard;
using FrontDesk.Kiosk.Controls.TextBoxExtensions;
using FrontDesk.Kiosk.Screens.Behaviors;
using FrontDesk.Kiosk.Workflow;

namespace FrontDesk.Kiosk.Screens
{
    /// <summary>
    /// Last name screeen
    /// </summary>
    public partial class LastName : UserControl, IPatientNameScreen
    {
        #region Constructor

        public LastName()
        {
            InitializeComponent();

            _screenHelper = new ScreenComponentHelper(this);

            _patientNameScreenBehavior = new PatientNameScreenBehavior(
                this, 
                (r) => r.LastName
                );

        }

        #endregion

        private ScreenComponentHelper _screenHelper;

        private PatientNameScreenBehavior _patientNameScreenBehavior;

        #region IPatientIdentityScreen Members

        public KeyboardControl KeyboardControl { get { return ucKeyboard; } }
        public TerminalTextBox TextInputCtrl { get { return ucText; } }
        public IScreeningResultState ResultState { get; set; }
        public TextBlock FieldLabel => lblText;
        public TextBlock InvalidEhrFieldLabel => lblInvalidEhrText;

        #endregion

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
            string lastName = ucText.Text.Trim();
            btnNext.IsEnabled = !String.IsNullOrEmpty(lastName);
        }

        private void OnNext(object sender, RoutedEventArgs e)
        {
            string lastName = ucText.Text.Trim();

            NextScreenClicked?.Invoke(this, new NextScreenClickedEventArg(null, lastName));

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

            _patientNameScreenBehavior.Init();

            _screenHelper.Show(withAnimation);
        }

        public void Hide()
        {
            _screenHelper.Hide();
        }

        #endregion

    }
}
