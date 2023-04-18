using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;

namespace TravelAgency.RepositoryInterfaces
{
    public interface IAccommodationGuestRatingRepository
    {
        public List<AccommodationGuestRating> GetAll();
        public AccommodationGuestRating Save(AccommodationGuestRating rating);
        public int NextId();
        public List<AccommodationGuestRating> GetByOwner(User owner);
        public void LinkReservations(List<AccommodationReservation> reservations);
    }
}
