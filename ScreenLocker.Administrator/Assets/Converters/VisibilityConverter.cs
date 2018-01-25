using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ScreenLocker.Administrator.Assets.Converters
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) throw new ArgumentNullException("value");
            var isVisible = (bool)value;

            if (parameter?.Equals("Invert") == true)
                isVisible = !isVisible;

            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) throw new ArgumentException("value");

            var visibility = (Visibility)value;

            if (parameter?.Equals("Invert") == true) return visibility != Visibility.Visible;

            return visibility == Visibility.Visible;
        }
    }
}
