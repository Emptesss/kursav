using CatGame.ViewModels;
using System;
using System.Globalization;
using System.Windows.Data;

namespace CatGame.Helpers
{
    public class MoveIndicatorPositionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return 0;
            }

            try
            {
                int index = (int)value;
                double bubbleSize = MiniGame2ViewModel.BubbleSize;
                double spacing = bubbleSize * 1.2; // Небольшой отступ между шариками
                                                   // Меняем расчет startX и порядок шариков
                double startX = MiniGame2ViewModel.FieldWidth / 2 - (bubbleSize * 2); // Позиция текущего шарика
                return startX - ((index + 1) * spacing); // +1 чтобы начать левее текущего шарика
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}