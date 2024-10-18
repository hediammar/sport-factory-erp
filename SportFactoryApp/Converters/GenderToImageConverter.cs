using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace SportFactoryApp.Converters
{
    public class GenderToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string gender)
            {
                switch (gender.ToLower())
                {
                    case "homme":
                        return new BitmapImage(new Uri("pack://application:,,,/Images/maleProfile.jpg"));
                    case "femme":
                        return new BitmapImage(new Uri("pack://application:,,,/Images/femaleProfile.png"));
                    default:
                        return null; // You can return a default image if needed
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
