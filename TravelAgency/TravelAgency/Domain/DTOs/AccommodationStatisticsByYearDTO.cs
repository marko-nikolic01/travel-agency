using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;

namespace TravelAgency.Domain.DTOs
{
    public class AccommodationStatisticsByYearDTO
    {
        public Accommodation? Accommodation { get; set; }
        public int Year { get; set; }
        public int NumberOfReservations { get; set; }
        public int NumberOfCancellations { get; set; }
        public int NumberOfMovings { get; set; }
        public int NumberOfRenovationSuggestions { get; set; }
        public int NumberOfBusyDays { get; set; }
        public List<AccommodationStatisticsByYearAndMonthDTO>? StatisticsByMonths { get; set; }

        public AccommodationStatisticsByYearDTO()
        {
            Accommodation = null;
            Year = 0;
            NumberOfReservations = 0;
            NumberOfCancellations = 0;
            NumberOfMovings = 0;
            NumberOfRenovationSuggestions = 0;
            NumberOfBusyDays = 0;

            StatisticsByMonths = new List<AccommodationStatisticsByYearAndMonthDTO>();
        }
    }
}
