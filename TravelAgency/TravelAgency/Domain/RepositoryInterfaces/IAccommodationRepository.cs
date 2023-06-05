using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;

namespace TravelAgency.Domain.RepositoryInterfaces
{
    public interface IAccommodationRepository
    {
        public List<Accommodation> GetAll();
        public List<Accommodation> GetActive();
        public Accommodation Save(Accommodation accommodation);
        public void Delete(Accommodation accommodation);
        public int NextId();
        public List<Accommodation> GetActiveByOwner(User owner);
        public List<Accommodation> GetActiveByLocationAndOwner(Location location, User owner);
        public List<Accommodation> GetActiveByLocation(Location location);
        public void LinkOwners(List<User> owners);
        public void LinkLocations(List<Location> locations);
        public void LinkPhotos(List<AccommodationPhoto> photos);
    }
}
