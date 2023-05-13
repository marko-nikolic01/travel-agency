using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;

namespace TravelAgency.Domain.DTOs
{
    public class AccommodationReservationMoveRequestWithAvailabilityDTO
    {
        public AccommodationReservationMoveRequest MoveRequest { get; set; }
        public bool IsNewDateSpanAvailable { get; set; }

        public AccommodationReservationMoveRequestWithAvailabilityDTO(AccommodationReservationMoveRequest moveRequest, bool isNewDateSpanAvailable)
        {
            MoveRequest = moveRequest;
            IsNewDateSpanAvailable = isNewDateSpanAvailable;
        }
    }
}
