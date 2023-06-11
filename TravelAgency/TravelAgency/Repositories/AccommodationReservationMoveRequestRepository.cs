using System;
using System.Collections.Generic;
using System.Linq;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Serializer;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            bool shouldSave = false;
            foreach (AccommodationReservationMoveRequest moveRequest in _moveRequests)
            {
                foreach (AccommodationReservation reservation in reservations)
                {
                    if (moveRequest.Reservation.Id == reservation.Id)
                    {
                        moveRequest.Reservation = reservation;
                        if (moveRequest.CheckExpiration())
                        {
                            shouldSave = true;
                        }
                        break;
                    }
                }
            }
            if (shouldSave)
            {
                _serializer.ToCSV(FilePath, _moveRequests);
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
                if (moveRequest.Id == id && !moveRequest.Reservation.Canceled)
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
                if (moveRequest.Reservation.Guest.Id == guest.Id && !moveRequest.Reservation.Canceled)
                {
                    moveRequests.Add(moveRequest);
                }
            }
            return moveRequests;
        }

        public List<AccommodationReservationMoveRequest> GetWaitingByOwner(User owner)
        {
            return _moveRequests.FindAll(mr => mr.Reservation.Accommodation.Owner.Id == owner.Id && mr.Status == AccommodationReservationMoveRequestStatus.WAITING && !mr.Reservation.Canceled);
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

        public List<AccommodationReservationMoveRequest> GetByAccommodation(Accommodation accommodation)
        {
            var filtered = new List<AccommodationReservationMoveRequest>();
            foreach (var moveRequest in _moveRequests)
            {
                if (moveRequest.Reservation.Accommodation == accommodation)
                {
                    filtered.Add(moveRequest);
                }
            }

            return filtered;
        }

        public List<AccommodationReservationMoveRequest> GetByReservation(AccommodationReservation reservation)
        {
            return _moveRequests.FindAll(mr => mr.Reservation.Id == reservation.Id);
        }
    }
}
