using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Common.Logging;
using FrontDesk.Kiosk.Controls.Keyboard;

namespace FrontDesk.Kiosk.Screens.Behaviors
{
    public class TextInputWithPopupSuggestControlBehavior : ITextInputControlBehavior
    {
        protected ScreenComponentHelper _screenHelper;
        protected int MaxITtemsToDisplay = 5;
        protected int MinLookupLength = 3;

        private ITextInputWithPopupSuggestControl _control;

        private ILog _logger = LogManager.GetLogger<TextInputWithPopupSuggestControlBehavior>();

        public TextInputWithPopupSuggestControlBehavior(ITextInputWithPopupSuggestControl control)
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

            _control.ReturnToKeyboardButton.Click += OnReturnButton_Click;
        }




        public void btnBackspace_KeyPressing(object sender, KeyPressingEventArgs e)
        {

        }

        private void btnBackspace_KeyPressed(object sender, EventArgs e)
        {
            _control.TextInputCtrl.Remove();
            var inputValue = _control.TextInputCtrl.Text;

            if (inputValue.Length < MinLookupLength)
            {
                HideMatchedItems();

                _logger.Debug("Hide matched items called.");
            }
            else
            {
                UpdateMatchedItems(inputValue);
                _logger.Debug("Refesh matched items.");
            }
            InitKeyboard(inputValue);
            Validate(inputValue);
        }



        // Find matched items
        private List<string> FindBestMatches(string inputValue)
        {
            List<string> filteredValues = _control.TypeAheadService.GetByPartOfName(inputValue);


            if (filteredValues.Count > MaxITtemsToDisplay)
            {
                //too many options
                return new List<string>(0);
            }
            else
            {
                return filteredValues;
            }
        }
        private DispatcherTimer _renderMatchedItemsTimer;

