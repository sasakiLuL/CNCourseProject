using System.Globalization;
using System.Windows.Data;

namespace ComputerNetworksCourseWork.Networks.Controls;

[ValueConversion(typeof(HashSet<int>), typeof(string))]
public class StringToWeightConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var hashSet = (HashSet<int>)value;

        return string.Join(' ', hashSet.Select(e => e.ToString()));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var valueToConvert = (string)value;

        var hashset = new HashSet<int>();

        foreach (var val in valueToConvert.Split(',', ' ', '.'))
        {
            if (int.TryParse(val, out var result))
            {
                hashset.Add(result);
            }
        }

        if (!hashset.Any())
        {
            hashset = [2, 3, 5, 7, 10, 12, 15, 20, 21, 25];
        }

        return hashset;
    }
}
