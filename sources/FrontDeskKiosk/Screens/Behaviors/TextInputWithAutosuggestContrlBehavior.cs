using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

using FrontDesk.Kiosk.Controls.Keyboard;

namespace FrontDesk.Kiosk.Screens.Behaviors
{
    public class TextInputWithAutosuggestContrlBehavior: ITextInputControlBehavior
    {
        protected ScreenComponentHelper _screenHelper;

        private ITextInputWithAutosuggestControl _control;


        public TextInputWithAutosuggestContrlBehavior(ITextInputWithAutosuggestControl control)
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


        protected bool IsAutoCompleteMode
        {
            get
            {
                return _control.BackspaceButton.IsAutoCompleteMode;
            }
            set
            {
                _control.BackspaceButton.IsAutoCompleteMode = value;
                _control.KeyboardControl.IsAutoCompleteMode = value;
            }
        }


        public void btnBackspace_KeyPressing(object sender, KeyPressingEventArgs e)
        {
            string inputValue = _control.TextInputCtrl.Text;

            string selectedValue = _control.TypeAheadService.GetByName(inputValue);
            List<string> filteredValues = _control.TypeAheadService.GetByPartOfName(inputValue);

            if (selectedValue != null || (IsPreviousStateSymbolSame(inputValue.Length, filteredValues)
                && !IsAutoCompleteMode))
            {
                IsAutoCompleteMode = true;
                _control.TextInputCtrl.Remove();
                DoBackspaceAction();
            }
            else if (!IsAutoCompleteMode)
            {
                _control.TextInputCtrl.Remove();
            }
            else
            {
                _control.TextInputCtrl.Remove();

                inputValue = _control.TextInputCtrl.Text;
                filteredValues = _control.TypeAheadService.GetByPartOfName(inputValue);
                if (inputValue == "" || (filteredValues != null && filteredValues.Count > 1) && !IsNextSymbolSame(inputValue.Length, filteredValues))
                {
                    IsAutoCompleteMode = false;
                }
                else
                {
                    DoBackspaceAction();
                }
            }
        }


        private void DoBackspaceAction()
        {
            var timer = new DispatcherTimer();
            timer.Tick += (sendr, args) =>
            {
                _control.BackspaceButton.Press();
                timer.Stop();
                IsAutoCompleteMode = false;
            };
            timer.Interval = TimeSpan.FromSeconds(0.05);
            timer.Start();
        }

        private void btnBackspace_KeyPressed(object sender, EventArgs e)
        {
            string inputValue = _control.TextInputCtrl.Text;

            List<string> filteredValues =  _control.TypeAheadService.GetByPartOfName(inputValue);

            List<char> allowedChars = new List<char>();
            foreach (var val in filteredValues)
            {
                if (val.Length > inputValue.Length)
                {
                    allowedChars.Add(val[inputValue.Length]);
                }
            }
            _control.KeyboardControl.SetEnabledState(allowedChars.Distinct().ToArray());

            Validate(inputValue);
        }

        private void ucKeyboard_KeyPressing(object sender, KeyPressingEventArgs e)
        {
            _control.TextInputCtrl.Add(e.KeyCode);
        }

        private void ucKeyboard_KeyPressed(object sender, EventArgs e)
        {
            string inputValue = _control.TextInputCtrl.Text;

            #region set keyboard enabled state

            List<string> filteredValues = _control.TypeAheadService.GetByPartOfName(inputValue);

            List<char> allowedChars = new List<char>();
            foreach (var val in filteredValues)
            {
                if (val.Length > inputValue.Length)
                {
                    allowedChars.Add(val[inputValue.Length]);
                }
            }

            IsAutoCompleteMode = filteredValues.Count == 1 && filteredValues[0].Length != inputValue.Length;

            _control.KeyboardControl.SetEnabledState(allowedChars.Distinct().ToArray());

            if (filteredValues.Count == 1 || IsNextSymbolSame(inputValue.Length, filteredValues))
            {
                if (filteredValues[0].Length != inputValue.Length)
                {
                    var timer = new DispatcherTimer();
                    timer.Tick += (sendr, args) => {
                        _control.KeyboardControl.SimulateKeyPressing(filteredValues[0][inputValue.Length]); timer.Stop(); };
                    timer.Interval = TimeSpan.FromSeconds(0.05);
                    timer.Start();
                }
            }

            #endregion

            Validate(inputValue);
        }

        private bool IsNextSymbolSame(int currentLength, List<string> values)
        {
            char nextSymbol;
            if (values != null && values.Count > 0 && values[0].Length > currentLength)
            {
                nextSymbol = values[0][currentLength];
            }
            else
            {
                return false;
            }

            foreach (string val in values)
            {
                if (val.Length <= currentLength)
                {
                    return false;
                }
                else
                {
                    if (val[currentLength] != nextSymbol)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        private bool IsPreviousStateSymbolSame(int currentLength, List<string> values)
        {
            char previousSymbol;
            if (values != null && values.Count > 0 && currentLength != 0)
            {
                previousSymbol = values[0][currentLength - 1];
            }
            else
            {
                return false;
            }

            foreach (string val in values)
            {
                if (val.Length <= currentLength)
                {
                    return false;
                }
                else
                {
                    if (val[currentLength - 1] != previousSymbol)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void Validate(string inputValue)
        {
            bool isValid = false;
            string value = null;

            if (String.IsNullOrEmpty(inputValue))
            {
                isValid = false;
            }
            else
            {
                value = _control.TypeAheadService.GetByName(inputValue);
                if (!string.IsNullOrEmpty(value))
                {
                    isValid = true;
                }
                else
                {
                    isValid = false;
                }
            }

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
            IsAutoCompleteMode = false;
            _control.NextButton.IsEnabled = false;
            _control.TextInputCtrl.Clear();

            Hide();
        }

        public void Show(bool withAnimation)
        {
            _control.BackspaceButton.SetFontSize(25);

            _control.TextInputCtrl.SetFocus();

            ReadOnlyCollection<string> values = _control.TypeAheadService.GetAll();
            List<char> allowedChars = new List<char>();
            foreach (var val in values)
            {
                allowedChars.Add(val[0]);
            }

            _control.KeyboardControl.SetEnabledState(allowedChars.Distinct().ToArray());

            _screenHelper.Show(withAnimation);
        }
    }
}
