using System;
using System.Globalization;
using System.Windows.Data;
using CatGame.Models;

namespace CatGame.Helpers
{
    public class IsSelectedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Skin skin && skin == parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}