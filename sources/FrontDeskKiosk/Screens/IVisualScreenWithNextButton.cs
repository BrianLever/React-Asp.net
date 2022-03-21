using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using FrontDesk.Kiosk.Controls.Keyboard;
using FrontDesk.Kiosk.Controls.TextBoxExtensions;
using FrontDesk.Kiosk.Services;

namespace FrontDesk.Kiosk.Screens
{
    public interface IDemographicsLookupVisualScreen : IVisualScreen
    {

        Button NextButton { get; }
        DemographicsLookupServiceBase LookupService { get; }
        WrapPanel AnswerOptionsPanel { get; }
        void TriggerNextScreenEvent(int selectedItemId);

    }
}
