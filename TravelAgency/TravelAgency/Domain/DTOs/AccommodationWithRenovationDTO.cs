using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;

namespace TravelAgency.Domain.DTOs
{
    public class AccommodationWithRenovationDTO
    {
        public Accommodation Accommodation { get; set; }
        public bool IsRenovatedInTheLastYear { get; set; }

        public AccommodationWithRenovationDTO(Accommodation accommodation, bool isRenovatedInTheLastYear)
        {
            Accommodation = accommodation;
            IsRenovatedInTheLastYear = isRenovatedInTheLastYear;
        }
    }
}
