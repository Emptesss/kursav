using System;
using System.Globalization;
using System.Windows.Data;

namespace CatGame.Helpers
{
    public class BoolToScaleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool b && b) ? 1 : -1; // 1 — без изменений, -1 — отзеркалить
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}