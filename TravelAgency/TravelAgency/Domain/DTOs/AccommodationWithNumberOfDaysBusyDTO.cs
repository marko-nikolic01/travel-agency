using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;

namespace TravelAgency.Domain.DTOs
{
    public class AccommodationWithNumberOfDaysBusyDTO
    {
        public Accommodation Accommodation { get; set; }
        public int NumberOfBusyDays { get; set; }

        public AccommodationWithNumberOfDaysBusyDTO()
        {
            
        }

        public AccommodationWithNumberOfDaysBusyDTO(Accommodation accommodation, int numberOfBusyDays)
        {
            Accommodation = accommodation;
            NumberOfBusyDays = numberOfBusyDays;
        }
    }
}
