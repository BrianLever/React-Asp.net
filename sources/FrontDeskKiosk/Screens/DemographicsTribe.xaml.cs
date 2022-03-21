using FrontDesk.Kiosk.Controls.Keyboard;
using FrontDesk.Kiosk.Controls.TextBoxExtensions;
using FrontDesk.Kiosk.Screens.Behaviors;
using FrontDesk.Kiosk.Services;
using FrontDesk.Kiosk.Workflow;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace FrontDesk.Kiosk.Screens
{
    public partial class DemographicsTribe : UserControl, ITextInputWithPopupSuggestControl, IVisualScreen
    {
        private ITextInputControlBehavior _behavour;

        public DemographicsTribe()
        {
            InitializeComponent();
            _behavour = new TextInputWithPopupSuggestControlBehavior(this);
            _behavour.Init();
            pnlMatchedItems.Visibility = Visibility.Collapsed;



            pnlMatchedItems.IsVisibleChanged += PnlMatchedItems_IsVisibleChanged;


        }

        private void PnlMatchedItems_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if((bool)e.NewValue == true)
            {
                var animation = (Storyboard)FindResource("DisplaySuggestedList");

                animation.Begin(pnlMatchedItems);
            }
        }

        #region ITextInputWithAutosuggestControl
        public TypeaheadDataSourceBase<string> TypeAheadService => ScreenTypeAheadDataSources.Default.Tribes;
        public KeyButton BackspaceButton => btnBackspace;
        public KeyboardControl KeyboardControl => ucKeyboard;
        public TerminalTextBox TextInputCtrl => ucText;
        public Button NextButton => btnNext;
        public FrameworkElement MatchedItemsPanel => pnlMatchedItems;
        public Panel MatchedItemsContainer => pnlMatchedItemsContainer;
        public Button ReturnToKeyboardButton => btnReturn;
        public IScreeningResultState ResultState { get; set; }

        public void TriggerNextScreenEvent(string inputValue)
        {
            NextScreenClicked?.Invoke(this, new NextScreenClickedEventArg(null, inputValue));
        }

        #endregion


        #region IVisualScreen Members

        public event EventHandler<NextScreenClickedEventArg> NextScreenClicked;

        public Button GetDefaultButtonForTesting()
        {
            return btnNext;
        }

        public void Reset()
        {
            _behavour.Reset();
        }

        public void Show(bool withAnimation)
        {
            _behavour.Show(withAnimation);
        }

        public void Hide()
        {
            _behavour.Hide();
        }

       


        #endregion

    }
}
