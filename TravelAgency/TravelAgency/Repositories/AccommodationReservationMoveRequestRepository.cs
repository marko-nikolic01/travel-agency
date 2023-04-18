using System;
using System.Collections.Generic;
using System.Linq;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Serializer;

namespace TravelAgency.Repositories
{
    public class AccommodationReservationMoveRequestRepository : IAccommodationReservationMoveRequestRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationReservationMoveRequests.csv";
        private readonly Serializer<AccommodationReservationMoveRequest> _serializer;
        private List<AccommodationReservationMoveRequest> _moveRequests;

        public AccommodationReservationMoveRequestRepository()
        {
            _serializer = new Serializer<AccommodationReservationMoveRequest>();
            _moveRequests = _serializer.FromCSV(FilePath);
        }

        public void LinkReservations(List<AccommodationReservation> reservations)
        {
            foreach (AccommodationReservationMoveRequest moveRequest in _moveRequests)
            {
                foreach (AccommodationReservation reservation in reservations)
                {
                    if (moveRequest.ReservationId == reservation.Id)
                    {
                        moveRequest.Reservation = reservation;
                        break;
                    }
                }
            }
        }

        public List<AccommodationReservationMoveRequest> GetAll()
        {
            return _moveRequests;
        }

        public AccommodationReservationMoveRequest GetById(int id)
        {
            foreach (AccommodationReservationMoveRequest moveRequest in _moveRequests)
            {
                if (moveRequest.Id == id)
                {
                    return moveRequest; ;
                }
            }
            return null;
        }

        public List<AccommodationReservationMoveRequest> GetAllByGuest(User guest)
        {
            List<AccommodationReservationMoveRequest> moveRequests = new List<AccommodationReservationMoveRequest>();
            foreach (AccommodationReservationMoveRequest moveRequest in _moveRequests)
            {
                if (moveRequest.Reservation.Guest.Id == guest.Id)
                {
                    moveRequests.Add(moveRequest);
                }
            }
            return moveRequests;
        }

        public List<AccommodationReservationMoveRequest> GetWaitingByOwner(User owner)
        {
            return _moveRequests.FindAll(mr => mr.Reservation.Accommodation.OwnerId == owner.Id && mr.Status == AccommodationReservationMoveRequestStatus.WAITING);
        }

        public int NextId()
        {
            if (_moveRequests.Count < 1)
            {
                return 1;
            }
            return _moveRequests.Max(c => c.Id) + 1;
        }

        public AccommodationReservationMoveRequest Save(AccommodationReservationMoveRequest moveRequest)
        {
            moveRequest.Id = NextId();
            _moveRequests.Add(moveRequest);
            _serializer.ToCSV(FilePath, _moveRequests);
            return moveRequest;
        }

        public void UpdateStatus(AccommodationReservationMoveRequest moveRequest, AccommodationReservationMoveRequestStatus status)
        {
            moveRequest.Status = status;
            _serializer.ToCSV(FilePath, _moveRequests);
        }

        public void UpdateStatusChangedFlag(AccommodationReservationMoveRequest moveRequest, bool status)
        {
            moveRequest.StatusChanged = status;
            _serializer.ToCSV(FilePath, _moveRequests);
        }
    }
}
