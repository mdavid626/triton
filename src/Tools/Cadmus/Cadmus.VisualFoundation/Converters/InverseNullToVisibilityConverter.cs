using System;
using System.Windows;
using System.Windows.Data;

namespace Cadmus.VisualFoundation.Converters
{
    public class InverseNullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool hidden = false;
            if (parameter != null)
                hidden = System.Convert.ToBoolean(parameter);
            var hiddenValue = hidden ? Visibility.Hidden : Visibility.Collapsed;
            return value != null ? hiddenValue : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}