using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Serializer;

namespace TravelAgency.Domain.RepositoryInterfaces
{
    public interface ITourRequestRepository
    {

        public List<TourRequest> GetAll();

        public TourRequest Save(TourRequest tourRequest);
    }
}
