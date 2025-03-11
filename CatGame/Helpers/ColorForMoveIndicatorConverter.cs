using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace CatGame.Helpers
{
    public class ColorForMoveIndicatorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 2)
                return "#FFB6C1";

            try
            {
                int index = (int)values[0];
                int nextColor = (int)values[1];

                // Если это первый индикатор (индекс 0), показываем следующий цвет
                if (index == 0)
                {
                    return ((SolidColorBrush)new ColorIndexToBrushConverter()
                        .Convert(nextColor, typeof(Brush), null, culture)).Color;
                }

                // Для остальных индикаторов возвращаем розовый цвет
                return Color.FromRgb(255, 182, 193); // #FFB6C1
            }
            catch
            {
                return Color.FromRgb(255, 182, 193);
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
