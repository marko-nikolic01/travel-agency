using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TravelAgency.Domain.Models;

namespace TravelAgency.Converters
{
    public class MoveRequestStatusToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch ((AccommodationReservationMoveRequestStatus)value)
            {
                case AccommodationReservationMoveRequestStatus.WAITING:
                    return "Na čekanju";
                case AccommodationReservationMoveRequestStatus.ACCEPTED:
                    return "Odobreno";
                case AccommodationReservationMoveRequestStatus.REJECTED:
                    return "Odbijeno";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

