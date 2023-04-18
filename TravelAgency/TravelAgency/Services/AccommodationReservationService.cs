using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.RepositoryInterfaces;

namespace TravelAgency.Services
{
    public class AccommodationReservationService
    {
        public IAccommodationReservationRepository ReservationRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
        public IAccommodationRepository AccommodationRepository { get; set; }
        public ILocationRepository LocationRepository { get; set; }
        public IAccommodationGuestRatingRepository GuestRatingRepository { get; set; }

        public AccommodationReservationService()
        {
            ReservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
            UserRepository = Injector.Injector.CreateInstance<IUserRepository>();
            AccommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
            LocationRepository = Injector.Injector.CreateInstance<ILocationRepository>();
            GuestRatingRepository = Injector.Injector.CreateInstance<IAccommodationGuestRatingRepository>();

            AccommodationRepository.LinkLocations(LocationRepository.GetAll());
            AccommodationRepository.LinkOwners(UserRepository.GetOwners());
            ReservationRepository.LinkGuests(UserRepository.GetUsers());
            ReservationRepository.LinkAccommodations(AccommodationRepository.GetAll());
            GuestRatingRepository.LinkReservations(ReservationRepository.GetAll());
        }

        public List<AccommodationReservation> GetByGuest(User guest)
        {
            return ReservationRepository.GetNotCanceledByGuest(guest);
        }

        public bool CreateReservation(AccommodationReservation reservation)
        {
            if (reservation.IsValid)
            {
                ReservationRepository.Save(reservation);
                return true;
            }

            return false;
        }

        public bool CancelReservation(AccommodationReservation reservation)
        {
            if (!IsDeadlineOverdue(reservation))
            {
                ReservationRepository.CancelReservation(reservation);
                return true;
            }
            return false;
        }

        private bool IsDeadlineOverdue(AccommodationReservation reservation)
        {
            DateOnly deadline = reservation.DateSpan.StartDate.AddDays(-reservation.Accommodation.DaysToCancel);
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            if (deadline.CompareTo(today) >= 0)
            {
                return false;
            }
            return true;
        }
    }
}
