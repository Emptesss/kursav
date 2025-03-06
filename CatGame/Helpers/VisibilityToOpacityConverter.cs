using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CatGame.Helpers
{
    public class VisibilityToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Проверяем тип значения и возвращаем соответствующую прозрачность
            if (value is bool isVisible)
            {
                return isVisible ? 1.0 : 0.3;
            }
            return 1.0; // Значение по умолчанию
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}