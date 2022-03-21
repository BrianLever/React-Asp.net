using FrontDesk.Kiosk.Controls.Keyboard;
using FrontDesk.Kiosk.Controls.TextBoxExtensions;
using System.Windows.Controls;

namespace FrontDesk.Kiosk.Screens
{
    public interface IPatientBirthdayScreen : IStatefulVisualScreen
    {
        KeyboardControl KeyboardControl { get; }
        MaskedDateTextBox InputCtrl { get; }
        TextBlock FieldLabel { get; }
        TextBlock InvalidEhrFieldLabel { get; }
    }
}
