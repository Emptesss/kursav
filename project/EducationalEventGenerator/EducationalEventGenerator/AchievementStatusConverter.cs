using System;
using System.Windows.Data;
using System.Globalization;

namespace EducationalEventGenerator
{
    public class AchievementStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isUnlocked = (bool)value;
            return isUnlocked ? "Получено" : "Не получено";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}