using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FrontDesk.Kiosk.Controls.Keyboard;
using FrontDesk.Kiosk.Controls.TextBoxExtensions;
using FrontDesk.Kiosk.Screens.Behaviors;
using FrontDesk.Kiosk.Workflow;

namespace FrontDesk.Kiosk.Screens
{
    public partial class Birthday : UserControl, IPatientBirthdayScreen
    {
        #region Constructor

        public Birthday()
        {
            InitializeComponent();
            _screenHelper = new ScreenComponentHelper(this);

            _patientBirthdayScreenBehavior = new PatientBirthdayScreenBehavior(
               this,
               (r) => r.Birthday
               );
        }

        #endregion

        private ScreenComponentHelper _screenHelper;

        private PatientBirthdayScreenBehavior _patientBirthdayScreenBehavior;

        #region IPatientBirthdayScreen Members

        public KeyboardControl KeyboardControl { get { return ucKeyboard; } }
        public MaskedDateTextBox InputCtrl { get { return ucDate; } }
        public IScreeningResultState ResultState { get; set; }
        public TextBlock FieldLabel => lblText;
        public TextBlock InvalidEhrFieldLabel => lblInvalidEhrText;
        #endregion

        private void btnBackspace_KeyPressing(object sender, KeyPressingEventArgs e)
        {
            ucDate.Remove();
        }

        private void ucKeyboard_KeyPressing(object sender, KeyPressingEventArgs e)
        {
            ucDate.Add(e.KeyCode);
        }

        private void ucKeyboard_KeyPressed(object sender, EventArgs e)
        {
            char[] allowedChars = ucDate.CurrentControl.GetAllowedValuesHandler();
            ucKeyboard.SetEnabledState(allowedChars);
            SetBackspaseEnbledState(allowedChars);

            bool isValid = true;
            DateTime? birthday = null;

            try
            {
                birthday = ucDate.Value;
            }
            catch
            {
                isValid = false;
            }

            if (isValid)
            {
                if (!birthday.HasValue)
                {
                    isValid = false;
                }
                else if (birthday.Value.Date > DateTime.Now.Date)
                {
                    isValid = false;
                }
            }

            btnNext.IsEnabled = isValid;


        }

        private void OnNext(object sender, RoutedEventArgs e)
        {
            //bool isValid = true;
            DateTime? birthday = ucDate.Value;

            // show loading indicator since there is a business rules that can take some time.
            pnlLoading.Visibility = Visibility.Visible;
            ucSpinner.Show();

            NextScreenClicked?.Invoke(this, new NextScreenClickedEventArg(null, birthday.Value));
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
            ucDate.Clear();
            Hide();
        }

        public void Show(bool withAnimation)
        {
            //btnNext.IsEnabled = false;
            ucDate.SetFocus();
            btnBackspace.SetFontSize(25);
            char[] allowedChars = ucDate.CurrentControl.GetAllowedValuesHandler();
            ucKeyboard.SetEnabledState(allowedChars);
            SetBackspaseEnbledState(allowedChars);

            _patientBirthdayScreenBehavior.Init();

            _screenHelper.Show(withAnimation);
        }

        public void Hide()
        {
            _screenHelper.Hide();
            pnlLoading.Visibility = Visibility.Collapsed;
            ucSpinner.Hide();
        }


        #endregion

    }
}
