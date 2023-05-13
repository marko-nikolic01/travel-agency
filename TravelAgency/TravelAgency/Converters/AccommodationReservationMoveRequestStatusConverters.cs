using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TravelAgency.Domain.Models;

namespace TravelAgency.Converters
{
    public class ReservationMoveRequestStatusToVisibilityConverter1 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            AccommodationReservationMoveRequestStatus status = (AccommodationReservationMoveRequestStatus)value;
            if (status == AccommodationReservationMoveRequestStatus.WAITING)
            {
                return Visibility.Visible;
            }
            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ReservationMoveRequestStatusToVisibilityConverter2 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            AccommodationReservationMoveRequestStatus status = (AccommodationReservationMoveRequestStatus)value;
            if (status == AccommodationReservationMoveRequestStatus.ACCEPTED)
            {
                return Visibility.Visible;
            }
            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ReservationMoveRequestStatusToVisibilityConverter3 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            AccommodationReservationMoveRequestStatus status = (AccommodationReservationMoveRequestStatus)value;
            if (status == AccommodationReservationMoveRequestStatus.REJECTED)
            {
                return Visibility.Visible;
            }
            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ReservationMoveRequestStatusToTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            AccommodationReservationMoveRequestStatus status = (AccommodationReservationMoveRequestStatus)value;
            switch (status)
            {
                case AccommodationReservationMoveRequestStatus.WAITING:
                    return Brushes.Black;
                case AccommodationReservationMoveRequestStatus.ACCEPTED:
                    return Brushes.Green;
                case AccommodationReservationMoveRequestStatus.REJECTED:
                    return Brushes.Red;
            }
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
