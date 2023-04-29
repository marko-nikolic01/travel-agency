using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;

namespace TravelAgency.Domain.RepositoryInterfaces
{
    public interface ITourRequestRepository
    {
        public List<TourRequest> GetAll();
        public List<TourRequest> GetRequestsByGuestId(int id);
        public int NextId();
        public TourRequest Save(TourRequest tourRequest);
    }
}
