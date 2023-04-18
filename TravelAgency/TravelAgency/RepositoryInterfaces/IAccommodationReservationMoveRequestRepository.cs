using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;

namespace TravelAgency.RepositoryInterfaces
{
    internal interface IAccommodationReservationMoveRequestRepository
    {
        public List<AccommodationReservationMoveRequest> GetAll();

        public AccommodationReservationMoveRequest GetById(int id);

        public List<AccommodationReservationMoveRequest> GetAllByGuest(User guest);

        public List<AccommodationReservationMoveRequest> GetWaitingByOwner(User owner);

        public int NextId();

        public AccommodationReservationMoveRequest Save(AccommodationReservationMoveRequest moveRequest);
    }
}
