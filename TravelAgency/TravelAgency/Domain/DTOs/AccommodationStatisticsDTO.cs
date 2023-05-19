using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;

namespace TravelAgency.Domain.DTOs
{
    public class AccommodationStatisticsDTO
    {
        public Accommodation Accommodation { get; set; }
        public List<AccommodationStatisticsByYearDTO> StatisticsByYear { get; set; }

        public AccommodationStatisticsDTO(Accommodation accommodation, List<AccommodationStatisticsByYearDTO> statisticsByYear)
        {
            Accommodation = accommodation;
            StatisticsByYear = statisticsByYear;
        }

        public AccommodationStatisticsDTO(Accommodation accommodation)
        {
            Accommodation = accommodation;
            StatisticsByYear = new List<AccommodationStatisticsByYearDTO>();
        }
    }
}
