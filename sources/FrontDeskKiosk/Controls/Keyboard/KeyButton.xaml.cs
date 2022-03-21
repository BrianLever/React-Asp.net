using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Threading;

using Common.Logging;

using FrontDesk.Kiosk.Controllers;

namespace FrontDesk.Kiosk.Controls.Keyboard
{
    public partial class KeyButton : UserControl
    {
        private ILog _logger = LogManager.GetLogger<KeyButton>();
        public char Symbol { get; set; }

        protected ButtonState State { get; set; } = ButtonState.Default;

        public string Text
        {
            get { return txt.Text; }
            set
            {
                txt.Text = value?.ToUpperInvariant();

                UpdateButtonStyle();
            }
        }

        public object _locker = new object();

        public ImageSource ImageSource
        {
            get { return img.Source; }
            set { img.Source = value; }
        }

        public Image ImageObject
        {
            get { return img; }
        }

        public bool IsAutoCompleteMode { get; set; }

        private bool _enabled;
        /// <summary>
        /// Key Enabled state
        /// </summary>
        public bool Enabled
        {
            get
            {
                return _enabled && CanBePressed;
            }
            set
            {
                _enabled = CanBePressed && value;
                this.Opacity = _enabled ? 1 : 0.4;
                //ApplyTemplate(_enabled ? DepressedTemplateKey : DisabledTemplateKey);
            }
        }

        private bool _highlighted;
        public bool Highlighted
        {
            get
            {
                return _highlighted;
            }
            set
            {
                _highlighted = value;
                if (_highlighted)
                {
                    if (txt.Effect == null)
                    {
                        txt.Effect = new DropShadowEffect()
                        {
                            Color = Colors.White,
                            Opacity = 0.5,
                            BlurRadius = 12,
                            Direction = 0,
                            ShadowDepth = 0
                        };
                    }
                }
                else
                {
                    txt.Effect = null;
                }
            }
        }

        public bool CanBePressed { get; set; }


        public event EventHandler<KeyPressingEventArgs> KeyPressing;
        public event EventHandler KeyPressed;


        public string DepressedTemplateKey { get; set; }
        public string PressedTemplateKey { get; set; }
        public string DisabledTemplateKey { get; set; }

        // Styles when button contains long text that need to be rendered in several lines
        public string DepressedTemplateKeyLongText { get; set; }
        public string PressedTemplateKeyLongText { get; set; }
        public string DisabledTemplateKeyLongText { get; set; }


        public int SingleLineTextMaxCharacters { get; set; } = 65;

        private bool IsSingleLineText()
        {
            var text = this.Text;
            return string.IsNullOrWhiteSpace(text) || text.Length <= SingleLineTextMaxCharacters;
        }

        public KeyButton()
        {
            _enabled = true;
            _highlighted = false;
            InitializeComponent();

            CanBePressed = true;

            _simulateMouseUpEventTimer.Elapsed += _simulateMouseUpEventTimer_Elapsed;
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _logger.DebugFormat("[Key] MouseDown fired. Text:{0}; IsAutoCompleteMode: {1}", this.Text, IsAutoCompleteMode);
            if (!IsAutoCompleteMode)
            {
                PerformMouseDownAction(true);

            }
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _logger.DebugFormat("[Key] MouseUp fired. Text:{0}; IsAutoCompleteMode: {1}", this.Text, IsAutoCompleteMode);
            if (!IsAutoCompleteMode)
            {
                PerformMouseUpAction();

            }
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            _logger.DebugFormat("[Key] MouseLeave fired. Text: {0}; IsAutoCompleteMode: {1}", txt.Text, IsAutoCompleteMode);

            if (!IsAutoCompleteMode)
            {
                PerformMouseUpAction();
            }
        }


        private void UpdateButtonStyle()
        {
            if (State == ButtonState.Default)
            {
                SetButtonDefaultStateStyle();
            }
            else
            {
                SetButtonPressedStateStyle();
            }
        }


        private void SetButtonDefaultStateStyle()
        {
            var template = IsSingleLineText() ? DepressedTemplateKey : DepressedTemplateKeyLongText;

            if (!string.IsNullOrEmpty(template))
            {
                ApplyTemplate(template);
            }
        }

