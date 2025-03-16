using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace CatGame.Helpers
{
    public class MultipleBooleansToBlurConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null) return 0.0;

            foreach (object value in values)
            {
                if (value == DependencyProperty.UnsetValue || value == null)
                    continue;

                try
                {
                    if ((bool)value)
                        return 10.0;
                }
                catch
                {
                    continue;
                }
            }
            return 0.0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}