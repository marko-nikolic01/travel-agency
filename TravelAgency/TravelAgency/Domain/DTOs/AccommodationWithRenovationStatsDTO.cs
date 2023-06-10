using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;

namespace TravelAgency.Domain.DTOs
{
    public class AccommodationWithRenovationStatsDTO
    {
        public Accommodation Accommodation { get; set; }
        public int ScheduledRenovationsCount { get; set; }
        public int RenovationDaysCount { get; set; }
        public List<AccommodationRenovation> Renovations { get; set; }

        public AccommodationWithRenovationStatsDTO()
        {
            
        }

        public AccommodationWithRenovationStatsDTO(Accommodation accommodation, int scheduledRenovationsCount, int renovationDaysCount, List<AccommodationRenovation> renovations)
        {
            Accommodation = accommodation;
            ScheduledRenovationsCount = scheduledRenovationsCount;
            RenovationDaysCount = renovationDaysCount;
            Renovations = renovations;
        }
    }
}
