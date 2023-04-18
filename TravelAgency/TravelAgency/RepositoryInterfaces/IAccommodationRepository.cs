using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;

namespace TravelAgency.RepositoryInterfaces
{
    public interface IAccommodationRepository
    {
        public List<Accommodation> GetAll();
        public Accommodation Save(Accommodation accommodation);
        public List<Accommodation> GetFiltered(AccommodationSearchFilter filter);
        public int NextId();
        public List<Accommodation> GetByOwner(User owner);
        public void LinkOwners(List<User> owners);
        public void LinkLocations(List<Location> locations);
        public void LinkPhotos(List<AccommodationPhoto> photos);
    }
}
