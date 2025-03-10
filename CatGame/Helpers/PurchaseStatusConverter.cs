using System.Globalization;
using System.Windows.Data;

namespace CatGame.Helpers
{
    public class PurchaseStatusConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType,
                            object parameter, CultureInfo culture)
        {
            if (values.Length == 2 &&
                values[0] is bool isPurchased &&
                values[1] is int price)
            {
                return isPurchased ? "Куплено" : $"Купить ";
            }
            return "Купить";
        }

        public object[] ConvertBack(object value, Type[] targetTypes,
                                  object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}