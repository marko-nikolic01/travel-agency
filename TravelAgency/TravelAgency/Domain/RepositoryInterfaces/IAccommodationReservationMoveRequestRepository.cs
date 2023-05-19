using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;

namespace TravelAgency.Domain.RepositoryInterfaces
{
    public interface IAccommodationReservationMoveRequestRepository
    {
        public void LinkReservations(List<AccommodationReservation> reservations);
        public List<AccommodationReservationMoveRequest> GetAll();

        public AccommodationReservationMoveRequest GetById(int id);

        public List<AccommodationReservationMoveRequest> GetAllByGuest(User guest);

        public List<AccommodationReservationMoveRequest> GetWaitingByOwner(User owner);

        public int NextId();

        public AccommodationReservationMoveRequest Save(AccommodationReservationMoveRequest moveRequest);

        public void UpdateStatus(AccommodationReservationMoveRequest moveRequest, AccommodationReservationMoveRequestStatus status);

        public void UpdateStatusChangedFlag(AccommodationReservationMoveRequest moveRequest, bool status);

        public List<AccommodationReservationMoveRequest> GetByAccommodation(Accommodation accommodation);
    }
}
