using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;

namespace TravelAgency.Services
{
    public class AccommodationGuestRatingService
    {
        public IAccommodationGuestRatingRepository RatingRepository { get; set; }
        public IAccommodationReservationRepository ReservationRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
        public IAccommodationRepository AccommodationRepository { get; set; }
        public ILocationRepository LocationRepository { get; set; }

        public AccommodationGuestRatingService()
        {
            RatingRepository = Injector.Injector.CreateInstance<IAccommodationGuestRatingRepository>();
            ReservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
            UserRepository = Injector.Injector.CreateInstance<IUserRepository>();
            AccommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
            LocationRepository = Injector.Injector.CreateInstance<ILocationRepository>();

            AccommodationRepository.LinkLocations(LocationRepository.GetAll());
            AccommodationRepository.LinkOwners(UserRepository.GetOwners());
            ReservationRepository.LinkGuests(UserRepository.GetUsers());
            ReservationRepository.LinkAccommodations(AccommodationRepository.GetAll());
            RatingRepository.LinkReservations(ReservationRepository.GetAll());
        }

        public List<AccommodationGuestRating> GetAllRatings()
        {
            return RatingRepository.GetAll();
        }

        public void CreateNew(AccommodationGuestRating rating)
        {
            RatingRepository.Save(rating);
        }

        public List<AccommodationGuestRating> GetByOwner(User owner)
        {
            return RatingRepository.GetByOwner(owner);
        }

        public List<AccommodationReservation> GetUnratedReservations()
        {
            List<AccommodationReservation> unrated = new();

            foreach (var accommodationReservation in ReservationRepository.GetAll())
            {
                if (IsValidForRating(accommodationReservation, RatingRepository.GetAll()))
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
                if (accommodationReservation.Id == accommodationGuestRating.AccommodationReservationId)
                {
                    return true;
                }
            }

            return false;
        }

        public int GetRatingsCountByOwner(User owner)
        {
            return RatingRepository.GetRatingsCountByOwner(owner);
        }
    }
}
