using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Observer;
using TravelAgency.Serializer;

namespace TravelAgency.Domain.RepositoryInterfaces
{
    public interface ITourRequestRepository
    {
        public List<TourRequest> GetAll();
        public void NotifyObservers();
        public void Subscribe(IObserver observer);
        public void Unsubscribe(IObserver observer);
        public TourRequest Save(TourRequest tourRequest);
        public void UpdateRequestStatus(TourRequest request);
        public List<TourRequest> GetRequestsByGuestId(int id);
        public int NextId();
        
    }
}
