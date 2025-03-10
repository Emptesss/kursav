using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CatGame.Helpers
{
    public class ArrowPointConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2 || !(values[0] is Point start) || !(values[1] is Point end))
                return new Point();

            double dx = end.X - start.X;
            double dy = end.Y - start.Y;
            double angle = Math.Atan2(dy, dx);
            double length = Math.Sqrt(dx * dx + dy * dy);

            // Размер наконечника стрелки
            double arrowSize = 20;

            string position = parameter as string;

            switch (position)
            {
                case "tip":
                    return end;
                case "left":
                    return new Point(
                        end.X - arrowSize * Math.Cos(angle - Math.PI / 6),
                        end.Y - arrowSize * Math.Sin(angle - Math.PI / 6)
                    );
                case "right":
                    return new Point(
                        end.X - arrowSize * Math.Cos(angle + Math.PI / 6),
                        end.Y - arrowSize * Math.Sin(angle + Math.PI / 6)
                    );
                default:
                    return new Point();
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
