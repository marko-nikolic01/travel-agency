using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;

namespace TravelAgency.Domain.RepositoryInterfaces
{
    public interface IAccommodationOwnerRatingRepository
    {
        public List<AccommodationOwnerRating> GetAll();

        public List<AccommodationOwnerRating> GetByOwner(User owner);

        public List<AccommodationOwnerRating> GetRatingsVisibleToOwner(User owner, IEnumerable<AccommodationGuestRating> guestRatings);

        public int NextId();

        public AccommodationOwnerRating Save(AccommodationOwnerRating rating);

        public void SaveAll();

        public void LinkReservations(List<AccommodationReservation> reservations);

        public void LinkRenovationRecommendations(List<RenovationRecommendation> recommendations);

        public List<AccommodationOwnerRating> GetByAccommodation(Accommodation accommodation);
    }
}
