using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using TravelAgency.Domain.Models;

namespace TravelAgency.Converters
{
    public class AccommodationPhotoToBitmapImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            AccommodationPhoto photo = (AccommodationPhoto)value;
            Uri uri = new Uri(photo.Path, UriKind.RelativeOrAbsolute);
            BitmapImage image = new BitmapImage(uri);
            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
