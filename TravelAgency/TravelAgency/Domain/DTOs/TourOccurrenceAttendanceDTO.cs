using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Domain.DTOs
{
    public class TourOccurrenceAttendanceDTO
    {
        public string TourName { get; set; }
        public string  Status { get; set; }
        public string ArrivalKeyPoint { get; set; }
        public DateTime TourDateTime { get; set; }

        public TourOccurrenceAttendanceDTO(string tourName, string status, string arrivalKeyPoint, DateTime tourDateTime)
        {
            TourName = tourName;
            Status = status;
            ArrivalKeyPoint = arrivalKeyPoint;
            TourDateTime = tourDateTime;
        }
    }
}
