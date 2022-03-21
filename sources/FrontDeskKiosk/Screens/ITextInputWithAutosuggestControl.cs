using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using FrontDesk.Kiosk.Controls.Keyboard;
using FrontDesk.Kiosk.Controls.TextBoxExtensions;
using FrontDesk.Kiosk.Services;
using FrontDesk.Kiosk.Workflow;

namespace FrontDesk.Kiosk.Screens
{
    public interface ITextInputWithAutosuggestControl : IStatefulVisualScreen
    {

        KeyButton BackspaceButton { get; }
        KeyboardControl KeyboardControl { get; }
        TerminalTextBox TextInputCtrl { get; }
        Button NextButton { get; }
        TypeaheadDataSourceBase<string> TypeAheadService { get; }
        void TriggerNextScreenEvent(string inputValue);
    }
}
