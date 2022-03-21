using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

using FrontDesk.Kiosk.Controls.Keyboard;

namespace FrontDesk.Kiosk.Screens.Behaviors
{
    public class TextInputControlBehavior : ITextInputControlBehavior
    {
        protected ScreenComponentHelper _screenHelper;

        private ITextInputWithAutosuggestControl _control;


        public TextInputControlBehavior(ITextInputWithAutosuggestControl control)
        {
            _control = control;
            _screenHelper = new ScreenComponentHelper(control);
        }


        public void Init()
        {
            _control.BackspaceButton.KeyPressing += btnBackspace_KeyPressing;
            _control.BackspaceButton.KeyPressed += btnBackspace_KeyPressed;

            _control.KeyboardControl.KeyPressing += ucKeyboard_KeyPressing;
            _control.KeyboardControl.KeyPressed += ucKeyboard_KeyPressed;

            _control.NextButton.Click += OnNext;
        }

        public void btnBackspace_KeyPressing(object sender, KeyPressingEventArgs e)
        {
            _control.TextInputCtrl.Remove();
        }


        private void btnBackspace_KeyPressed(object sender, EventArgs e)
        {
            string inputValue = _control.TextInputCtrl.Text;

            Validate(inputValue);
        }

        private void ucKeyboard_KeyPressing(object sender, KeyPressingEventArgs e)
        {
            _control.TextInputCtrl.Add(e.KeyCode);
        }

        private void ucKeyboard_KeyPressed(object sender, EventArgs e)
        {
            string inputValue = _control.TextInputCtrl.Text;


            Validate(inputValue);
        }


        private void Validate(string inputValue)
        {
            bool isValid = true;

            _control.NextButton.IsEnabled = isValid;
        }

        private void OnNext(object sender, RoutedEventArgs e)
        {
            string inputValue = _control.TextInputCtrl.Text.Trim();

            inputValue = _control.TypeAheadService.GetByName(inputValue);

            _control.TriggerNextScreenEvent(inputValue);
        }



        public void Hide()
        {
            _screenHelper.Hide();
        }

        public void Reset()
        {
            _control.NextButton.IsEnabled = true;
            _control.TextInputCtrl.Clear();

            Hide();
        }

        public void Show(bool withAnimation)
        {
            _control.BackspaceButton.SetFontSize(25);

            _control.TextInputCtrl.SetFocus();

            _screenHelper.Show(withAnimation);
        }
    }
}
