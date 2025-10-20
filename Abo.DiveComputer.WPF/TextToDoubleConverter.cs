using System.Globalization;
using System.Windows.Data;

namespace RealWorldPlotter;

public class TextToDoubleConverter : IValueConverter
{
    // Conversion de double vers string (vue)
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double d)
            return d.ToString("G", culture); // Format général
        return string.Empty;
    }

    // Conversion de string vers double (modèle)
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string input = value as string;

        if (double.TryParse(input, NumberStyles.Any, culture, out double result))
            return result;

        // Retourne DependencyProperty.UnsetValue pour ignorer la mise à jour du binding
        return System.Windows.DependencyProperty.UnsetValue;
    }
}


public class TextToPercentageConverter : IValueConverter
{
    // Conversion de double vers string (vue)
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int d && d >= 0 && d <= 100)
            return d.ToString("N0", culture); // Format général
        return string.Empty;
    }

    // Conversion de string vers double (modèle)
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string input = value as string;

        if (int.TryParse(input, NumberStyles.Any, culture, out int result))
            return result;

        // Retourne DependencyProperty.UnsetValue pour ignorer la mise à jour du binding
        return System.Windows.DependencyProperty.UnsetValue;
    }
}