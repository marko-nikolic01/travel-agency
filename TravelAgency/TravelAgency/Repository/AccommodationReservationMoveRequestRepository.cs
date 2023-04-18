using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Serializer;

namespace TravelAgency.Repository
{
    public class AccommodationReservationMoveRequestRepository : IRepository<AccommodationReservationMoveRequest>
    {
        private const string FilePath = "../../../Resources/Data/accommodationReservationMoveRequests.csv";
        private readonly Serializer<AccommodationReservationMoveRequest> _serializer;
        private List<AccommodationReservationMoveRequest> _moveRequests;

        public AccommodationReservationMoveRequestRepository(AccommodationReservationRepository accommodationReservationRepository)
        {
            _serializer = new Serializer<AccommodationReservationMoveRequest>();

            _moveRequests = _serializer.FromCSV(FilePath);

            foreach (AccommodationReservationMoveRequest moveRequest in _moveRequests)
            {
                foreach (AccommodationReservation reservation in accommodationReservationRepository.GetAll())
                {
                    if (moveRequest.ReservationId == reservation.Id)
                    {
                        moveRequest.Reservation = reservation;
                    }
                }
            }
        }

        public void Delete(AccommodationReservationMoveRequest entity)
        {
            DeleteById(entity.Id);
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            AccommodationReservationMoveRequest moveRequest = _moveRequests.Find(mr => mr.Id == id);
            _moveRequests.Remove(moveRequest);
            _serializer.ToCSV(FilePath, _moveRequests);
        }

        public List<AccommodationReservationMoveRequest> GetAll()
        {
            return _moveRequests;
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

        public int NextId()
        {
            _moveRequests = _serializer.FromCSV(FilePath);
            if (_moveRequests.Count < 1)
            {
                return 1;
            }
            return _moveRequests.Max(c => c.Id) + 1;
        }

        public AccommodationReservationMoveRequest Save(AccommodationReservationMoveRequest moveRequest)
        {
            moveRequest.Id = NextId();
            _moveRequests = _serializer.FromCSV(FilePath);
            _moveRequests.Add(moveRequest);
            _serializer.ToCSV(FilePath, _moveRequests);
            return moveRequest;
        }

        public void SaveAll(IEnumerable<AccommodationReservationMoveRequest> entities)
        {
            throw new NotImplementedException();
        }

        public bool NotifyStatusChange()
        {
            bool notify = false;
            foreach(AccommodationReservationMoveRequest moveRequest in _moveRequests) 
            {
                if (StatusChanged(moveRequest))
                {
                    notify = true;
                }
            }
            return notify;
        }

        private bool StatusChanged(AccommodationReservationMoveRequest moveRequest)
        {
            if (!moveRequest.StatusChanged)
            {
                return false;
            }

            moveRequest.StatusChanged = false;
            return true;
        }

        public List<AccommodationReservationMoveRequest> GetWaitingByOwner(User owner)
        {
            return _moveRequests.FindAll(mr => mr.Reservation.Accommodation.OwnerId == owner.Id && mr.Status == AccommodationReservationMoveRequestStatus.WAITING);
        }
    }
}
