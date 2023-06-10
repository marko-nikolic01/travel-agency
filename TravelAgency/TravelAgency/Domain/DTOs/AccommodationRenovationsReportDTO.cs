using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;

namespace TravelAgency.Domain.DTOs
{
    public class AccommodationRenovationsReportDTO
    {
        public string Header { get; set; }
        public DateOnly ReportDate { get; set; }
        public User Owner { get; set; }
        public DateSpan ReportDateSpan { get; set; }
        public int RenovationDaysCount { get; set; }
        public int RenovationsCount { get; set; }
        public List<AccommodationWithRenovationStatsDTO> AccommodationStats { get; set; }

        public AccommodationRenovationsReportDTO()
        {
            
        }
    }
}
