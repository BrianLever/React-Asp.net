using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FrontDesk.Kiosk.Controls.TextBoxExtensions
{
    public partial class MaskedDateTextBox : UserControl
    {
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

        public DateTime? Value
        {
            get
            {
                if (!_completed)
                {
                    return null;
                }
                else
                {
                    StringBuilder day = new StringBuilder();
                    day.Append(ucDay1.GetValue<int>().Value);
                    day.Append(ucDay2.GetValue<int>().Value);

                    StringBuilder month = new StringBuilder();
                    month.Append(ucMonth1.GetValue<int>().Value);
                    month.Append(ucMonth2.GetValue<int>().Value);

                    StringBuilder year = new StringBuilder();
                    year.Append(ucYear1.GetValue<int>().Value);
                    year.Append(ucYear2.GetValue<int>().Value);
                    year.Append(ucYear3.GetValue<int>().Value);
                    year.Append(ucYear4.GetValue<int>().Value);

                    return new DateTime(Convert.ToInt32(year.ToString()),
                            Convert.ToInt32(month.ToString()), Convert.ToInt32(day.ToString()));
                }
            }
        }

        public Color WatermarkForeground { get; set; }
        public Color FilledInControlForeground { get; set; }


        public event EventHandler ValueModified;


        private bool _completed;

        public MaskedDateTextBox()
        {
            InitializeComponent();

            _currentControl = ucMonth1;
            _completed = false;

            WatermarkForeground = MaskHelper.WatermarkForeground;
            FilledInControlForeground = MaskHelper.FilledInControlForeground;
        }


        #region value limitation methods

        private char[] GetAllowedValuesOfFirstDayPosition()
        {
            return new char[] { '0', '1', '2', '3', '\u2408' };
        }

        private char[] GetAllowedValuesOfSecondDayPosition()
        {
            int firstDay = ucDay1.GetValue<int>().Value;
            if (firstDay == 0)
            {
                return new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '\u2408' };
            }
            else if (firstDay < 3)
            {
                return new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '\u2408' };
            }
            return new char[] { '0', '1', '\u2408' };
        }

        private char[] GetAllowedValuesOfFirstMonthPosition()
        {
            return new char[] { '0', '1' };
        }

        private char[] GetAllowedValuesOfSecondMonthPosition()
        {
            if (ucMonth1.GetValue<int>().Value == 0)
            {
                return new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '\u2408' };
            }
            return new char[] { '0', '1', '2', '\u2408' };
        }

        private char[] GetAllowedValuesOfFirstYearPosition()
        {
            return new char[] { '1', '2', '\u2408' };
        }

        private char[] GetAllowedValuesOfSecondYearPosition()
        {
            if (ucYear1.GetValue<int>().Value == 1)
            {
                return new char[] { '8', '9', '\u2408' };
            }
            return new char[] { '0', '\u2408' };
        }

        private char[] GetAllowedValuesOfThirdYearPosition()
        {
            if (ucYear2.GetValue<int>().Value > 0)
            {
                return new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '\u2408' };
            }

            int max = Convert.ToInt32(DateTime.Now.Year.ToString()[2]);
            char[] range = new char[max + 2];
            for (int i = 0; i <= max; i++)
            {
                range[i] = Convert.ToChar(i);
            }
            range[range.Length - 1] = '\u2408';
            return range;
        }

        private char[] GetAllowedValuesOfFourthYearPosition()
        {
            if (ucYear1.GetValue<int>().Value == 1 ||
                    ucYear3.GetValue<int>().Value < int.Parse(DateTime.Now.Year.ToString()[2].ToString()))
            {
                return new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '\u2408' };
            }

            int max = Convert.ToInt32(DateTime.Now.Year.ToString()[3]);
            char[] range = new char[max + 2];
            for (int i = 0; i <= max; i++)
            {
                range[i] = Convert.ToChar(i);
            }

            range[range.Length - 1] = '\u2408';
            return range;

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
                //_currentControl.SetValue(null);
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
                //_currentControl.SetValue(null);
                _currentControl.SetEmptyValue();
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
                //_currentControl.SetValue(null);
                _currentControl.SetEmptyValue();
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

            ucDay1.Clear();
            ucDay2.Clear();
            ucMonth1.Clear();
            ucMonth2.Clear();
            ucYear1.Clear();
            ucYear2.Clear();
            ucYear3.Clear();
            ucYear4.Clear();

            _currentControl = ucMonth1;
        }

        public void SetFocus()
        {
            _currentControl.SetFocus();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            MaskHelper.InitializeMaskItem(ucMonth1, ucMonth2, null, new MaskedTerminalTextBox.GetAllowedValuesDelegate(GetAllowedValuesOfFirstMonthPosition), 'M', WatermarkForeground, FilledInControlForeground);
            MaskHelper.InitializeMaskItem(ucMonth2, ucDay1, ucMonth1, new MaskedTerminalTextBox.GetAllowedValuesDelegate(GetAllowedValuesOfSecondMonthPosition), 'M', WatermarkForeground, FilledInControlForeground);
            MaskHelper.InitializeMaskItem(ucDay1, ucDay2, ucMonth2, new MaskedTerminalTextBox.GetAllowedValuesDelegate(GetAllowedValuesOfFirstDayPosition), 'D', WatermarkForeground, FilledInControlForeground);
            MaskHelper.InitializeMaskItem(ucDay2, ucYear1, ucDay1, new MaskedTerminalTextBox.GetAllowedValuesDelegate(GetAllowedValuesOfSecondDayPosition), 'D', WatermarkForeground, FilledInControlForeground);
            MaskHelper.InitializeMaskItem(ucYear1, ucYear2, ucDay2, new MaskedTerminalTextBox.GetAllowedValuesDelegate(GetAllowedValuesOfFirstYearPosition), 'Y', WatermarkForeground, FilledInControlForeground);
            MaskHelper.InitializeMaskItem(ucYear2, ucYear3, ucYear1, new MaskedTerminalTextBox.GetAllowedValuesDelegate(GetAllowedValuesOfSecondYearPosition), 'Y', WatermarkForeground, FilledInControlForeground);
            MaskHelper.InitializeMaskItem(ucYear3, ucYear4, ucYear2, new MaskedTerminalTextBox.GetAllowedValuesDelegate(GetAllowedValuesOfThirdYearPosition), 'Y', WatermarkForeground, FilledInControlForeground);
            MaskHelper.InitializeMaskItem(ucYear4, null, ucYear3, new MaskedTerminalTextBox.GetAllowedValuesDelegate(GetAllowedValuesOfFourthYearPosition), 'Y', WatermarkForeground, FilledInControlForeground);
        }
    }
}
