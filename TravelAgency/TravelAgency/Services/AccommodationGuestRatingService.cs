using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Repositories;

namespace TravelAgency.Services
{
    public class AccommodationGuestRatingService
    {
        public IAccommodationGuestRatingRepository GuestRatingRepository { get; set; }
        public IAccommodationOwnerRatingRepository AccommodationOwnerRatingRepository { get; set; }
        public IAccommodationReservationRepository ReservationRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
        public IAccommodationRepository AccommodationRepository { get; set; }
        public ILocationRepository LocationRepository { get; set; }

        public AccommodationGuestRatingService()
        {
            GuestRatingRepository = Injector.Injector.CreateInstance<IAccommodationGuestRatingRepository>();
            AccommodationOwnerRatingRepository = Injector.Injector.CreateInstance<IAccommodationOwnerRatingRepository>();
            ReservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
            UserRepository = Injector.Injector.CreateInstance<IUserRepository>();
            AccommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
            LocationRepository = Injector.Injector.CreateInstance<ILocationRepository>();

            AccommodationRepository.LinkLocations(LocationRepository.GetAll());
            AccommodationRepository.LinkOwners(UserRepository.GetOwners());
            ReservationRepository.LinkGuests(UserRepository.GetUsers());
            ReservationRepository.LinkAccommodations(AccommodationRepository.GetActive());
            GuestRatingRepository.LinkReservations(ReservationRepository.GetAll());
            AccommodationOwnerRatingRepository.LinkReservations(ReservationRepository.GetAll());
        }

        public List<AccommodationGuestRating> GetAllRatings()
        {
            return GuestRatingRepository.GetAll();
        }

        public void CreateNew(AccommodationGuestRating rating)
        {
            GuestRatingRepository.Save(rating);
        }

        public List<AccommodationGuestRating> GetByOwner(User owner)
        {
            return GuestRatingRepository.GetByOwner(owner);
        }

        public List<AccommodationReservation> GetUnratedReservationsByOwner(User owner)
        {
            List<AccommodationReservation> unrated = new();

            foreach (var accommodationReservation in ReservationRepository.GetAll())
            {
                if (IsValidForRating(accommodationReservation, GuestRatingRepository.GetAll()) && accommodationReservation.Accommodation.Owner.Id == owner.Id)
                {
                    unrated.Add(accommodationReservation);
                    continue;
                }
            }

            return unrated;
        }

        private bool IsValidForRating(AccommodationReservation accommodationReservation, IEnumerable<AccommodationGuestRating> accommodationGuestRatings)
        {
            return CalculateDaysLeftForRating(accommodationReservation) >= 1 &&
                !IsActive(accommodationReservation) &&
                !IsRated(accommodationReservation, accommodationGuestRatings) &&
                !accommodationReservation.Canceled;
        }

        public int CalculateDaysLeftForRating(AccommodationReservation accommodationReservation)
        {
            return 5 - DateOnly.Parse(DateTime.Now.Date.ToShortDateString()).DayNumber + accommodationReservation.DateSpan.EndDate.DayNumber;
        }

        private bool IsActive(AccommodationReservation accommodationReservation)
        {
            return DateOnly.Parse(DateTime.Now.Date.ToShortDateString()).DayNumber - accommodationReservation.DateSpan.EndDate.DayNumber < 0;
        }

        private bool IsRated(AccommodationReservation accommodationReservation, IEnumerable<AccommodationGuestRating> accommodationGuestRatings)
        {
            foreach (var accommodationGuestRating in accommodationGuestRatings)
            {
                if (accommodationReservation.Id == accommodationGuestRating.AccommodationReservation.Id)
                {
                    return true;
                }
            }

            return false;
        }

        public int GetRatingsCountByOwner(User owner)
        {
            return GuestRatingRepository.GetRatingsCountByOwner(owner);
        }

        public List<AccommodationGuestRating> GetRatingsVisibleToGuest(User guest)
        {
            List<AccommodationGuestRating> guestRatings = new List<AccommodationGuestRating>();
            foreach (var ownerRating in AccommodationOwnerRatingRepository.GetAll())
            { 
                foreach (var guestRating in GuestRatingRepository.GetAll())
                {
                    if (guestRating.AccommodationReservation.Id == ownerRating.AccommodationReservation.Id && guest.Id == guestRating.AccommodationReservation.Guest.Id)
                    {
                        guestRatings.Add(guestRating);
                        break;
                    }
                }
            }
            return guestRatings;
        }
    }
}
