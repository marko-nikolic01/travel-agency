using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Injector;
using TravelAgency.Repositories;

namespace TravelAgency.Services
{
    public class RenovationService
    {
        public IUserRepository UserRepository { get; set; }
        public ILocationRepository LocationRepository { get; set; }
        public IAccommodationRepository AccommodationRepository { get; set; }
        public IAccommodationPhotoRepository AccommodationPhotoRepository { get; set; }
        public IAccommodationRenovationRepository RenovationRepository { get; set; }
        public IRenovationRecommendationRepository RecommendationRepository { get; set; }
        public IAccommodationOwnerRatingRepository RatingRepository { get; set; }


        public RenovationService()
        {
            UserRepository = Injector.Injector.CreateInstance<IUserRepository>();
            LocationRepository = Injector.Injector.CreateInstance<ILocationRepository>();
            AccommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
            AccommodationPhotoRepository = Injector.Injector.CreateInstance<IAccommodationPhotoRepository>();
            RenovationRepository = Injector.Injector.CreateInstance<IAccommodationRenovationRepository>();
            RecommendationRepository = Injector.Injector.CreateInstance<IRenovationRecommendationRepository>();
            RatingRepository = Injector.Injector.CreateInstance<IAccommodationOwnerRatingRepository>();

            AccommodationRepository.LinkOwners(UserRepository.GetAll());
            AccommodationRepository.LinkPhotos(AccommodationPhotoRepository.GetAll());
            AccommodationRepository.LinkLocations(LocationRepository.GetAll());
            RatingRepository.LinkRenovationRecommendations(RecommendationRepository.GetAll());
            RenovationRepository.LinkAccommodations(AccommodationRepository.GetAll());
        }

        public bool RecommendRenovation(AccommodationOwnerRating rating, RenovationRecommendation recommendation)
        {
            if (recommendation.IsValid)
            {
                recommendation.RatingId = rating.Id;
                RecommendationRepository.Save(recommendation);
                rating.RenovationReccommendationId = recommendation.Id;
                rating.RenovationRecommendation = recommendation;
                return true;
            }
            return false;
        }

        public List<AccommodationRenovation> GetPastRenovationsByOwner(User owner)
        {
            var renovations = RenovationRepository.GetAll();
            var filtered = new List<AccommodationRenovation>();

            foreach (var renovation in renovations)
            {
                if (!IsRenovationActive(renovation))
                {
                    filtered.Add(renovation);
                }
            }

            return filtered;
        }

        public List<AccommodationRenovation> GetScheduledRenovationsByOwner(User owner)
        {
            var renovations = RenovationRepository.GetByOwner(owner);
            var filtered = new List<AccommodationRenovation>();

            foreach (var renovation in renovations)
            {
                if (IsRenovationActive(renovation))
                {
                    filtered.Add(renovation);
                }
            }

            return filtered;
        }

        private bool IsRenovationActive(AccommodationRenovation renovation)
        {
            return DateOnly.FromDateTime(DateTime.Now).CompareTo(renovation.DateSpan.EndDate) < 0;
        }

        public void CancelRenovation(AccommodationRenovation renovation)
        {
            RenovationRepository.Delete(renovation);
        }
    }
}
