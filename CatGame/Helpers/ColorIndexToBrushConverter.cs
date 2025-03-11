using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CatGame.Helpers
{
    public class ColorIndexToBrushConverter : IValueConverter
    {
        // Переименуем массив, чтобы избежать конфликта имён
        private static readonly Brush[] ColorBrushes = {
    new SolidColorBrush(Color.FromRgb(163,51,78)),    
        new SolidColorBrush(Color.FromRgb(247,136,163)),     
        new SolidColorBrush(Color.FromRgb(208,133,151)),     
        new SolidColorBrush(Color.FromRgb(100,59,69)),     
        new SolidColorBrush(Color.FromRgb(155,81,99)),    
        new SolidColorBrush(Color.FromRgb(249,92,130)) 
};
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return Brushes.Transparent; // Добавленная проверка
            if (value is int index && index >= 0 && index < ColorBrushes.Length)
            {
                return ColorBrushes[index];
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}