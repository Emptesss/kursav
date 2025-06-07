
using System;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;

namespace EducationalEventGenerator
{
    public class AchievementBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isUnlocked = (bool)value;
            return isUnlocked ? new SolidColorBrush(Colors.LightGreen) : new SolidColorBrush(Colors.LightGray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
