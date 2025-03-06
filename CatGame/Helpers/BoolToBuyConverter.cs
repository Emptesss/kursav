using System;
using System.Globalization;
using System.Windows.Data;
using CatGame.Models;

namespace CatGame.Helpers
{
    public class BoolToBuyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isPurchased)
            {
                return isPurchased ? "Куплено" : $"Купить ({parameter})";
            }
            return "Купить";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}