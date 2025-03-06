using System;
using System.Globalization;
using System.Windows.Data;
using CatGame.Models;

namespace CatGame.Helpers
{
    public class SelectedToTextConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is Skin selected && values[1] is Skin current)
            {
                return selected == current ? "Выключить" : "Включить";
            }
            return "Включить";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}