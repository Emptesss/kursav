using System;
using System.Globalization;
using System.Windows.Data;

namespace CatGame.Helpers
{
    public class DirectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double direction)
                return direction * 100; // Масштабирование длины стрелки
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}