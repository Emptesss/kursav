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
    Brushes.Red,
    Brushes.Blue,
    Brushes.Green,
    Brushes.Yellow,
    Brushes.Purple,
    Brushes.Orange // 6-й цвет
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