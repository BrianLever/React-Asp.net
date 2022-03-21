using System;
using System.Globalization;
using System.Windows.Data;

namespace FrontDesk.Kiosk.Converters
{
    public class ToUpperValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                              CultureInfo culture)
        {
            if (value is string)
            {
                return value.ToString().ToUpper();
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                  CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
