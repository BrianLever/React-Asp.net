using System;
using FrontDesk.Kiosk.Controls.Keyboard;

namespace FrontDesk.Kiosk.Screens.Behaviors
{
    public interface ITextInputControlBehavior
    {
        //event EventHandler<NextScreenClickedEventArg> NextScreenClicked;

        void btnBackspace_KeyPressing(object sender, KeyPressingEventArgs e);
        void Hide();
        void Init();
        void Reset();
        void Show(bool withAnimation);
    }
}