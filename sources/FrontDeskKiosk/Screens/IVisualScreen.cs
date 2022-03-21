using System;
using System.Windows.Controls;

namespace FrontDesk.Kiosk.Screens
{
	public interface IVisualScreen
    {
        event EventHandler<NextScreenClickedEventArg> NextScreenClicked;

        Button GetDefaultButtonForTesting();

        /// <summary>
        /// Restore screen's state to the default
        /// </summary>
        void Reset();


        void Show(bool withAnimation);
        void Hide();
    }
}
