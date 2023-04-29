using System.Collections.Generic;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Observer;
using TravelAgency.Serializer;

namespace TravelAgency.Repositories
{
    public class TourRequestRepository : ITourRequestRepository
    {
        private const string FilePath = "../../../Resources/Data/tourRequests.csv";
        private readonly Serializer<TourRequest> _serializer;
        private List<TourRequest> tourRequests;

        public TourRequestRepository()
        {
            _serializer = new Serializer<TourRequest>();
            tourRequests = _serializer.FromCSV(FilePath);
        }
        public int NextId()
        {
            if (tourRequests.Count == 0)
            {
                return 1;
            }
            return tourRequests[tourRequests.Count - 1].Id + 1;
        }

        public List<TourRequest> GetAll()
        {
            return tourRequests;
        }

        public TourRequest Save(TourRequest tourRequest)
        {
            tourRequest.Id = NextId();
            tourRequests.Add(tourRequest);
            _serializer.ToCSV(FilePath, tourRequests);
            return tourRequest;

        }

        public List<TourRequest> GetRequestsByGuestId(int id)
        {
            List<TourRequest> result = new List<TourRequest>();
            foreach (TourRequest tourRequest in tourRequests)
            {
                if (tourRequest.GuestId == id)
                {
                    result.Add(tourRequest);
                }
            }
            return result;
        }
    }
}