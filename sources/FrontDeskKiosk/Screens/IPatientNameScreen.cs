using FrontDesk.Kiosk.Controls.Keyboard;
using FrontDesk.Kiosk.Controls.TextBoxExtensions;
using System.Windows.Controls;

namespace FrontDesk.Kiosk.Screens
{
    public interface IPatientNameScreen : IStatefulVisualScreen
    {
        KeyboardControl KeyboardControl { get; }
        TerminalTextBox TextInputCtrl { get; }
        TextBlock FieldLabel { get; }
        TextBlock InvalidEhrFieldLabel { get; }

    }
}
