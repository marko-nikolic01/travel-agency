using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using TravelAgency.Domain.Models;

namespace TravelAgency.Converters
{
    public class DaysLeftForRatingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            AccommodationReservation reservation = (AccommodationReservation)value;
            int daysLeft = 6 - (DateOnly.FromDateTime(DateTime.Now).DayNumber - reservation.DateSpan.EndDate.DayNumber);
            return daysLeft;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DaysLeftForRatingToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            AccommodationReservation reservation = (AccommodationReservation)value;
            int daysLeft = 6 - (DateOnly.FromDateTime(DateTime.Now).DayNumber - reservation.DateSpan.EndDate.DayNumber);
            if (daysLeft > 1)
            {
                return Visibility.Hidden;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
