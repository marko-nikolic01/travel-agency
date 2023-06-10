using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TravelAgency.Domain.Models;

namespace TravelAgency.Converters
{
    public class PhotoListToImageSourceListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<AccommodationPhoto> accommodationPhotos = (ObservableCollection<AccommodationPhoto>)value;

            ObservableCollection<ImageSource> images = new ObservableCollection<ImageSource>();

            foreach (var photo in accommodationPhotos)
            {
                ImageSource image = new BitmapImage(new Uri(photo.Path, UriKind.RelativeOrAbsolute));
                images.Add(image);
            }

            return images;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
