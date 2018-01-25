using System;
using System.Globalization;
using System.Windows.Data;

namespace ScreenLocker.Administrator.Assets.Converters
{
    public class LocalDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            var utc = (DateTime)value;

            return utc.ToLocalTime();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
