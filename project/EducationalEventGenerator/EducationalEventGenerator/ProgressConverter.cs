using System;
using System.Globalization;
using System.Windows.Data;

namespace EducationalEventGenerator
{
    public class ProgressConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 3 &&
                values[0] != null && values[1] != null && values[2] != null &&
                double.TryParse(values[0].ToString(), out double value) &&
                double.TryParse(values[1].ToString(), out double maximum) &&
                double.TryParse(values[2].ToString(), out double width))
            {
                return (value / maximum) * width;
            }
            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}