using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Observer;
using TravelAgency.Serializer;

namespace TravelAgency.RepositoryInterfaces
{
    public interface ITourRepository
    {
        public List<Tour> GetAll();

        public Tour Save(Tour tour);
    }
}
