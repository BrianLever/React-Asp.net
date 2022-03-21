using System;
using System.Windows;

namespace FrontDesk.Kiosk.Screens.Behaviors
{
    public class PatientNameScreenBehavior
    {
        protected ScreenComponentHelper _screenHelper;

        private IPatientNameScreen _screen;

        private Func<ScreeningResult, string> _propertyGetter;

        public PatientNameScreenBehavior(
            IPatientNameScreen screen,
            Func<ScreeningResult, string> propertyGetter
            )
        {
            _screen = screen;
            _propertyGetter = propertyGetter;

            _screenHelper = new ScreenComponentHelper(screen);
        }

        public void Init()
        {

            var state = _screen.ResultState;

            if (state.IsRepeatingPatientNameAfterValidationFailed)
            {
                _screen.TextInputCtrl.Text = _propertyGetter(state.Result);

                // display alternative label
                _screen.FieldLabel.Visibility = Visibility.Collapsed;
                _screen.InvalidEhrFieldLabel.Visibility = Visibility.Visible;

            }
            else
            {
                // display default label
                _screen.FieldLabel.Visibility = Visibility.Visible;
                _screen.InvalidEhrFieldLabel.Visibility = Visibility.Collapsed;
            }
        }
    }
}
