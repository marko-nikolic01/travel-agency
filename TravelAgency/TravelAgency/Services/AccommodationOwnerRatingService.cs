using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;

namespace TravelAgency.Services
{
    public class AccommodationOwnerRatingService
    {
        public IAccommodationOwnerRatingRepository OwnerRatingRepository { get; set; }
        public IAccommodationGuestRatingRepository GuestRatingRepository { get; set; }
        public IAccommodationRatingPhotoRepository RatingPhotoRepository { get; set; }
        public IAccommodationReservationRepository ReservationRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
        public IAccommodationRepository AccommodationRepository { get; set; }
        public IAccommodationPhotoRepository AccommodationPhotoRepository { get; set; }
        public ILocationRepository LocationRepository { get; set; }

        public AccommodationOwnerRatingService()
        {
            OwnerRatingRepository = Injector.Injector.CreateInstance<IAccommodationOwnerRatingRepository>();
            GuestRatingRepository = Injector.Injector.CreateInstance<IAccommodationGuestRatingRepository>();
            ReservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
            UserRepository = Injector.Injector.CreateInstance<IUserRepository>();
            AccommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
            AccommodationPhotoRepository = Injector.Injector.CreateInstance<IAccommodationPhotoRepository>();
            LocationRepository = Injector.Injector.CreateInstance<ILocationRepository>();
            RatingPhotoRepository = Injector.Injector.CreateInstance<IAccommodationRatingPhotoRepository>();
            AccommodationRepository.LinkLocations(LocationRepository.GetAll());
            AccommodationRepository.LinkOwners(UserRepository.GetOwners());
            AccommodationRepository.LinkPhotos(AccommodationPhotoRepository.GetAll());
            ReservationRepository.LinkGuests(UserRepository.GetUsers());
            ReservationRepository.LinkAccommodations(AccommodationRepository.GetAll());
            OwnerRatingRepository.LinkReservations(ReservationRepository.GetAll());
            GuestRatingRepository.LinkReservations(ReservationRepository.GetAll());

        }

        public bool CreateRating(AccommodationOwnerRating rating)
        {
            if (rating.IsValid)
            {
                OwnerRatingRepository.Save(rating);
                SavePhotos(rating);
                return true;
            }
            return false;
        }

        private void SavePhotos(AccommodationOwnerRating rating)
        {
            foreach (AccommodationRatingPhoto photo in rating.Photos)
            {
                photo.RatingId = rating.Id;
            }
            RatingPhotoRepository.SaveAll(rating.Photos);
        }

        public List<AccommodationReservation> GetUnratedReservationsByGuest(User guest)
        {
            List<AccommodationReservation> unratedByGuest = new List<AccommodationReservation>();
            List<AccommodationReservation> reservationsByGuest = ReservationRepository.GetAllNotCanceledByGuest(guest);
            List<AccommodationOwnerRating> ratings = OwnerRatingRepository.GetAll();

            foreach (AccommodationReservation reservation in reservationsByGuest)
            {
                if (CanBeRated(reservation, ratings))
                {
                    unratedByGuest.Add(reservation);
                }
            }
            return unratedByGuest;
        }

        private bool CanBeRated(AccommodationReservation reservation, List<AccommodationOwnerRating> ratings)
        {
            return !IsReservationActive(reservation) &&
                CalculateDaysLeftForRating(reservation) >= 0 &&
                !IsRated(reservation, ratings);
        }

        public int CalculateDaysLeftForRating(AccommodationReservation accommodationReservation)
        {
            return 5 - (DateOnly.FromDateTime(DateTime.Now).DayNumber - accommodationReservation.DateSpan.EndDate.DayNumber);
        }

        public bool IsReservationActive(AccommodationReservation accommodationReservation)
        {
            return DateOnly.FromDateTime(DateTime.Now).CompareTo(accommodationReservation.DateSpan.EndDate) <= 0;
        }

        private bool IsRated(AccommodationReservation reservation, List<AccommodationOwnerRating> ratings)
        {
            foreach (AccommodationOwnerRating rating in ratings)
            {
                if (reservation.Id == rating.AccommodationReservationId)
                {
                    return true;
                }
            }
            return false;
        }

        public List<AccommodationOwnerRating> GetRatingsVisibleToOwner(User owner)
        {
            List<AccommodationOwnerRating> ownerRatings = new List<AccommodationOwnerRating>();

            foreach (var guestRating in GuestRatingRepository.GetAll())
            {
                foreach (var ownerRating in OwnerRatingRepository.GetAll())
                {
                    if (ownerRating.AccommodationReservationId == guestRating.AccommodationReservationId && owner.Id == ownerRating.AccommodationReservation.Accommodation.OwnerId)
                    {
                        ownerRatings.Add(ownerRating);
                        break;
                    }
                }
            }

            return ownerRatings;
        }
    }
}
