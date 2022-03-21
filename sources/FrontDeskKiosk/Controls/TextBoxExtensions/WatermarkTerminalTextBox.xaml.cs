using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FrontDesk.Kiosk.Controls.TextBoxExtensions
{
	/// <summary>
	/// Interaction logic for WatermarkTerminalTextBox.xaml
	/// </summary>
	public partial class WatermarkTerminalTextBox : UserControl
	{
        public Color WatermarkForeground { get; set; }
        public Color FilledInControlForeground { get; set; }

		public WatermarkTerminalTextBox()
		{
			InitializeComponent();
			ucText.IsTemplated = true;

            WatermarkForeground = MaskHelper.WatermarkForeground;
		    FilledInControlForeground = MaskHelper.FilledInControlForeground;

		}

		private String _watermark { get; set; }

		public event EventHandler<TextChangedEventArgs> TextChanged;

		private void ucText_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (TextChanged != null)
			{
				TextChanged(this, e);
			}
		}
		public int MaxLength
		{
			get
			{
				return ucText.txt.MaxLength;
			}
			set
			{
				ucText.txt.MaxLength = value;
			}
		}
		public String Watermark
		{
			get
			{
				return _watermark;
			}
			set
			{
				_watermark = value;
			}
		}

		public String Text
		{
			get
			{
				return ucText.txt.Text;
			}
			set
			{
				ucText.txt.Text = value;
			}
		}

		public void SetWatermark()
		{
			ucText.IsTemplated = true;
			ucText.txt.Foreground = new SolidColorBrush(WatermarkForeground);
			ucText.Text = _watermark;
		}
		public void Clear()
		{
			ucText.IsTemplated = false;
			ucText.txt.Foreground = new SolidColorBrush(FilledInControlForeground);
			ucText.Clear();
		}
		public void Add(char c)
		{
			if (ucText.IsTemplated)
			{
				Clear();
			}
			ucText.Add(c);
		}
		public void Remove()
		{
			if (!IsEmpty())
			{
				ucText.Remove();
			}
			if (IsEmpty())
			{
				SetWatermark();
			}
		}
		public bool IsEmpty()
		{
			return (ucText.IsTemplated || String.IsNullOrEmpty(ucText.txt.Text));
		}
		public void SetFocus()
		{
			SetWatermark();
			ucText.SetFocus();
		}
		public void ClearFocus()
		{
			ucText.ClearFocus();
		}
	}
}
