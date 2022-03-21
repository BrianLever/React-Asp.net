using System;
using System.Windows.Controls;

using FrontDesk.Kiosk.Controls.Keyboard;
using FrontDesk.Kiosk.Controls.TextBoxExtensions;
using FrontDesk.Kiosk.Screens.Behaviors;
using FrontDesk.Kiosk.Services;
using FrontDesk.Kiosk.Workflow;

namespace FrontDesk.Kiosk.Screens
{
    public partial class DemographicsCountyName : UserControl, ITextInputWithAutosuggestControl, IVisualScreen
    {
        private TextInputWithAutosuggestContrlBehavior _behavour;

        public DemographicsCountyName()
        {
            InitializeComponent();

            _behavour = new TextInputWithAutosuggestContrlBehavior(this);
            _behavour.Init();

        }

        #region ITextInputWithAutosuggestControl
        public TypeaheadDataSourceBase<string> TypeAheadService => ScreenTypeAheadDataSources.Default.CountyNames;
        public KeyButton BackspaceButton => btnBackspace;
        public KeyboardControl KeyboardControl => ucKeyboard;
        public TerminalTextBox TextInputCtrl => ucText;
        public Button NextButton => btnNext;
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
