using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Injector;
using TravelAgency.Repositories;

namespace TravelAgency.Services
{
    public class SuperOwnerService
    {
        public IUserRepository UserRepository { get; set; }
        public ILocationRepository LocationRepository { get; set; }
        public IAccommodationPhotoRepository AccommodationPhotoRepository { get; set; }
        public IAccommodationRepository AccommodationRepository { get; set; }
        public IAccommodationReservationRepository AccommodationReservationRepository { get; set; }
        public IAccommodationOwnerRatingRepository AccommodationOwnerRatingRepository { get; set; }

        public SuperOwnerService()
        {
            UserRepository = Injector.Injector.CreateInstance<IUserRepository>();
            LocationRepository = Injector.Injector.CreateInstance<ILocationRepository>();
            AccommodationPhotoRepository = Injector.Injector.CreateInstance<IAccommodationPhotoRepository>();
            AccommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
            AccommodationReservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
            AccommodationOwnerRatingRepository = Injector.Injector.CreateInstance<IAccommodationOwnerRatingRepository>();

            AccommodationRepository.LinkLocations(LocationRepository.GetAll());
            AccommodationRepository.LinkPhotos(AccommodationPhotoRepository.GetAll());
            AccommodationReservationRepository.LinkAccommodations(AccommodationRepository.GetAll());
            AccommodationOwnerRatingRepository.LinkReservations(AccommodationReservationRepository.GetAll());
        }

        public bool IsSuperOwner(User owner)
        {
            return AccommodationOwnerRatingRepository.GetByOwner(owner).Count >= 0 && GetAverageRatingForOwner(owner) >= 4.5;
        }

        public double GetAverageRatingForOwner(User owner)
        {
            var ratings = AccommodationOwnerRatingRepository.GetByOwner(owner);

            double averageRating = 0;
            foreach (var rating in ratings)
            {
                double currentRating = (double)(rating.AccommodationCleanliness +
                                       rating.AccommodationComfort +
                                       rating.AccommodationLocation +
                                       rating.OwnerCorrectness +
                                       rating.OwnerResponsiveness) / 5;
                averageRating += currentRating;
            }

            averageRating /= ratings.Count;

            return averageRating;
        }

        public void SetSuperOwners()
        {
            foreach (var user in UserRepository.GetOwners())
            {
                if (IsSuperOwner(user))
                {
                    user.IsSuperOwner = true;
                }
                else
                {
                    user.IsSuperOwner = false;
                }
            }

            UserRepository.UpdateSuperOwners();
        }

        public List<Accommodation> SortBySuperOwnersFirst(List<Accommodation> accommodations)
        {
            var a = new List<Accommodation>(accommodations);
            List<Accommodation> sortedAccommodations = new List<Accommodation>();

            foreach (var accommodation in accommodations)
            {
                if (accommodation.Owner.IsSuperOwner)
                {
                    sortedAccommodations.Add(accommodation);
                    a.Remove(accommodation);
                }
            }

            foreach (var accommodation in a)
            {
                sortedAccommodations.Add(accommodation);
            }

            return sortedAccommodations;
        }
    }
}
