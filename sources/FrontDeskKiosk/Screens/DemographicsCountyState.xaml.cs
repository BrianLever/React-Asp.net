using System;
using System.Windows.Controls;

using FrontDesk.Kiosk.Controls.Keyboard;
using FrontDesk.Kiosk.Controls.TextBoxExtensions;
using FrontDesk.Kiosk.Screens.Behaviors;
using FrontDesk.Kiosk.Services;
using FrontDesk.Kiosk.Workflow;

namespace FrontDesk.Kiosk.Screens
{
    public partial class DemographicsCountyState : UserControl, ITextInputWithAutosuggestControl, IVisualScreen
    {
        private TextInputWithAutosuggestContrlBehavior _behavour;
        private readonly DemographicsCountyStateService _countryStateService;

        public DemographicsCountyState()
        {
            InitializeComponent();

            _countryStateService = ScreenTypeAheadDataSources.Default.CountyStates;
            this.TypeAheadService = (TypeaheadDataSourceBase<string>)_countryStateService;

            _behavour = new TextInputWithAutosuggestContrlBehavior(this);
            _behavour.Init();

        }



        #region ITextInputWithAutosuggestControl
        public TypeaheadDataSourceBase<string> TypeAheadService { private set; get; }
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
            _countryStateService.SelectedCountyName = string.Empty;
            _behavour.Reset();
            
        }

        public void Show(bool withAnimation)
        {
            _countryStateService.SelectedCountyName = ResultState.Demographics.CountyNameOfResidence;
            _countryStateService.Refresh();

            _behavour.Show(withAnimation);
        }

        public void Hide()
        {
            _behavour.Hide();
        }

        #endregion

    }
}
