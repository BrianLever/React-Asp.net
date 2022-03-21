using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FrontDesk.Kiosk.Controls.Keyboard;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using FrontDesk.Kiosk.Services;
using System.Diagnostics;

namespace FrontDesk.Kiosk.Screens
{
    public partial class State : UserControl, IVisualScreen
    {
        #region Constructor

        public State()
        {
            InitializeComponent();

            _screenHelper = new ScreenComponentHelper(this);
        }

        #endregion

        private ScreenComponentHelper _screenHelper;

        protected readonly PatientStateService _stateService = ScreenTypeAheadDataSources.Default.States;

        private bool IsAutoCompleteMode
        {
            get
            {
                return btnBackspace.IsAutoCompleteMode;
            }
            set
            {
                btnBackspace.IsAutoCompleteMode = value;
                ucKeyboard.IsAutoCompleteMode = value;
            }
        }


        private void btnBackspace_KeyPressing(object sender, KeyPressingEventArgs e)
        {
            string stateName = ucText.Text;

            FrontDesk.State state = _stateService.GetByName(stateName);
            List<FrontDesk.State> states = _stateService.GetByPartOfName(stateName);

            if (state != null || (IsPreviousStateSymbolSame(stateName.Length, states) && !IsAutoCompleteMode))
            {
                IsAutoCompleteMode = true;
                ucText.Remove();
                DoBackspaceAction();
            }
            else if (!IsAutoCompleteMode)
            {
                ucText.Remove();
            }
            else
            {
                ucText.Remove();

                stateName = ucText.Text;
                states = _stateService.GetByPartOfName(stateName);
                if (stateName == "" || (states != null && states.Count > 1) && !IsNextStateSymbolSame(stateName.Length, states))
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
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += (sendr, args) => {
                btnBackspace.Press(); timer.Stop();
                IsAutoCompleteMode = false;
            };
            timer.Interval = TimeSpan.FromSeconds(0.05);
            timer.Start();
        }

        private void btnBackspace_KeyPressed(object sender, EventArgs e)
        {
            string stateName = ucText.Text;

            List<FrontDesk.State> states = _stateService.GetByPartOfName(stateName);

            List<char> allowedChars = new List<char>();
            foreach (var state in states)
            {
                if (state.Name.Length > stateName.Length)
                {
                    allowedChars.Add(state.Name[stateName.Length]);
                }
            }
            ucKeyboard.SetEnabledState(allowedChars.Distinct().ToArray());

            Validate(stateName);
        }

        private void ucKeyboard_KeyPressing(object sender, KeyPressingEventArgs e)
        {
            ucText.Add(e.KeyCode);
        }

        private void ucKeyboard_KeyPressed(object sender, EventArgs e)
        {
            string stateName = ucText.Text;

            #region set keyboard enabled state

            FrontDesk.State.UseLocalDatabaseConnection = true;
            List<FrontDesk.State> states = _stateService.GetByPartOfName(stateName);

            List<char> allowedChars = new List<char>();
            foreach (var state in states)
            {
                if (state.Name.Length > stateName.Length)
                {
                    allowedChars.Add(state.Name[stateName.Length]);
                }
            }

            IsAutoCompleteMode = states.Count == 1 && states[0].Name.Length != stateName.Length;

            ucKeyboard.SetEnabledState(allowedChars.Distinct().ToArray());

            if (states.Count == 1 || IsNextStateSymbolSame(stateName.Length, states))
            {
                if (states[0].Name.Length != stateName.Length)
                {
                    var timer = new DispatcherTimer();
                    timer.Tick += (sendr, args) => { ucKeyboard.SimulateKeyPressing(states[0].Name[stateName.Length]); timer.Stop(); };
                    timer.Interval = TimeSpan.FromSeconds(0.05);
                    timer.Start();
                }
            }

            #endregion

            Validate(stateName);
        }

        private bool IsNextStateSymbolSame(int currentLength, List<FrontDesk.State> states)
        {
            char nextSymbol;
            if (states != null && states.Count > 0 && states[0].Name.Length > currentLength)
            {
                nextSymbol = states[0].Name[currentLength];
            }
            else
            {
                return false;
            }

            foreach (FrontDesk.State state in states)
            {
                if (state.Name.Length <= currentLength)
                {
                    return false;
                }
                else
                {
                    if (state.Name[currentLength] != nextSymbol)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        private bool IsPreviousStateSymbolSame(int currentLength, List<FrontDesk.State> states)
        {
            char previousSymbol;
            if (states != null && states.Count > 0 && currentLength != 0)
            {
                previousSymbol = states[0].Name[currentLength - 1];
            }
            else
            {
                return false;
            }

            foreach (FrontDesk.State state in states)
            {
                if (state.Name.Length <= currentLength)
                {
                    return false;
                }
                else
                {
                    if (state.Name[currentLength - 1] != previousSymbol)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void OnNext(object sender, RoutedEventArgs e)
        {
            FrontDesk.State state = null;

            string stateName = ucText.Text.Trim();

            state = _stateService.GetByName(stateName);

            if (state != null)
            {

                NextScreenClicked?.Invoke(this, new NextScreenClickedEventArg(null, state.StateCode));
            }
            else
            {
                Debugger.Launch();
            }
        }

        private void Validate(string stateName)
        {
            bool isValid = false;
            FrontDesk.State s = null;

            if (String.IsNullOrEmpty(stateName))
            {
                isValid = false;
            }
            else
            {
                s = _stateService.GetByName(stateName);
                if (s != null)
                {
                    isValid = true;
                }
                else
                {
                    isValid = false;
                }
            }

            btnNext.IsEnabled = isValid;
        }

        #region IVisualScreen Members

        public event EventHandler<NextScreenClickedEventArg> NextScreenClicked;

        public Button GetDefaultButtonForTesting()
        {
            return btnNext;
        }

        public void Reset()
        {
            IsAutoCompleteMode = false;
            btnNext.IsEnabled = false;
            ucText.Clear();
            Hide();
        }

        public void Show(bool withAnimation)
        {
            btnBackspace.SetFontSize(25);

            ucText.SetFocus();

            ReadOnlyCollection<FrontDesk.State> states = _stateService.GetAll();
            List<char> allowedChars = new List<char>();
            foreach (var state in states)
            {
                allowedChars.Add(state.Name[0]);
            }

            ucKeyboard.SetEnabledState(allowedChars.Distinct().ToArray());

            _screenHelper.Show(withAnimation);
        }

        public void Hide()
        {
            _screenHelper.Hide();
        }

        #endregion
    }
}
