using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;

namespace TravelAgency.Domain.RepositoryInterfaces
{
    public interface IAccommodationRenovationRepository
    {
        public List<AccommodationRenovation> GetAll();
        public AccommodationRenovation Save(AccommodationRenovation accommodationRenovation);
        public int NextId();
        public List<AccommodationRenovation> GetByAccommodation(Accommodation accommodation);
        public List<AccommodationRenovation> GetByAccommodationId(int id);
        public void LinkAccommodations(List<Accommodation> accommodations);
        public List<AccommodationRenovation> GetByOwner(User owner);
        public void Delete(AccommodationRenovation renovation);
    }
}
