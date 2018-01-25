using System;
using System.Globalization;
using System.Windows.Data;

namespace ScreenLocker.Administrator.Assets.Converters
{
    public class ButtonStatusConverter : IValueConverter
    {
        private const string ACTIVE_STATUS = "Habilitar";
        private const string NOT_ACTIVE_STATUS = "Deshabilitar";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) throw new ArgumentNullException("value");
            var isActive = (bool)value;

            return !isActive ? ACTIVE_STATUS : NOT_ACTIVE_STATUS;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) throw new ArgumentNullException("value");

            return !value.Equals(ACTIVE_STATUS);
        }
    }
}
