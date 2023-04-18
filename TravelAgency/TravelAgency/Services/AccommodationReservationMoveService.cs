using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.RepositoryInterfaces;

namespace TravelAgency.Services
{
    public class AccommodationReservationMoveService
    {
        public IAccommodationReservationMoveRequestRepository MoveRequestRepository { get; set; }
        public IAccommodationReservationRepository ReservationRepository { get; set; }
        public IUserRepository UserRepository { get; set; }

        public AccommodationReservationMoveService()
        {
            
        }


    }
}
