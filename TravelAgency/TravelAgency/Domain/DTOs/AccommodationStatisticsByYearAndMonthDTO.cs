using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelAgency.Domain.Models;

namespace TravelAgency.Domain.DTOs
{

    public class AccommodationStatisticsByYearAndMonthDTO
    {
        public Accommodation Accommodation { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int NumberOfReservations { get; set; }
        public int NumberOfCancellations { get; set; }
        public int NumberOfMovings { get; set; }
        public int NumberOfRenovationSuggestions { get; set; }
        public int NumberOfBusyDays { get; set; }

        public AccommodationStatisticsByYearAndMonthDTO(Accommodation accommodation, int year, int month, int numberOfReservations, int numberOfCancellations, int numberOfReservationMovings, int numberOfRenovationSuggestions, int busyDays)
        {
            Accommodation = accommodation;
            Year = year;
            Month = month;
            NumberOfReservations = numberOfReservations;
            NumberOfCancellations = numberOfCancellations;
            NumberOfMovings = numberOfReservationMovings;
            NumberOfRenovationSuggestions = numberOfRenovationSuggestions;
            NumberOfBusyDays = busyDays;
        }
    }
}
