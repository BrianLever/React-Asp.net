using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Common.Logging;

namespace FrontDesk.Kiosk.Controls.Keyboard
{
    public partial class KeyboardControl : UserControl
    {
        private KeyboardController _controller;
        private ILog _logger = LogManager.GetLogger<KeyboardControl>();

        /// <summary>
        /// The type of keyboard
        /// </summary>
        public KeyboardType KbType { get; set; }

        public bool ShowAdditionalSymbols { get; set; }

        private bool isUpperCaseMode;
        public bool IsUpperCaseMode
        {
            get
            {
                return isUpperCaseMode;
            }
            set
            {
                isUpperCaseMode = value;
                if (this.shift != null)
                {
                    this.shift.Highlighted = isUpperCaseMode;
                }
            }
        }

        private KeyButton shift = null;

        private List<KeyButton> _buttons = new List<KeyButton>();


        public bool AlwaysUpperCase { get; set; }

        private bool _isAutoCompleteMode = false;
        public bool IsAutoCompleteMode
        {
            get
            {
                return _isAutoCompleteMode;
            }
            set
            {
                _isAutoCompleteMode = value;
                _buttons.ForEach((Action<KeyButton>)((KeyButton key) => { key.IsAutoCompleteMode = _isAutoCompleteMode; }));
            }

        }

        public KeyboardControl()
        {
            InitializeComponent();

            //Defaults
            KbType = KeyboardType.Full; // full keyboard
            AlwaysUpperCase = true;
        }

        public event EventHandler<KeyPressingEventArgs> KeyPressing;
        public event EventHandler KeyPressed;
        public event EventHandler BackspacePressed;
        public event EventHandler ShiftPressed;

        private bool _loaded = false;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_loaded)
            {
                _controller = new KeyboardController(spKeys, KbType, ShowAdditionalSymbols);

                spKeys.BeginInit();

                ApplyTypeSpecificStyles();

                for (int y = 0; y < _controller.RowsCount; y++)
                {
                    StackPanel rowContiner = _controller.CreateRowControl();

                    int rowLength = _controller.GetRowLength(y);
                    for (int x = 0; x < rowLength; x++)
                    {
                        KeyButton key = new KeyButton();
                        key.IsAutoCompleteMode = false;
                        key.Name = String.Format("key_{0:D2}_{1:D2}", x, y);
   
                        key.KeyPressing += new EventHandler<KeyPressingEventArgs>(OnKeyPressing);
                        key.KeyPressed += new EventHandler(OnKeyPressed);
                        _controller.Initialize(key);

                        if(key.HorizontalAlignment != HorizontalAlignment.Center)
                        {
                            rowContiner.HorizontalAlignment = key.HorizontalAlignment;
                        }

                        if (key.Symbol == '\u8679')
                        {
                            shift = key;
                        }

                        rowContiner.Children.Add(key);
                        _buttons.Add(key);
                    }
                }

                IsUpperCaseMode = true;

                spKeys.EndInit();
                spKeys.UpdateLayout();

                _loaded = true;
            }
        }


        private void ApplyTypeSpecificStyles()
        {
           
        }

        private void OnKeyPressing(object sender, KeyPressingEventArgs e)
        {
            _logger.DebugFormat("[Keyboard] Key pressed: {0}", e.KeyCode);

            if (e.KeyCode == '\u8679') //shift
            {
                IsUpperCaseMode = !IsUpperCaseMode;

                if (ShiftPressed != null)
                {
                    ShiftPressed(this, new EventArgs());
                }
            }
            else if (e.KeyCode == '\u2408') //backspace
            {
                if (BackspacePressed != null)
                {
                    BackspacePressed(this, new EventArgs());
                }
            }
            else if (KeyPressing != null)
            {
                if (KeyPressing != null)
                {
                    e.KeyCode = IsUpperCaseMode ?
                        Char.ToUpper(e.KeyCode) : Char.ToLower(e.KeyCode);

                    KeyPressing(this, e);
                }

                if (KeyboardController.PreUpperCaseChars.Contains(e.KeyCode) || AlwaysUpperCase)
                {
                    IsUpperCaseMode = true;
                }
                else if (IsUpperCaseMode)
                {
                    IsUpperCaseMode = false;
                }
            }


        }

        private void OnKeyPressed(object sender, EventArgs e)
        {
            if (KeyPressed != null)
            {
                KeyPressed(this, e);
            }
        }

        public void SimulateKeyPressing(char key)
        {
            KeyButton button = _buttons.FirstOrDefault(b => Char.ToLower(b.Symbol) == Char.ToLower(key));
            button?.Press();
        }


        /// <summary>
        /// Set kb keys enebled state 
        /// </summary>
        public void SetEnabledState(char[] enabledSymbols)
        {
            spKeys.BeginInit();

            foreach (StackPanel rowContainer in spKeys.Children)
            {
                foreach (KeyButton key in rowContainer.Children)
                {

                    key.Enabled =
                        enabledSymbols.Contains(Char.ToLower(key.Symbol)) ||
                        enabledSymbols.Contains(Char.ToUpper(key.Symbol));
                }
            }

            spKeys.EndInit();
            spKeys.UpdateLayout();
        }

        /// <summary>
        /// Clear applied enabled state of kb keys
        /// </summary>
        public void EnableAll()
        {
            spKeys.BeginInit();

            foreach (StackPanel rowContainer in spKeys.Children)
            {
                foreach (KeyButton key in rowContainer.Children)
                {
                    key.Enabled = true;
                }
            }

            spKeys.EndInit();
            spKeys.UpdateLayout();

        }

        public void SetDisabledState(char[] disabledSymbols)
        {
            spKeys.BeginInit();

            foreach (StackPanel rowContainer in spKeys.Children)
            {
                foreach (KeyButton key in rowContainer.Children)
                {

                    key.Enabled = !(
                        disabledSymbols.Contains(Char.ToLower(key.Symbol)) ||
                        disabledSymbols.Contains(Char.ToUpper(key.Symbol))
                        );
                }
            }

            spKeys.EndInit();
            spKeys.UpdateLayout();
        }
    }
}
