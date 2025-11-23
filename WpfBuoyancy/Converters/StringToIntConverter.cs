using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfBuoyancy
{
    public class DebugConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           // System.Diagnostics.Debugger.Break();
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Diagnostics.Debugger.Break();
            return value;
        }
    }
    public class StringToIntConverter : IValueConverter
    {
        public int Min { get; set; } = int.MinValue;
        public int Max { get; set; } = int.MaxValue;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Conversion de int → string (pour l’affichage)
            if (value == null)
                return string.Empty;
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? text = value as string;
            if (string.IsNullOrWhiteSpace(text))
                return Binding.DoNothing; // ou Min, selon ton besoin

            if (!int.TryParse(text, out int result))
                return Binding.DoNothing; // saisie invalide : on ignore la mise à jour

            // Clamp entre Min et Max
            if (result < Min)
                result = Min;
            if (result > Max)
                result = Max;

            return result;
        }
    }
}