        private void RenderMatchedItems(ICollection<string> items)
        {
            //Display
            var itemsToDisplay = items;

            _logger.DebugFormat("[TextInputWithPopupSuggestControlBehavior][RenderMatchedItems] Calling RenderMatchedItems with item count {0}.", items.Count);

            if (_renderMatchedItemsTimer != null && _renderMatchedItemsTimer.IsEnabled)
            {
                _renderMatchedItemsTimer?.Stop();

                _logger.DebugFormat("[TextInputWithPopupSuggestControlBehavior][RenderMatchedItems] Terminated timer.");

            }

            _renderMatchedItemsTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1000)
            };

            _renderMatchedItemsTimer.Tick += (sendr, args) =>
            {
                if (!cancelMachedItemRender) /* if items become hidden, do not render */
                {
                    _logger.DebugFormat("[TextInputWithPopupSuggestControlBehavior][RenderMatchedItems] Calling DisplayMatchedItems method.");
                    DisplayMatchedItems(itemsToDisplay);

                    _logger.DebugFormat("[TextInputWithPopupSuggestControlBehavior][RenderMatchedItems] Matched items displayed.");
                }
                else
                {
                    _logger.Debug("Cancelling rendering. cancelMachedItemRender: true");

                }
                _renderMatchedItemsTimer.Stop();
            };

            _renderMatchedItemsTimer.Start();
        }

        private void ucKeyboard_KeyPressing(object sender, KeyPressingEventArgs e)
        {
            _control.TextInputCtrl.Add(e.KeyCode);
            InitKeyboard();
        }

        // Lookup of the value
        private void ucKeyboard_KeyPressed(object sender, EventArgs e)
        {
            onKeyPressed();
        }




        private void onKeyPressed()
        {
            string inputValue = _control.TextInputCtrl.Text;

            if (string.IsNullOrWhiteSpace(inputValue) || inputValue.Length < MinLookupLength)
            {
                return;
            }


            UpdateMatchedItems(inputValue);

            Validate(inputValue);
        }



        private void Validate(string inputValue)
        {
            bool isValid = false;

            if (String.IsNullOrWhiteSpace(inputValue))
            {
                isValid = false;
            }
            else
            {
                isValid = true;
            }

            _control.NextButton.IsEnabled = isValid;
        }

        private void OnNext(object sender, RoutedEventArgs e)
        {
            string inputValue = _control.TextInputCtrl.Text.Trim();

            _control.TriggerNextScreenEvent(inputValue);
        }



        public void Hide()
        {
            _screenHelper.Hide();
        }

        public void Reset()
        {
            _control.NextButton.IsEnabled = false;
            _control.TextInputCtrl.Clear();

            Hide();
        }

        public void Show(bool withAnimation)
        {
            _control.BackspaceButton.SetFontSize(25);

            _control.TextInputCtrl.SetFocus();

            InitKeyboard();

            _screenHelper.Show(withAnimation);
        }

        protected void InitKeyboard(string inputValue)
        {
            if (string.IsNullOrWhiteSpace(inputValue))
            {
                var disabledChars = new List<char>();

                disabledChars.Add(' ');
                disabledChars.Add('-');
                disabledChars.Add(',');
                disabledChars.Add('.');
                disabledChars.Add('_');

                _control.KeyboardControl.SetDisabledState(disabledChars.ToArray());
            }
            else
            {
                _control.KeyboardControl.SetDisabledState(new char[0]);

            }

        }

        protected void InitKeyboard()
        {
            var textInput = _control.TextInputCtrl.Text;
            InitKeyboard(textInput);
        }


        #region Matched Items


        protected void UpdateMatchedItems(string inputValue)
        {
            var matchedItems = FindBestMatches(inputValue);

            if (matchedItems.Count == 0)
            {
                HideMatchedItems();
                return;
            }
            cancelMachedItemRender = false;
            Application.Current.Dispatcher.BeginInvoke(new Action(() => RenderMatchedItems(matchedItems)));
        }

        protected void ShowMatchedItems()
        {
            cancelMachedItemRender = false;
            _control.MatchedItemsPanel.Visibility = System.Windows.Visibility.Visible;
        }

        protected void BindMatchedItems(ICollection<string> items)
        {
            _control.MatchedItemsContainer.Children.Clear();

            _control.MatchedItemsContainer.BeginInit();
            try
            {
                foreach (var textItem in items)
                {
                    var keyButton = new KeyButton
                    {
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                        Text = textItem,
                        DepressedTemplateKey = "AutoSuggestPopupItemBtn",
                        DepressedTemplateKeyLongText = "AutoSuggestPopupItemBtnMultiLine",

                        PressedTemplateKey = "AutoSuggestPopupItemBtn_Pressed",
                        PressedTemplateKeyLongText = "AutoSuggestPopupItemBtnMultiLine_Pressed",

                        DisabledTemplateKey = "AutoSuggestPopupItemBtn_Disabled",
                        DisabledTemplateKeyLongText = "AutoSuggestPopupItemBtnMultiLine_Disabled",

                        SingleLineTextMaxCharacters = 65,
                        Margin = new System.Windows.Thickness(10, 0, 0, 5),
                        Style = (Style)Application.Current.FindResource("AutoSuggestPopupItem"),
                    };

                    keyButton.KeyPressed += KeyButton_KeyPressed;

                    _control.MatchedItemsContainer.Children.Add(keyButton);
                }
            }
            finally
            {
                _control.MatchedItemsContainer.EndInit();
            }


        }

        private void KeyButton_KeyPressed(object sender, EventArgs e)
        {
            var ctl = (KeyButton)sender;
            _control.TextInputCtrl.Text = ctl.Text;
            HideMatchedItems();
        }

        private bool cancelMachedItemRender = false;

        protected void HideMatchedItems()
        {
            _control.MatchedItemsContainer.BeginInit();
            _control.MatchedItemsPanel.Visibility = System.Windows.Visibility.Collapsed;
            _control.MatchedItemsContainer.Children.Clear();

            cancelMachedItemRender = true;

            _control.MatchedItemsContainer.EndInit();
        }

        private void OnReturnButton_Click(object sender, RoutedEventArgs e)
        {
            HideMatchedItems();
        }


        public void DisplayMatchedItems(ICollection<string> items)
        {
            if (items.Count > 0)
            {
                BindMatchedItems(items);
                ShowMatchedItems();
            }
            else
            {
                HideMatchedItems();
            }
        }

        #endregion


    }
}
