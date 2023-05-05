using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;

namespace TravelAgency.Domain.RepositoryInterfaces
{
    public interface IAccommodationGuestRatingRepository
    {
        public List<AccommodationGuestRating> GetAll();
        public AccommodationGuestRating Save(AccommodationGuestRating rating);
        public int NextId();
        public List<AccommodationGuestRating> GetByOwner(User owner);
        public void LinkReservations(List<AccommodationReservation> reservations);
        public int GetRatingsCountByOwner(User owner);
    }
}
