using System;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace FrontDesk.Kiosk.Controls.TextBoxExtensions
{
	public partial class MaskedPhoneTextBox : UserControl
	{

		public Color WatermarkForeground { get; set; }
		public Color FilledInControlForeground { get; set; }

		/// <summary>
		/// Currently edditing textbox;
		/// </summary>
		private MaskedTerminalTextBox _currentControl;
		public MaskedTerminalTextBox CurrentControl
		{
			get
			{
				return _currentControl;
			}
		}

		private bool _completed;

		public event EventHandler ValueModified;

		/// <summary>
		/// Entered phone number
		/// </summary>
		public string Value
		{
			get
			{
				if (!_completed)
				{
					return null;
				}
				else
				{
					StringBuilder phone = new StringBuilder();
					phone.Append('(');
					phone.Append(ucNum1.GetValue<int>().Value);
					phone.Append(ucNum2.GetValue<int>().Value);
					phone.Append(ucNum3.GetValue<int>().Value);
					phone.Append(") ");
					phone.Append(ucNum4.GetValue<int>().Value);
					phone.Append(ucNum5.GetValue<int>().Value);
					phone.Append(ucNum6.GetValue<int>().Value);
					phone.Append('-');
					phone.Append(ucNum7.GetValue<int>().Value);
					phone.Append(ucNum8.GetValue<int>().Value);
					phone.Append(ucNum9.GetValue<int>().Value);
					phone.Append(ucNum10.GetValue<int>().Value);

					return phone.ToString();
				}
			}
		}


		public MaskedPhoneTextBox()
		{
			InitializeComponent();

            WatermarkForeground = MaskHelper.WatermarkForeground;
		    FilledInControlForeground = MaskHelper.FilledInControlForeground;


			_currentControl = ucNum1;
			_completed = false;

			char blank = ' ';

			MaskHelper.InitializeMaskItem(ucNum1, ucNum2, null, new MaskedTerminalTextBox.GetAllowedValuesDelegate(GetFirstPhoneDigitAllowedSymbols), blank, WatermarkForeground, FilledInControlForeground);
			MaskHelper.InitializeMaskItem(ucNum2, ucNum3, ucNum1, new MaskedTerminalTextBox.GetAllowedValuesDelegate(GetPhoneAllowedSymbols), blank, WatermarkForeground, FilledInControlForeground);
			MaskHelper.InitializeMaskItem(ucNum3, ucNum4, ucNum2, new MaskedTerminalTextBox.GetAllowedValuesDelegate(GetPhoneAllowedSymbols), blank, WatermarkForeground, FilledInControlForeground);
			MaskHelper.InitializeMaskItem(ucNum4, ucNum5, ucNum3, new MaskedTerminalTextBox.GetAllowedValuesDelegate(GetPhoneAllowedSymbols), blank, WatermarkForeground, FilledInControlForeground);
			MaskHelper.InitializeMaskItem(ucNum5, ucNum6, ucNum4, new MaskedTerminalTextBox.GetAllowedValuesDelegate(GetPhoneAllowedSymbols), blank, WatermarkForeground, FilledInControlForeground);
			MaskHelper.InitializeMaskItem(ucNum6, ucNum7, ucNum5, new MaskedTerminalTextBox.GetAllowedValuesDelegate(GetPhoneAllowedSymbols), blank, WatermarkForeground, FilledInControlForeground);
			MaskHelper.InitializeMaskItem(ucNum7, ucNum8, ucNum6, new MaskedTerminalTextBox.GetAllowedValuesDelegate(GetPhoneAllowedSymbols), blank, WatermarkForeground, FilledInControlForeground);
			MaskHelper.InitializeMaskItem(ucNum8, ucNum9, ucNum7, new MaskedTerminalTextBox.GetAllowedValuesDelegate(GetPhoneAllowedSymbols), blank, WatermarkForeground, FilledInControlForeground);
			MaskHelper.InitializeMaskItem(ucNum9, ucNum10, ucNum8, new MaskedTerminalTextBox.GetAllowedValuesDelegate(GetPhoneAllowedSymbols), blank, WatermarkForeground, FilledInControlForeground);
			MaskHelper.InitializeMaskItem(ucNum10, null, ucNum9, new MaskedTerminalTextBox.GetAllowedValuesDelegate(GetPhoneAllowedSymbols), blank, WatermarkForeground, FilledInControlForeground);
		}

		public char[] GetPhoneAllowedSymbols()
		{
			return new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '\u2408' };
		}

		public char[] GetFirstPhoneDigitAllowedSymbols()
		{
			return new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
		}

		/// <summary>
		/// Add eneterd symbol to TB
		/// </summary>
		public void Add(char c)
		{
			_currentControl.SetValue(c);
			if (_currentControl.Next != null)
			{
				_currentControl.ClearFocus();
				_currentControl = _currentControl.Next;

				if (ValueModified != null)
				{
					ValueModified(this, new EventArgs());
				}
			}
			else
			{
				_completed = true;
			}

			if (!_completed)
			{
				_currentControl.SetValue(null);
			}

			SetFocus();
		}

		/// <summary>
		/// Remove value from previous position
		/// </summary>
		public void Remove()
		{
			_completed = false;

			if (_currentControl.Next == null && _currentControl.GetValue<int>().HasValue)
			{
				_currentControl.SetValue(null);
				_currentControl.SetFocus();
				if (ValueModified != null)
				{
					ValueModified(this, new EventArgs());
				}
			}
			else if (_currentControl.Previous != null)
			{
				_currentControl.ClearFocus();
				_currentControl.SetEmptyValue();
				_currentControl = _currentControl.Previous;
				_currentControl.SetValue(null);
				_currentControl.SetFocus();

				if (ValueModified != null)
				{
					ValueModified(this, new EventArgs());
				}
			}
		}

		/// <summary>
		/// Clear entered info
		/// </summary>
		public void Clear()
		{
			_completed = false;

			ucNum1.Clear();
			ucNum2.Clear();
			ucNum3.Clear();
			ucNum4.Clear();
			ucNum5.Clear();
			ucNum6.Clear();
			ucNum7.Clear();
			ucNum8.Clear();
			ucNum9.Clear();
			ucNum10.Clear();

			_currentControl = ucNum1;

		}

		public void SetFocus()
		{
			_currentControl.SetFocus();
		}
	}
}
