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
    public class RenovationService
    {
        public IRenovationRecommendationRepository RecommendationRepository { get; set; }
        public IAccommodationOwnerRatingRepository RatingRepository { get; set; }


        public RenovationService()
        {
            RecommendationRepository = Injector.Injector.CreateInstance<IRenovationRecommendationRepository>();
            RatingRepository = Injector.Injector.CreateInstance<IAccommodationOwnerRatingRepository>();
            RatingRepository.LinkRenovationRecommendations(RecommendationRepository.GetAll());
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
    }
}
