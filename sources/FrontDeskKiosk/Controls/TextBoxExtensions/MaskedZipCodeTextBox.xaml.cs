using System;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace FrontDesk.Kiosk.Controls.TextBoxExtensions
{
	/// <summary>
	/// Zip code masked textbox
	/// </summary>
	public partial class MaskedZipCodeTextBox : UserControl
	{
		public Color WatermarkForeground { get; set; }
		public Color FilledInControlForeground { get; set; }

		public MaskedZipCodeTextBox()
		{
			InitializeComponent();

            WatermarkForeground = MaskHelper.WatermarkForeground;
		    FilledInControlForeground = MaskHelper.FilledInControlForeground;

			_currentControl = ucNum1;
			_completed = false;

			char _blank = ' ';

			MaskHelper.InitializeMaskItem(ucNum1, ucNum2, null, new MaskedTerminalTextBox.GetAllowedValuesDelegate(GetAllowedValuesOfFirstNumber), _blank, WatermarkForeground, FilledInControlForeground);
			MaskHelper.InitializeMaskItem(ucNum2, ucNum3, ucNum1, new MaskedTerminalTextBox.GetAllowedValuesDelegate(GetAllowedValues), _blank, WatermarkForeground, FilledInControlForeground);
			MaskHelper.InitializeMaskItem(ucNum3, ucNum4, ucNum2, new MaskedTerminalTextBox.GetAllowedValuesDelegate(GetAllowedValues), _blank, WatermarkForeground, FilledInControlForeground);
			MaskHelper.InitializeMaskItem(ucNum4, ucNum5, ucNum3, new MaskedTerminalTextBox.GetAllowedValuesDelegate(GetAllowedValues), _blank, WatermarkForeground, FilledInControlForeground);
			MaskHelper.InitializeMaskItem(ucNum5, null, ucNum4, new MaskedTerminalTextBox.GetAllowedValuesDelegate(GetAllowedValues), _blank, WatermarkForeground, FilledInControlForeground);
		}

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

		/// <summary>
		/// Entered zip code.
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
					StringBuilder zip = new StringBuilder();
					zip.Append(ucNum1.GetValue<int>().Value);
					zip.Append(ucNum2.GetValue<int>().Value);
					zip.Append(ucNum3.GetValue<int>().Value);
					zip.Append(ucNum4.GetValue<int>().Value);
					zip.Append(ucNum5.GetValue<int>().Value);

					return zip.ToString();
				}
			}
		}

		public event EventHandler ValueModified;

		private bool _completed;

		#region value limitation methods

		private char[] GetAllowedValuesOfFirstNumber()
		{
			return new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
		}

		private char[] GetAllowedValues()
		{
			return new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '\u2408' };
		}

		#endregion

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

			_currentControl = ucNum1;
		}


		public void SetFocus()
		{
			_currentControl.SetFocus();
		}

	}
}
