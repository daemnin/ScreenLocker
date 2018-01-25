using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ScreenLocker.Assets.Converters
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) throw new ArgumentNullException("value");
            var isVisible = (bool)value;

            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) throw new ArgumentException("value");

            var visibility = (Visibility)value;

            return visibility == Visibility.Visible;
        }
    }
}
