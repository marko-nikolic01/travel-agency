using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;

namespace TravelAgency.RepositoryInterfaces
{
    public interface IAccommodationReservationRepository
    {
        public void LinkAccommodations(List<Accommodation> accommodations);
        public void LinkGuests(List<User> guests);
        public List<AccommodationReservation> GetAll();
        public void Delete(AccommodationReservation reservation);
        public List<AccommodationReservation> GetNotCanceledByGuest(User guest);
        public int NextId();
        public AccommodationReservation Save(AccommodationReservation reservation);
        public bool IsActive(AccommodationReservation reservation);
        public List<AccommodationReservation> GetByAccommodation(Accommodation accommodation);
    }
}
