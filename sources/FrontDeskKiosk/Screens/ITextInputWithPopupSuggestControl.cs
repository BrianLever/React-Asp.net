using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace FrontDesk.Kiosk.Screens
{
    public interface ITextInputWithPopupSuggestControl : ITextInputWithAutosuggestControl, IVisualScreen
    {
        
        FrameworkElement MatchedItemsPanel { get; }
        Panel MatchedItemsContainer { get; }
        Button ReturnToKeyboardButton { get; }
    }
}
