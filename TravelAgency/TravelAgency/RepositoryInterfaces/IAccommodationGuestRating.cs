using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;

namespace TravelAgency.RepositoryInterfaces
{
    public interface IAccommodationGuestRating
    {
        public List<AccommodationGuestRating> GetAll();
        public AccommodationGuestRating Save(AccommodationGuestRating rating);
        public AccommodationGuestRating GetById(int id);
        public int NextId();
        public List<AccommodationGuestRating> GetByOwner(User owner);
    }
}
