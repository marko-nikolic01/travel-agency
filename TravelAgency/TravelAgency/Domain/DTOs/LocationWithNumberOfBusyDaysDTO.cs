using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;

namespace TravelAgency.Domain.DTOs
{
    public class LocationWithNumberOfBusyDaysDTO
    {
        public Location Location { get; set; }
        public int NumberOfBusyDays { get; set; }

        public LocationWithNumberOfBusyDaysDTO()
        {
            NumberOfBusyDays = 0;
            Location = null;
        }

        public LocationWithNumberOfBusyDaysDTO(Location location, int numberOfBusyDays)
        {
            Location = location;
            NumberOfBusyDays = numberOfBusyDays;
        }
    }
}
