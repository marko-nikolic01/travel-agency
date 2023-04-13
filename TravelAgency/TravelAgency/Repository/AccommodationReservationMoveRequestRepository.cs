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

        public void Delete(AccommodationReservationMoveRequest entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
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
    }
}
