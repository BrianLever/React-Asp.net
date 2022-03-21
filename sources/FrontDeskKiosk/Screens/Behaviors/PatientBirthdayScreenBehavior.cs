using System;
using System.Linq;
using System.Windows;

namespace FrontDesk.Kiosk.Screens.Behaviors
{
    public class PatientBirthdayScreenBehavior
    {
        protected ScreenComponentHelper _screenHelper;

        private IPatientBirthdayScreen _screen;

        private Func<ScreeningResult, DateTime> _propertyGetter;

        public PatientBirthdayScreenBehavior(
            IPatientBirthdayScreen screen,
            Func<ScreeningResult, DateTime> propertyGetter
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
                var dateOfBirth = _propertyGetter(state.Result).ToString("MMddyyyy").ToCharArray();

                foreach (var c in dateOfBirth)
                {
                    _screen.KeyboardControl.SimulateKeyPressing(c);
                }

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
