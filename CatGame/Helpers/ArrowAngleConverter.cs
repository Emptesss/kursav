using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CatGame.Helpers
{
    public class ArrowAngleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2 || !(values[0] is Point start) || !(values[1] is Point end))
                return 0.0;

            double dx = end.X - start.X;
            double dy = end.Y - start.Y;

            // Преобразуем радианы в градусы
            return Math.Atan2(dy, dx) * 180 / Math.PI;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
