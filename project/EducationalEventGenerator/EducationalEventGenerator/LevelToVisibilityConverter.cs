using System;
using System.Windows;
using System.Windows.Data;

namespace EducationalEventGenerator
{
    public class LevelToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int level && parameter is string requiredLevelStr)
            {
                if (int.TryParse(requiredLevelStr, out int requiredLevel))
                {
                    return level >= requiredLevel ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}