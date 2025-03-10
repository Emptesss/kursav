// Helpers/CenterConverter.cs
using CatGame.ViewModels;
using System;
using System.Globalization;
using System.Windows.Data;

namespace CatGame.Helpers
{
    public class CenterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value - (MiniGame2ViewModel.BubbleSize / 2);
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}