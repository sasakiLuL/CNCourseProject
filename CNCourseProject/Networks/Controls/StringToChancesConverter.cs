using System.Globalization;
using System.Windows.Data;

namespace ComputerNetworksCourseWork.Networks.Controls;

[ValueConversion(typeof(HashSet<double>), typeof(string))]
public class StringToChancesConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var hashSet = (HashSet<double>)value;

        return string.Join(' ', hashSet.Select(e => e.ToString()));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var valueToConvert = (string)value;

        var hashset = new HashSet<double>();

        foreach (var val in valueToConvert.Split(',', ' '))
        {
            if (double.TryParse(val, out var result))
            {
                hashset.Add(result);
            }
        }

        if (!hashset.Any())
        {
            hashset = [0.01, 0.02, 0.03];
        }

        return hashset;
    }
}
