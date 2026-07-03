namespace Orders.Management.UI;

public class EnumToBooleanConverter : System.Windows.Data.IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        return value?.ToString() == parameter?.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        return (bool)value ? Enum.Parse(targetType, parameter.ToString()) : System.Windows.DependencyProperty.UnsetValue;
    }
}