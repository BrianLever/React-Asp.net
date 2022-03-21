using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using FrontDesk.Kiosk.Screens;
using FrontDesk.Kiosk.Workflow;

namespace FrontdDeskKiosk.Tests.VisualScreenControllerTests
{
    internal class FakeVisualScreen : IVisualScreen
    {
        public event EventHandler<NextScreenClickedEventArg> NextScreenClicked;
        public Button GetDefaultButtonForTesting()
        {
            return null;
        }

        public void Reset()
        {
            
        }

        public void Show(bool withAnimation)
        {
           
        }

        public void Hide()
        {
            
        }

        public ScreeningStep Step { get; set; }
    }
}
