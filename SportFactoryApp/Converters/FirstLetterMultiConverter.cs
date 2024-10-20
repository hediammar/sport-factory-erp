using System;
using System.Globalization;
using System.Windows.Data;

namespace SportFactoryApp.Converters
{
    public class FirstLetterMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length >= 2)
            {
                string firstName = values[0]?.ToString();
                string lastName = values[1]?.ToString();
                return $"{firstName?[0]}{lastName?[0]}"; // Return the first letters
            }
            return string.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
