using System.Windows.Media;

namespace FrontDesk.Kiosk.Controls.TextBoxExtensions
{
	public static class MaskHelper
	{
		public static void InitializeMaskItem(MaskedTerminalTextBox tb,
				MaskedTerminalTextBox next, MaskedTerminalTextBox previous,
				FrontDesk.Kiosk.Controls.TextBoxExtensions.MaskedTerminalTextBox.GetAllowedValuesDelegate handler,
				char blank,
				System.Windows.Media.Color watermarkForeground,
				System.Windows.Media.Color filledInControlForeground)
		{
			tb.Next = next;
			tb.Previous = previous;
			tb.GetAllowedValuesHandler = handler;
			tb.WatermarkForeground = watermarkForeground;
			tb.FilledInControlForeground = filledInControlForeground;
			tb.SetEmptyValue(blank);
		}


		public static Color WatermarkForeground = Color.FromArgb(0x99, 0xee, 0xee, 0xee);
		public static Color FilledInControlForeground = Color.FromArgb(0xff, 0x4c, 0x4c, 0x4c);
	}
}
