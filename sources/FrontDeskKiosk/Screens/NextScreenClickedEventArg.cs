using System;

namespace FrontDesk.Kiosk.Screens
{
	/// <summary>
	/// NextScreenClicked event arguments
	/// </summary>
	public class NextScreenClickedEventArg : EventArgs
    {
        /// <summary>
        /// Entered value from the leaving step
        /// </summary>
        public object Value { get; set; }

        public IVisualSectionScreen Screen { get; set; }

        public NextScreenClickedEventArg(IVisualSectionScreen screen, object value)
        {
            this.Value = value;
            this.Screen = screen;
        }
    }
}
