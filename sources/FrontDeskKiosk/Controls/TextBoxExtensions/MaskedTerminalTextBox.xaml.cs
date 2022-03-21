using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace FrontDesk.Kiosk.Controls.TextBoxExtensions
{
    public partial class MaskedTerminalTextBox : UserControl
    {
        public MaskedTerminalTextBox Next { get; set; }
        public MaskedTerminalTextBox Previous { get; set; }

        private char _blank = '_';

        public Color WatermarkForeground { get; set; }
        public Color FilledInControlForeground { get; set; }

        /// <summary>
        /// Set a value to masked textobx item
        /// if c is null - an empty string will be added
        /// </summary>
        public void SetValue(char? c)
        {
            ucText.txt.Foreground = new SolidColorBrush(FilledInControlForeground);
            ucText.Text = c.HasValue ? c.Value.ToString() : String.Empty;
        }

        public void SetEmptyValue(char blank)
        {
            this._blank = blank;
            SetEmptyValue();
        }

        public void SetEmptyValue()
        {
            ucText.txt.Foreground = new SolidColorBrush(WatermarkForeground);
            ucText.Text = _blank.ToString();
        }


        /// <summary>
        /// Get entered value
        /// </summary>
        public T? GetValue<T>()
                where T : struct
        {
            if (String.IsNullOrEmpty(ucText.Text) || ucText.Text[0] == this._blank)
            {
                return null;
            }
            return (T)Convert.ChangeType(ucText.Text, typeof(T));
        }

        public void Clear()
        {
            SetEmptyValue();
            ucText.ClearFocus();
        }


        public delegate char[] GetAllowedValuesDelegate();
        public GetAllowedValuesDelegate GetAllowedValuesHandler;

        public MaskedTerminalTextBox()
        {
            InitializeComponent();

            WatermarkForeground = MaskHelper.WatermarkForeground;
            FilledInControlForeground = MaskHelper.FilledInControlForeground;
        }

        public void SetFocus()
        {
            ucText.SetFocus();
        }

        internal void ClearFocus()
        {
            ucText.ClearFocus();
        }
    }
}