        private void SetButtonPressedStateStyle()
        {
            var template = IsSingleLineText() ? PressedTemplateKey : PressedTemplateKeyLongText;
            if (!string.IsNullOrEmpty(template))
            {
                ApplyTemplate(template);
            }
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //if long text not set, make it the same as single line
            DepressedTemplateKeyLongText = DepressedTemplateKeyLongText ?? DepressedTemplateKey;
            PressedTemplateKeyLongText = PressedTemplateKeyLongText ?? PressedTemplateKey;
            DisabledTemplateKeyLongText = DisabledTemplateKeyLongText ?? DisabledTemplateKey;

            SetButtonDefaultStateStyle();
            State = ButtonState.Default;
        }

        public void SetFontSize(double size)
        {

        }

        public void ApplyTemplate(string templateKey)
        {
            if (!String.IsNullOrEmpty(templateKey))
            {
                _logger.TraceFormat("[Key] ApplyTemplate was called. Text: {0}; templateKey: {1}", txt.Text, templateKey);
                ucLayout.Template = (ControlTemplate)this.FindResource(templateKey);

                ApplyKeyStyle(templateKey + "_Text");
            }


        }

        public void ApplyKeyStyle(string templateKey)
        {
            if (!String.IsNullOrEmpty(templateKey))
            {
                _logger.TraceFormat("[Key] ApplyKeyStyle was called. Text: {0}; templateKey: {1}", txt.Text, templateKey);
                txt.Style = (Style)this.FindResource(templateKey);
            }
        }


        #region Simulate Button Up event

        private readonly System.Timers.Timer _simulateMouseUpEventTimer = new System.Timers.Timer()
        {
            Interval = 1000,
            AutoReset = false,
            Enabled = false
        };

        private void _simulateMouseUpEventTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var self = this;
            Task.Run(() =>
            {
                self.Dispatcher.BeginInvoke(new Action(delegate
                {
                    _logger.DebugFormat("[Key] SimulateMouseUpEventTimer.Elapsed fired. Text: {0}", txt.Text);
                    PerformMouseUpAction(true, true);

                }), DispatcherPriority.Input);
            });



        }

        #endregion

        private void PerformMouseDownAction(bool withSound, bool fireClickEventHandler = true)
        {

            if (_enabled && State != ButtonState.Pressed)
            {
                var self = this;

                SetButtonPressedStateStyle();

                //play sound
                if (withSound)
                {
                    SoundController.Instance.PlayButtonSound();
                }

                if (KeyPressing != null && fireClickEventHandler)
                {
                    KeyPressing(self, new KeyPressingEventArgs(self.Symbol));
                }

                State = ButtonState.Pressed;


                //register automatic MouseUp Action if event did not happen //workaround for Surface Go Tablets
                _simulateMouseUpEventTimer.Stop();
                _simulateMouseUpEventTimer.Start();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fireClickEventHandler">Default is true.</param>
        /// <param name="forceFireEventImmidiatelly">When true, disable the delay to trigger Un-pressed style. Default is false</param>
        private void PerformMouseUpAction(bool fireClickEventHandler = true, bool forceFireEventImmidiatelly = false)
        {
            _logger.DebugFormat("[Key] PerformMouseUpAction called: {0}", this.Text);

            _simulateMouseUpEventTimer.Stop();

            if (_enabled && State == ButtonState.Pressed)
            {
                var self = this;
                var delayTimeoutInMilliseconds = forceFireEventImmidiatelly ? 0 : 400;

                Task.Delay(delayTimeoutInMilliseconds).ContinueWith((t) =>
                {
                    self.Dispatcher.BeginInvoke(new Action(delegate
                    {
                        self.ApplyTemplate(DepressedTemplateKey);
                    }), DispatcherPriority.Input);
                });
                


                if (KeyPressed != null && fireClickEventHandler)
                {
                    KeyPressed(self, new EventArgs());
                }

                State = ButtonState.Default;
            }
        }

        /// <summary>
        /// Simulate button press action
        /// </summary>
        public void Press()
        {
            PerformMouseDownAction(false);
            PerformMouseUpAction();
        }

        private void UserControl_TouchDown(object sender, TouchEventArgs e)
        {
            _logger.DebugFormat("[Key] TouchDown occured.");
        }

        private void UserControl_TouchLeave(object sender, TouchEventArgs e)
        {
            _logger.DebugFormat("[Key] TouchLeave occured.");
        }

        private void UcLayout_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }


    public class KeyPressingEventArgs : EventArgs
    {
        public char KeyCode { get; set; }

        public KeyPressingEventArgs(char keyCode)
                : base()
        {
            this.KeyCode = keyCode;
        }
    }

    public enum ButtonState
    {
        Default = 0,
        Pressed = 1
    }


}
