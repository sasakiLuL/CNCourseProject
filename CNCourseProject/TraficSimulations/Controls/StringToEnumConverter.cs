using System.Globalization;
using System.Windows.Data;

namespace ComputerNetworksCourseWork.TraficSimulations.Controls;

[ValueConversion(typeof(string), typeof(Protocol))]
public class StringToEnumConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((Protocol)value).ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var str = (string)value;

        if (Enum.TryParse<Protocol>(str, out var result))
        {
            return result;
        }

        return Protocol.TCP;
    }
}
